// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensions.CreateClassProxyWithTarget.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Castle.DynamicProxy.Extensions
{
  public static partial class ProxyGeneratorExtensions
  {
    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a proxy that delegates method calls to the target object while allowing
    /// interception via asynchronous interceptors. Use this when you want to add cross-cutting concerns to an
    /// existing object.</remarks>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TClass that delegates to the target with the specified interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxyWithTarget<TClass>(this IProxyGenerator proxyGenerator, TClass? target, params IAsyncInterceptor[] asyncInterceptors) where TClass : class =>
      proxyGenerator.CreateClassProxyWithTarget(target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a proxy with custom generation options that delegates method calls to the
    /// target object. The proxy will invoke the provided asynchronous interceptors for intercepted method calls.</remarks>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Cannot be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TClass that delegates to the target with the specified options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxyWithTarget<TClass>(this IProxyGenerator proxyGenerator, TClass? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) where TClass : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateClassProxyWithTarget(target, options, [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// optionally implementing additional interfaces and applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>The returned proxy delegates method calls to the target object and can be cast to the class
    /// type and any additional interfaces specified. This is useful for adding interface implementations to existing
    /// objects.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target, implementing any additional interfaces and applying the provided interceptors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given proxy generation options, constructor arguments, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method allows specification of both proxy options and constructor arguments when creating
    /// a target-delegating proxy. The proxy will intercept method calls before delegating to the target.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target with the specified options, constructor arguments, and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, object? target, ProxyGenerationOptions options, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, options, constructorArguments, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// using the given constructor arguments and applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a target-delegating proxy that requires specific constructor arguments.
    /// The proxy will intercept method calls via the provided asynchronous interceptors before delegating to the
    /// target.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target with the specified constructor arguments and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, object? target, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, constructorArguments, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This is the simplest overload for creating a target-delegating class proxy. The proxy will
    /// intercept method calls via the provided asynchronous interceptors before delegating to the target object.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target with the specified interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, object? target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a target-delegating proxy with custom generation options. The proxy will
    /// intercept method calls via the provided asynchronous interceptors before delegating to the target.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target with the specified options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, object? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(classToProxy, Type.EmptyTypes, target, options, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// optionally implementing additional interfaces and applying the given proxy generation options and asynchronous
    /// interceptors.
    /// </summary>
    /// <remarks>This method combines interface implementation with custom proxy options and target delegation.
    /// The returned proxy can be cast to the class type and any additional interfaces specified.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target, implementing any additional interfaces and applying the provided options and interceptors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, options, [], asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// with full customization including additional interfaces, proxy generation options, constructor arguments, and
    /// asynchronous interceptors.
    /// </summary>
    /// <remarks>This is the most comprehensive overload for target-delegating class proxy creation, allowing
    /// complete control over all aspects of proxy generation. The returned proxy delegates to the target while
    /// supporting additional interfaces and custom options.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class. Cannot be <see langword="null"/>.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target with all customizations applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, ProxyGenerationOptions options, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors)
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(classToProxy);
      ArgumentNullException.ThrowIfNull(options);
      return proxyGenerator.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, options, constructorArguments, [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a class proxy for the specified target instance, applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>This method uses the default proxy generation options. The proxy will forward calls to the
    /// target instance and apply the specified interceptors. If the target is null, the proxy will not delegate calls
    /// to a backing instance.</remarks>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy.</param>
    /// <param name="serviceProvider">The service provider used for dependency resolution during proxy creation.</param>
    /// <param name="target">The target instance to proxy. Can be null if a proxy without a backing instance is desired.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. At least one interceptor must be provided.</param>
    /// <returns>A proxy instance of type TClass that wraps the specified target and applies the provided asynchronous
    /// interceptors.</returns>
    public static TClass CreateClassProxyWithTarget<TClass>(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, TClass? target, params IAsyncInterceptor[] asyncInterceptors) where TClass : class =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a class proxy for the specified target instance, applying the given asynchronous interceptors and proxy
    /// generation options.
    /// </summary>
    /// <remarks>The created proxy will delegate calls to the target instance and apply the specified
    /// interceptors. If the target is null, the proxy generator will instantiate a new object of type TClass using the
    /// service provider. This method is useful for scenarios where interception of asynchronous methods is
    /// required.</remarks>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be null.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies for the proxy. Cannot be null.</param>
    /// <param name="target">The target instance to proxy. If null, a new instance will be created by the proxy generator.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be null.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Can be empty.</param>
    /// <returns>A proxy instance of type TClass that wraps the specified target and applies the provided asynchronous
    /// interceptors.</returns>
    public static TClass CreateClassProxyWithTarget<TClass>(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, TClass? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) where TClass : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(serviceProvider);
      ArgumentNullException.ThrowIfNull(options);
      return (TClass)proxyGenerator.CreateClassProxyWithTarget(serviceProvider, typeof(TClass), Type.EmptyTypes, target, options, [], [.. asyncInterceptors.ToInterceptors()]);
    }

    /// <summary>
    /// Creates a class proxy with the specified target object and applies the given asynchronous interceptors. The
    /// proxy can implement additional interfaces as specified.
    /// </summary>
    /// <remarks>The created proxy delegates calls to the specified target object and applies the provided
    /// asynchronous interceptors. Use this method when you need to intercept asynchronous operations on an existing
    /// object instance. The proxy will implement any additional interfaces specified in the parameters.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be null.</param>
    /// <param name="serviceProvider">The service provider used for dependency resolution during proxy creation. Cannot be null.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types to be implemented by the proxy. Can be null or empty if no extra
    /// interfaces are required.</param>
    /// <param name="target">The target object to be proxied. Must be an instance of the specified class type.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be null or empty.</param>
    /// <returns>An instance of the proxied class implementing the specified interfaces and interceptors. The returned object is
    /// of the same type as the class being proxied.</returns>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type, using the provided target object and asynchronous
    /// interceptors.
    /// </summary>
    /// <remarks>The created proxy will forward calls to the target object unless an interceptor handles the
    /// invocation. The proxy can be used to add cross-cutting concerns such as logging, validation, or asynchronous
    /// behaviors to the target class.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies for the proxy and its interceptors.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target object to proxy. Methods called on the proxy will be forwarded to this object unless intercepted.</param>
    /// <param name="options">The options used to configure proxy generation, such as additional interfaces or hook behaviors.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the constructor of the proxied class. Can be null if no arguments are required.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. These interceptors can modify or observe method
    /// calls.</param>
    /// <returns>A proxy instance of the specified class type with the given target and interceptors applied.</returns>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, ProxyGenerationOptions options, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, options, constructorArguments, [.. asyncInterceptors.ToInterceptors()]);

    /// <summary>
    /// Creates a proxy instance for the specified class type, using the provided target object and applying the given
    /// asynchronous interceptors.
    /// </summary>
    /// <remarks>The created proxy will use the default proxy generation options. The proxy enables
    /// interception of asynchronous methods and delegates calls to the specified target object. Dependencies required
    /// by interceptors or the proxied class are resolved using the provided service provider.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be null.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies for the proxy and its interceptors. Cannot be null.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target object to proxy. Methods called on the proxy will be forwarded to this object.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the constructor of the proxied class. Can be null if no arguments are required.</param>
    /// <param name="asyncInterceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be null or empty.</param>
    /// <returns>An object instance that is a proxy of the specified class type, forwarding calls to the target and applying the
    /// provided interceptors.</returns>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, object?[]? constructorArguments, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, target, ProxyGenerationOptions.Default, constructorArguments, asyncInterceptors);

    /// <summary>
    /// Creates a class proxy for the specified type using the given target instance and asynchronous interceptors.
    /// </summary>
    /// <remarks>The proxy instance is created with default proxy generation options and no additional
    /// interfaces. Use this overload for simple proxy scenarios where only the class type and interceptors are
    /// required.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies for the proxy.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target instance to proxy. Can be null if the proxy does not require a target.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Each interceptor can modify or observe method
    /// invocations.</param>
    /// <returns>An object representing the created class proxy instance. The returned proxy implements the specified class type
    /// and applies the provided interceptors.</returns>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class, using the provided target object and asynchronous
    /// interceptors.
    /// </summary>
    /// <remarks>The created proxy will forward calls to the target object and apply the specified
    /// asynchronous interceptors. Use this method when you need to proxy an existing instance rather than creating a
    /// new one. The proxy can intercept both synchronous and asynchronous methods, depending on the interceptors
    /// provided.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies required by the proxy.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target object to wrap with the proxy. Can be null if the proxy does not require a target instance.</param>
    /// <param name="options">The proxy generation options that configure proxy behavior, such as additional interfaces or hook settings.</param>
    /// <param name="asyncInterceptors">An array of asynchronous interceptors to apply to the proxy. Each interceptor can modify or observe method
    /// invocations.</param>
    /// <returns>An object instance that is a proxy of the specified class, wrapping the provided target and applying the given
    /// interceptors.</returns>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, options, asyncInterceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type, using the provided target object and applying
    /// asynchronous interceptors. The proxy implements any additional interfaces specified and is configured with the
    /// given proxy generation options.
    /// </summary>
    /// <remarks>The created proxy delegates calls to the target object and applies asynchronous interception
    /// logic as specified. The proxy can be used to add cross-cutting concerns such as logging, validation, or
    /// authorization to the target class. Thread safety depends on the implementation of the interceptors and the
    /// target object.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be null.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies required during proxy creation. Cannot be null.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement. Can be null or empty if no extra
    /// interfaces are required.</param>
    /// <param name="target">The target object to which method calls will be delegated. Can be null if the proxy does not require a target.</param>
    /// <param name="options">The options used to configure proxy generation, such as hook and selector settings. Cannot be null.</param>
    /// <param name="asyncInterceptors">A set of asynchronous interceptors to apply to the proxy. At least one interceptor must be provided.</param>
    /// <returns>An object instance representing the proxy of the specified class type. The returned proxy implements the
    /// requested interfaces and applies the provided interceptors.</returns>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, ProxyGenerationOptions options, params IAsyncInterceptor[] asyncInterceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, additionalInterfacesToProxy, target, options, [], [.. asyncInterceptors.ToInterceptors()]);

  }
}