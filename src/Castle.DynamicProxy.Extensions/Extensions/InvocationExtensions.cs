// -----------------------------------------------------------------------
// <copyright file="InvocationExtensions.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Castle.DynamicProxy
{
  /// <summary>
  /// Provides extension methods for IInvocation to support asynchronous interception scenarios.
  /// </summary>
  /// <remarks>These methods enable interceptors to correctly handle asynchronous method invocations, ensuring
  /// that asynchronous return values such as Task and ValueTask are properly awaited. Use these extensions within
  /// asynchronous interceptors to maintain correct execution flow and result propagation.</remarks>
  public static class InvocationExtensions
  {
    /// <summary>
    /// Asynchronously proceeds to the next interceptor or target method in the invocation pipeline.
    /// </summary>
    /// <remarks>This method should be used within asynchronous interceptors to ensure that asynchronous
    /// return values are properly awaited. It supports both Task and ValueTask return types from the target
    /// method.</remarks>
    /// <param name="invocation">The invocation context representing the current method call. Cannot be <see langword="null"/>.</param>
    /// <returns>A <seealso cref="ValueTask"/> that represents the asynchronous operation. The task completes when the invocation and any
    /// asynchronous return value have finished executing.</returns>
    public static async ValueTask ProceedAsync(this IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);

      invocation.CaptureProceedInfo().Invoke();

      ValueTask awaitable = invocation.ReturnValue switch
      {
        Task t => new ValueTask(t),
        ValueTask vt => vt,
        _ => default
      };

      await awaitable.ConfigureAwait(false);
    }

    /// <summary>
    /// Invokes the intercepted method asynchronously and returns its result as a value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <remarks>This method is intended for use with asynchronous method interceptions where the intercepted
    /// method returns either a <seealso cref="Task{TResult}"/> or a <seealso cref="ValueTask{TResult}"/>.
    /// If the intercepted method does not return a compatible asynchronous result 
    /// (<seealso cref="Task{TResult}"/> or <seealso cref="ValueTask{TResult}"/>), the default value of <typeparamref name="TResult"/>
    /// is returned. For reference types, this is null; for value types, this is the 
    /// zero-initialized value.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result returned by the intercepted asynchronous method.</typeparam>
    /// <param name="invocation">The invocation context representing the method call to proceed with. Cannot be <see langword="null"/>.</param>
    /// <returns>A <seealso cref="ValueTask{TResult}"/> that represents the asynchronous operation. The task result contains the value returned by the
    /// intercepted method, or the default value of <typeparamref name="TResult"/> if the method does not return a value.</returns>
    public static async ValueTask<TResult?> ProceedAsync<TResult>(this IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);

      invocation.CaptureProceedInfo().Invoke();

      return invocation.ReturnValue switch
      {
        Task<TResult> task => await task.ConfigureAwait(false),
        ValueTask<TResult> valueTask => await valueTask.ConfigureAwait(false),
        _ => default
      };
    }
  }
}
