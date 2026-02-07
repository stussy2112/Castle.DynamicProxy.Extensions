// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensions.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Castle.DynamicProxy.Extensions;

namespace Castle.DynamicProxy
{
  /// <summary>
  /// Provides extension methods for IProxyGenerator to simplify the creation of class and interface proxies with
  /// asynchronous interceptors.
  /// </summary>
  /// <remarks>These extension methods enable convenient creation of proxies for classes and interfaces using
  /// asynchronous interceptors, supporting a variety of proxying scenarios such as proxying with or without a target,
  /// specifying additional interfaces, and customizing proxy generation options. All methods require a non-null
  /// IProxyGenerator instance. Null arguments for required parameters will result in an <see cref="ArgumentNullException"/>. These
  /// methods are intended to streamline integration with asynchronous interception patterns.</remarks>
  public static partial class ProxyGeneratorExtensions
  {
    /// <summary>
    /// Creates a proxy instance of the specified class type and applies the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This method uses the default proxy generation options and does not add additional interfaces
    /// to the proxy. The returned proxy will invoke the provided asynchronous interceptors for intercepted method
    /// calls.</remarks>
    /// <typeparam name="TClass">The class type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Can be empty.</param>
    /// <returns>A proxy instance of type TClass with the specified asynchronous interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxy<TClass>(this IProxyGenerator proxyGenerator, params IAsyncInterceptor[] asyncInterceptors) where TClass : class =>
      proxyGenerator.CreateClassProxy<TClass>(ProxyGenerationOptions.Default, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type and applies the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This method is an extension for IProxyGenerator that simplifies the creation of class proxies
    /// with asynchronous interceptors. The returned proxy will invoke the provided interceptors for eligible method
    /// calls.</remarks>
    /// <typeparam name="TClass">The type of class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain null elements.</param>
    /// <returns>A proxy instance of type TClass with the specified asynchronous interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxy<TClass>(this IProxyGenerator proxyGenerator, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) where TClass : class =>
      proxyGenerator.CreateClassProxy<TClass>(options, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type that intercepts asynchronous method calls using the
    /// provided interceptors.
    /// </summary>
    /// <remarks>The created proxy will intercept asynchronous methods and delegate interception logic to the
    /// provided interceptors. Synchronous methods are not intercepted by IAsyncInterceptor instances.</remarks>
    /// <typeparam name="TClass">The class type to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TClass with the specified interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxy<TClass>(this IProxyGenerator proxyGenerator, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) where TClass : class =>
      proxyGenerator.CreateClassProxy<TClass>(ProxyGenerationOptions.Default, constructorArguments, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type, applying the given asynchronous interceptors and using the
    /// provided constructor arguments.
    /// </summary>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. At least one interceptor must be provided.</param>
    /// <returns>A proxy instance of type TClass with the specified interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxy<TClass>(this IProxyGenerator proxyGenerator, ProxyGenerationOptions options, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) where TClass : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateClassProxy<TClass>(options, constructorArguments, [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a proxy instance of the specified class type and applies the given asynchronous interceptors.
    /// </summary>
    /// <remarks>The returned proxy instance will have the specified asynchronous interceptors applied to all
    /// virtual methods. Use this method when you do not need to specify constructor arguments or additional proxy
    /// options.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object representing the proxied instance of the specified class type.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxy(classToProxy, Type.EmptyTypes, ProxyGenerationOptions.Default, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type, applying the given proxy generation options and
    /// asynchronous interceptors.
    /// </summary>
    /// <remarks>The returned proxy instance will implement the same public and protected members as the
    /// original class, with method calls intercepted by the provided asynchronous interceptors. Use this method when
    /// you need to intercept asynchronous methods on a class without specifying constructor arguments.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object representing the generated proxy instance of the specified class type.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxy(classToProxy, Type.EmptyTypes, options, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type, optionally implementing additional interfaces and applying
    /// the given asynchronous interceptors.
    /// </summary>
    /// <remarks>The returned proxy instance can be cast to the specified class type and any additional
    /// interfaces provided. Asynchronous interceptors allow interception of asynchronous method calls on the
    /// proxy.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract class type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are
    /// required.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy of the specified class type, implementing any additional interfaces and
    /// applying the provided asynchronous interceptors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, Type[]? additionalInterfacesToProxy, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxy(classToProxy, additionalInterfacesToProxy, ProxyGenerationOptions.Default, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type, applying the given asynchronous interceptors and using the
    /// provided constructor arguments.
    /// </summary>
    /// <remarks>The returned proxy instance can be cast to the specified class type. Use this method to
    /// enable interception of virtual methods for classes that do not implement interfaces.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the constructor of the proxied class. Can be <see langword="null"/> or empty if the default
    /// constructor should be used.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy of the specified class type with the provided interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxy(classToProxy, Type.EmptyTypes, ProxyGenerationOptions.Default, constructorArguments, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type, applying the given proxy generation options,
    /// constructor arguments, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method allows you to specify both proxy generation options and constructor arguments
    /// while creating a class proxy. The returned proxy will invoke the provided asynchronous interceptors for
    /// intercepted method calls.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object representing the proxied instance of the specified class type.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, ProxyGenerationOptions options, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxy(classToProxy, Type.EmptyTypes, options, constructorArguments, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type, optionally implementing additional interfaces and
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>The returned proxy can be cast to the specified class type and any additional interfaces
    /// provided. This method combines interface implementation with custom proxy options and asynchronous
    /// interception.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy of the specified class type, implementing any additional interfaces and applying the provided options and interceptors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxy(classToProxy, additionalInterfacesToProxy, options, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance of the specified class type with full customization options, including additional
    /// interfaces, proxy generation options, constructor arguments, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This is the most comprehensive overload for class proxy creation, allowing complete control
    /// over all aspects of proxy generation. The returned proxy can be cast to the class type and any additional
    /// interfaces specified.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class. Cannot be <see langword="null"/>.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy of the specified class type with all customizations applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/>, or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxy(this IProxyGenerator proxyGenerator, Type classToProxy, Type[]? additionalInterfacesToProxy, ProxyGenerationOptions options, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors)
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(classToProxy);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateClassProxy(classToProxy, additionalInterfacesToProxy, options, constructorArguments, [.. asyncInterceptors.ToInterceptors()]);
    }
  }
}