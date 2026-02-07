// -----------------------------------------------------------------------
// <copyright file="AsyncInterceptorProcessor.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Castle.DynamicProxy.Extensions
{
  /// <summary>
  /// Provides an implementation of the IInterceptor interface that delegates interception logic to an asynchronous
  /// interceptor, enabling support for both synchronous and asynchronous method invocations.
  /// </summary>
  /// <remarks>
  /// AsyncInterceptorProcessor adapts an IAsyncInterceptor to work with interception frameworks that
  /// expect IInterceptor implementations. It automatically detects whether the intercepted method is 
  /// synchronous or asynchronous and routes the invocation accordingly. Handler methods are cached 
  /// per return type using a thread-safe concurrent dictionary for optimal performance. This allows 
  /// seamless interception of methods returning Task, ValueTask, or their generic counterparts, as well 
  /// as synchronous methods. This class is typically used in scenarios where asynchronous interception 
  /// is required but the interception infrastructure is synchronous by default.
  /// </remarks>
  internal sealed class AsyncInterceptorProcessor : IInterceptor
  {
    private enum MethodType
    {
      /// <summary>Synchronous method (no <see cref="Task" />/<see cref="ValueTask"/> return).</summary>
      Synchronous,
      /// <summary>Async method returning <see cref="Task" /> (no result).</summary>
      AsyncAction,
      /// <summary>Async method returning <see cref="Task{TResult}"/> (with result).</summary>
      AsyncFunction,
      /// <summary>Async method returning <see cref="ValueTask"/> (no result).</summary>
      AsyncValueAction,
      /// <summary>Async method returning <see cref="ValueTask{TResult}"/> (with result).</summary>
      AsyncValueFunction,
    }

    // Reflection-only lambda - parameters never executed at runtime
    private static readonly MethodInfo _invocationHandler = GetMethodInfo(() => HandleAsyncInvocation(null!, null!, default))
      ?? throw new InvalidOperationException($"Unable to retrieve {nameof(HandleAsyncInvocation)} method info.");

    private static readonly MethodInfo _genericResultInvocationHandler = (GetMethodInfo(() => HandleAsyncInvocation<object>(null!, null!, default))
      ?? throw new InvalidOperationException($"Unable to retrieve generic {nameof(HandleAsyncInvocation)} method info."))
      .GetGenericMethodDefinition()
      ?? throw new InvalidOperationException($"Unable to create generic method definition for {nameof(HandleAsyncInvocation)}.");

    private readonly IAsyncInterceptor _asyncInterceptor;
    private readonly ConcurrentDictionary<Type, Action<IInvocation>> _handlers = new();

    public AsyncInterceptorProcessor(IAsyncInterceptor interceptor)
    {
      ArgumentNullException.ThrowIfNull(interceptor);
      _asyncInterceptor = interceptor;
    }

    /// <inheritdoc/>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);

      Type returnType = (invocation.GetConcreteMethodInvocationTarget()
        ?? invocation.MethodInvocationTarget
        ?? invocation.Method
        ?? throw new InvalidOperationException("Unable to determine target method for invocation."))
        .ReturnType;

      Action<IInvocation> handler = _handlers.GetOrAdd(returnType, CreateHandler);
      handler.Invoke(invocation);
    }

    private Action<IInvocation> CreateHandler(Type methodReturnType)
    {
      MethodType methodType = GetMethodType(methodReturnType);

      return methodType switch
      {
        MethodType.AsyncValueAction or MethodType.AsyncAction => CompileAsyncHandler(_invocationHandler, methodType),
        MethodType.AsyncValueFunction or MethodType.AsyncFunction => CompileAsyncHandler(_genericResultInvocationHandler.MakeGenericMethod(methodReturnType.GenericTypeArguments), methodType),
        MethodType.Synchronous => _asyncInterceptor.Intercept,
        _ => throw new InvalidOperationException($"Unable to find invocation handler for method type: {methodType}")
      };
    }

    private Action<IInvocation> CompileAsyncHandler(MethodInfo handlerMethod, MethodType methodType)
    {
      // Parameter: IInvocation invocation
      ParameterExpression invocationParam = Expression.Parameter(typeof(IInvocation), "invocation");

      // Create constant expressions for the captured variables
      ConstantExpression asyncInterceptorConst = Expression.Constant(_asyncInterceptor, typeof(IAsyncInterceptor));
      ConstantExpression methodTypeConst = Expression.Constant(methodType, typeof(MethodType));

      // Build the method call: HandleAsyncInvocation(invocation, _asyncInterceptor, methodType)
      MethodCallExpression methodCall = Expression.Call(
        handlerMethod,
        invocationParam,
        asyncInterceptorConst,
        methodTypeConst);

      // Compile the lambda: (IInvocation invocation) => HandleAsyncInvocation(invocation, _asyncInterceptor, methodType)
      var lambda = Expression.Lambda<Action<IInvocation>>(methodCall, invocationParam);

      return lambda.Compile();
    }

    /// <summary>
    /// Extracts <see cref="MethodInfo"/> from a lambda expression by analyzing its method call body.
    /// </summary>
    /// <param name="expression">The lambda expression representing a method call.</param>
    /// <returns>The <see cref="MethodInfo"/> of the called method.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the expression body is not a method call.</exception>
    private static MethodInfo GetMethodInfo(LambdaExpression expression) =>
      expression.Body switch
      {
        MethodCallExpression methodCall => methodCall.Method,
        _ => throw new InvalidOperationException("Expression must be a method call.")
      };

    private static MethodType GetMethodType(Type methodReturnType) =>
      methodReturnType switch
      {
        Type t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Task<>) => MethodType.AsyncFunction,
        Type t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ValueTask<>) => MethodType.AsyncValueFunction,
        Type t when t == typeof(Task) => MethodType.AsyncAction,
        Type t when t == typeof(ValueTask) => MethodType.AsyncValueAction,
        _ => MethodType.Synchronous
      };

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Necessary for async exception results")]
    private static void HandleAsyncInvocation(IInvocation invocation, IAsyncInterceptor asyncInterceptor, MethodType methodType)
    {
      ValueTask result;

      try
      {
        result = asyncInterceptor.InterceptAsync(invocation);
      }
      catch (Exception ex)
      {
        result = ValueTask.FromException(ex);
      }

      invocation.ReturnValue = methodType == MethodType.AsyncAction ? result.AsTask() : result;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Necessary for async exception results")]
    private static void HandleAsyncInvocation<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor, MethodType methodType)
    {
      ValueTask<TResult?> result;

      try
      {
        result = asyncInterceptor.InterceptAsync<TResult>(invocation);
      }
      catch (Exception ex)
      {
        result = ValueTask.FromException<TResult?>(ex);
      }

      invocation.ReturnValue = methodType == MethodType.AsyncFunction ? result.AsTask() : result;
    }
  }
}