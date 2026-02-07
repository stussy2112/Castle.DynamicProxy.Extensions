// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensions.CreateInterfaceProxyWithoutTarget.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Castle.DynamicProxy.Extensions
{
  public static partial class ProxyGeneratorExtensions
  {
    /// <summary>
    /// Creates a proxy instance for the specified interface type without a target, applying the given asynchronous
    /// interceptors.
    /// </summary>
    /// <remarks>This creates an interface proxy without a backing implementation. All method calls must be
    /// fully handled by the provided interceptors. This is useful for creating mock objects or implementing
    /// interfaces dynamically.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The interface type to proxy. Must be an interface type.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interface with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="interfaceToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithoutTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceToProxy, Type.EmptyTypes, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type without a target, optionally implementing additional
    /// interfaces and applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates an interface proxy that implements multiple interfaces without a backing target.
    /// All method calls must be fully handled by the provided interceptors. The proxy can be cast to any of the
    /// implemented interfaces.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The primary interface type to proxy. Must be an interface type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interfaces with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="interfaceToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithoutTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, Type[]? additionalInterfacesToProxy, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type without a target, applying the given proxy generation
    /// options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates an interface proxy without a backing implementation but with custom generation
    /// options. All method calls must be fully handled by the provided interceptors.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The interface type to proxy. Must be an interface type.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interface with the options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithoutTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceToProxy, Type.EmptyTypes, options, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type without a target, with full customization including
    /// additional interfaces, proxy generation options, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This is the most comprehensive overload for creating interface proxies without a target. The
    /// proxy implements multiple interfaces without a backing implementation, and all method calls must be fully
    /// handled by the provided interceptors.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The primary interface type to proxy. Must be an interface type. Cannot be <see langword="null"/>.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interfaces with all customizations applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/>, or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithoutTarget(this IProxyGenerator proxyGenerator, Type interfaceToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors)
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(interfaceToProxy);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, options, [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a proxy instance for the specified interface type without a target, applying the given asynchronous
    /// interceptors.
    /// </summary>
    /// <remarks>This creates a strongly-typed interface proxy without a backing implementation. All method
    /// calls must be fully handled by the provided interceptors. This is useful for creating type-safe mock objects
    /// or dynamic implementations.</remarks>
    /// <typeparam name="TInterface">The interface type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TInterface with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> is <see langword="null"/>.</exception>
    public static TInterface CreateInterfaceProxyWithoutTarget<TInterface>(this IProxyGenerator proxyGenerator, params IAsyncInterceptor[] asyncInterceptors) where TInterface : class =>
      proxyGenerator.CreateInterfaceProxyWithoutTarget<TInterface>(ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type without a target, applying the given proxy generation
    /// options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates a strongly-typed interface proxy without a backing implementation but with custom
    /// generation options. All method calls must be fully handled by the provided interceptors.</remarks>
    /// <typeparam name="TInterface">The interface type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TInterface with the options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TInterface CreateInterfaceProxyWithoutTarget<TInterface>(this IProxyGenerator proxyGenerator, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) where TInterface : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateInterfaceProxyWithoutTarget<TInterface>(options, [.. asyncInterceptors.ToInterceptors()]);
    }
  }
}
