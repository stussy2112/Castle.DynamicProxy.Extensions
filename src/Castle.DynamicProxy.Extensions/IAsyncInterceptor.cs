// -----------------------------------------------------------------------
// <copyright file="IAsyncInterceptor.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

namespace Castle.DynamicProxy.Extensions
{
  /// <summary>
  /// Defines methods for intercepting synchronous and asynchronous method invocations in dynamic proxy scenarios.
  /// </summary>
  /// <remarks>Implement this interface to provide custom logic that can be executed before, after, or instead
  /// of the target method invocation, supporting both synchronous and asynchronous patterns. This is typically used in
  /// aspect-oriented programming to add cross-cutting concerns such as logging, caching, or transaction management to
  /// method calls. The interface extends IInterceptor to ensure compatibility with synchronous interception.</remarks>
  public interface IAsyncInterceptor
  {
    /// <summary>
    /// Intercepts a method invocation and allows custom behavior to be applied before or after the target method is
    /// called.
    /// </summary>
    /// <remarks>Implement this method to define how method calls are intercepted and handled. Typical
    /// scenarios include logging, validation, or modifying arguments and return values. The implementation should call
    /// invocation.Proceed() to invoke the original method unless the interception is intended to suppress or replace
    /// the call.</remarks>
    /// <param name="invocation">The method invocation information, including the target object, method, and arguments. Cannot be null.</param>
    void Intercept(IInvocation invocation);

    /// <summary>
    /// Intercepts the specified method invocation asynchronously, allowing custom logic to be executed before or after
    /// the target method is called.
    /// </summary>
    /// <remarks>Use this method to implement cross-cutting concerns such as logging, validation, or
    /// authorization in an asynchronous context. The interception can modify arguments, prevent the target method from
    /// executing, or alter the return value as needed.</remarks>
    /// <param name="invocation">The method invocation to intercept. Provides access to the target method, arguments, and the ability to proceed
    /// with or short-circuit the invocation.</param>
    /// <returns>A ValueTask that represents the asynchronous interception operation.</returns>
    ValueTask InterceptAsync(IInvocation invocation);

    /// <summary>
    /// Intercepts the specified method invocation asynchronously and returns a result of the specified type.
    /// </summary>
    /// <typeparam name="TResult">The type of the result expected from the intercepted invocation. Can be a reference or value type.</typeparam>
    /// <param name="invocation">The method invocation to intercept. Provides access to the target method, arguments, and context for the
    /// interception.</param>
    /// <returns>A value task that represents the asynchronous interception operation. The result contains the value returned by
    /// the intercepted method, or null if the method has no return value or the interception yields no result.</returns>
    ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation);
  }
}
