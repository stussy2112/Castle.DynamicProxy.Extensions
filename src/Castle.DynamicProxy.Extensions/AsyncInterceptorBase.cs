// -----------------------------------------------------------------------
// <copyright file="AsyncInterceptorBase.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Castle.DynamicProxy.Extensions
{
  /// <summary>
  /// Provides a base class for implementing asynchronous method interception using the IAsyncInterceptor and
  /// IInterceptor interfaces.
  /// </summary>
  /// <remarks>Derive from this class to create custom interceptors that support both synchronous and
  /// asynchronous method interception. This class handles the integration between synchronous and asynchronous
  /// interception patterns, allowing implementers to focus on the interception logic by overriding the abstract
  /// methods. Thread safety and reentrancy depend on the implementation of the derived interceptor.</remarks>
  public abstract class AsyncInterceptorBase : IAsyncInterceptor, IInterceptor
  {
    private readonly AsyncInterceptorProcessor _interceptorProcessor;

    /// <summary>
    /// Initializes a new instance of the AsyncInterceptorBase class.
    /// </summary>
    /// <remarks>This protected constructor is intended to be called by derived classes to set up the base
    /// functionality required for asynchronous interception. It should not be called directly by user code.</remarks>
    protected AsyncInterceptorBase() =>
      _interceptorProcessor = new AsyncInterceptorProcessor(this);

    void IInterceptor.Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      _interceptorProcessor.Intercept(invocation);
    }

    /// <inheritdoc />
    public abstract void Intercept(IInvocation invocation);

    /// <inheritdoc />
    public abstract ValueTask InterceptAsync(IInvocation invocation);

    /// <inheritdoc />
    public abstract ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation);
  }
}