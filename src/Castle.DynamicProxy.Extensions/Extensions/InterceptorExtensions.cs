// -----------------------------------------------------------------------
// <copyright file="InterceptorExtensions.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy.Extensions;

namespace Castle.DynamicProxy
{
  /// <summary>
  /// Provides extension methods for converting asynchronous interceptors to synchronous interceptors.
  /// </summary>
  /// <remarks>These methods enable integration of asynchronous interceptor implementations with APIs that
  /// require synchronous interceptors. The extensions are intended to simplify interoperability between asynchronous
  /// and synchronous interception patterns.</remarks>
  public static class InterceptorExtensions
  {
    /// <summary>
    /// Converts an asynchronous interceptor to a synchronous interceptor that can be used with APIs expecting an
    /// <seealso cref="IInterceptor"/> instance.
    /// </summary>
    /// <param name="asyncInterceptor">The asynchronous interceptor to adapt. Cannot be <see langword="null"/>.</param>
    /// <returns>An <seealso cref="IInterceptor"/> instance that delegates calls to the specified asynchronous interceptor.</returns>
    public static IInterceptor ToInterceptor(this IAsyncInterceptor asyncInterceptor) =>
      new AsyncInterceptorProcessor(asyncInterceptor);

    /// <summary>
    /// Converts a collection of asynchronous interceptors to their synchronous interceptor equivalents.
    /// </summary>
    /// <param name="asyncInterceptors">The collection of asynchronous interceptors to convert. Can be <see langword="null"/>.</param>
    /// <returns>An enumerable collection of synchronous interceptors. Returns an empty collection if <paramref
    /// name="asyncInterceptors"/> is null.</returns>
    public static IEnumerable<IInterceptor> ToInterceptors(this IEnumerable<IAsyncInterceptor> asyncInterceptors) =>
      asyncInterceptors?.Select(ToInterceptor) ?? [];
  }
}
