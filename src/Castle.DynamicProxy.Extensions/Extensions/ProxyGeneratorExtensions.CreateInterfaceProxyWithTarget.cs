// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensions.CreateInterfaceProxyWithTarget.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Castle.DynamicProxy.Extensions
{
  public static partial class ProxyGeneratorExtensions
  {
    /// <summary>
    /// Creates a proxy instance for the specified interface type that forwards calls to the provided target instance,
    /// applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates a strongly-typed interface proxy that delegates method calls to the target
    /// implementation. The proxy will intercept calls via the provided asynchronous interceptors before delegating
    /// to the target.</remarks>
    /// <typeparam name="TInterface">The interface type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TInterface that delegates to the target with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="target"/> is <see langword="null"/>.</exception>
    public static TInterface CreateInterfaceProxyWithTarget<TInterface>(this IProxyGenerator proxyGenerator, TInterface target, params IAsyncInterceptor[] asyncInterceptors) where TInterface : class =>
      proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that forwards calls to the provided target instance,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates a strongly-typed interface proxy with custom generation options that delegates
    /// method calls to the target implementation. The proxy will intercept calls via the provided asynchronous
    /// interceptors before delegating to the target.</remarks>
    /// <typeparam name="TInterface">The interface type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TInterface that delegates to the target with the options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="target"/>, or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TInterface CreateInterfaceProxyWithTarget<TInterface>(this IProxyGenerator proxyGenerator, TInterface target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) where TInterface : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(target);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateInterfaceProxyWithTarget(target, options, [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a proxy instance for the specified interface type that forwards calls to the provided target instance,
    /// applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates an interface proxy that delegates method calls to the target implementation. The
    /// proxy will intercept calls via the provided asynchronous interceptors before delegating to the target.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The interface type to proxy. Must be an interface type.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interface and delegates to the target with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/> or <paramref name="target"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, object target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that forwards calls to the provided target instance,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates an interface proxy with custom generation options that delegates method calls to
    /// the target implementation. The proxy will intercept calls via the provided asynchronous interceptors before
    /// delegating to the target.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The interface type to proxy. Must be an interface type.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interface and delegates to the target with the options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/>, <paramref name="target"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, object target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, Type.EmptyTypes, target, options, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that forwards calls to the provided target instance,
    /// optionally implementing additional interfaces and applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates an interface proxy that delegates method calls to the target while also
    /// implementing additional interfaces. The proxy can be cast to any of the implemented interfaces and will
    /// intercept calls via the provided asynchronous interceptors.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The primary interface type to proxy. Must be an interface type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interfaces and delegates to the target with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/> or <paramref name="target"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, Type[]? additionalInterfacesToProxy, object target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that forwards calls to the provided target instance,
    /// with full customization including additional interfaces, proxy generation options, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This is the most comprehensive overload for target-delegating interface proxy creation. The
    /// proxy implements multiple interfaces, delegates to the target, and supports custom generation options and
    /// asynchronous interception.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The primary interface type to proxy. Must be an interface type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interfaces and delegates to the target with all customizations applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/>, <paramref name="target"/>, or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, Type[]? additionalInterfacesToProxy, object target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors)
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(interfaceToProxy);
      ArgumentNullException.ThrowIfNull(target);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, additionalInterfacesToProxy, target, options, [.. asyncInterceptors.ToInterceptors()]);
    }
  }
}
