// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensions.CreateInterfaceProxyWithTargetInterface.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Castle.DynamicProxy.Extensions
{
  /// <summary>
  /// Provides extension methods for creating interface proxies that maintain a reference to the target as the interface
  /// type, supporting asynchronous interceptors and dynamic target switching.
  /// </summary>
  /// <remarks>These extension methods enable advanced proxy generation scenarios using Castle DynamicProxy,
  /// allowing callers to create proxies that can change their target at runtime and apply asynchronous interception
  /// logic. Overloads support customization with additional interfaces and proxy generation options. All methods
  /// require a non-null proxy generator and enforce argument validation for safe usage.</remarks>
  public static partial class ProxyGeneratorExtensions
  {

    /// <summary>
    /// Creates a proxy instance for the specified interface type that maintains the target interface reference,
    /// applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>Unlike CreateInterfaceProxyWithTarget, this method maintains a reference to the target as the
    /// interface type, allowing the target to be changed at runtime. This is useful for scenarios requiring dynamic
    /// target switching.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The interface type to proxy. Must be an interface type.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interface and delegates to the target with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="interfaceToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTargetInterface(this IProxyGenerator proxyGenerator, Type interfaceToProxy, object? target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that maintains the target interface reference,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a proxy with custom generation options that maintains a reference to the
    /// target as the interface type. This allows the target to be changed at runtime while supporting custom proxy
    /// options.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The interface type to proxy. Must be an interface type.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interface and delegates to the target with the options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/>, <paramref name="target"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTargetInterface(this IProxyGenerator proxyGenerator, Type interfaceToProxy, object? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, Type.EmptyTypes, target, options, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that maintains the target interface reference,
    /// optionally implementing additional interfaces and applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a proxy that maintains a reference to the target as the interface type
    /// while also implementing additional interfaces. The proxy can be cast to any of the implemented interfaces and
    /// allows runtime target switching.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The primary interface type to proxy. Must be an interface type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interfaces and delegates to the target with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="interfaceToProxy"/> or <paramref name="target"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTargetInterface(this IProxyGenerator proxyGenerator, Type interfaceToProxy, Type[]? additionalInterfacesToProxy, object? target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that maintains the target interface reference,
    /// with full customization including additional interfaces, proxy generation options, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This is the most comprehensive overload for creating interface proxies with target interface
    /// reference. The proxy maintains a reference to the target as the interface type, implements multiple interfaces,
    /// and supports custom generation options while allowing runtime target switching.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="interfaceToProxy">The primary interface type to proxy. Must be an interface type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that implements the specified interfaces and delegates to the target with all customizations applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="target"/>, or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateInterfaceProxyWithTargetInterface(this IProxyGenerator proxyGenerator, Type interfaceToProxy, Type[]? additionalInterfacesToProxy, object? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors)
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(target);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, target, options, [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a proxy instance for the specified interface type that maintains the target interface reference,
    /// applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates a strongly-typed interface proxy that maintains a reference to the target as the
    /// interface type. This allows the target to be changed at runtime while providing type-safe access. This is
    /// useful for scenarios requiring dynamic target switching.</remarks>
    /// <typeparam name="TInterface">The interface type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TInterface that delegates to the target with the interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="target"/> is <see langword="null"/>.</exception>
    public static TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(this IProxyGenerator proxyGenerator, TInterface? target, params IAsyncInterceptor[] asyncInterceptors) where TInterface : class =>
      proxyGenerator.CreateInterfaceProxyWithTargetInterface(target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified interface type that maintains the target interface reference,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This creates a strongly-typed interface proxy with custom generation options that maintains a
    /// reference to the target as the interface type. This allows the target to be changed at runtime while providing
    /// type-safe access and supporting custom proxy options.</remarks>
    /// <typeparam name="TInterface">The interface type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the interface proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TInterface that delegates to the target with the options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="target"/>, or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(this IProxyGenerator proxyGenerator, TInterface? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) where TInterface : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(target);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateInterfaceProxyWithTargetInterface(target, options, [.. asyncInterceptors.ToInterceptors()]);
    }
  }
}
