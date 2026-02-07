// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.AddSingleton.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Castle.DynamicProxy;

namespace Microsoft.Extensions.DependencyInjection
{
  public static partial class ServiceCollectionExtensions
  {
    /// <summary>
    /// Adds a singleton service of the specified type to the dependency injection container with the provided
    /// interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service so that the single instance will be wrapped with the specified interceptors. Use this to enable
    /// cross-cutting concerns such as logging, validation, or authorization for singleton services. The service type
    /// and implementation type are the same.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      params IInterceptor[] interceptors) =>
      services.AddIntercepted(serviceType, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of the specified type to the dependency injection container with the provided
    /// interceptor types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with interceptor types that will be resolved from the dependency injection container when needed.
    /// Interceptors are applied in the order specified. Use this for cross-cutting concerns such as logging,
    /// validation, or authorization.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.AddIntercepted(serviceType, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of the specified type to the dependency injection container using an existing instance
    /// and the provided interceptors.
    /// </summary>
    /// <remarks>
    /// This method registers an existing instance as a singleton service with the specified interceptors. The instance
    /// will be wrapped in a proxy that applies the interceptors to method calls. This is useful when you have a
    /// pre-configured instance that requires interception for cross-cutting concerns such as logging or auditing.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationInstance">An existing instance of the service to be registered as a singleton.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      object implementationInstance,
      params IInterceptor[] interceptors)
    {
      Func<IServiceProvider, object> implementationFactory = (_) => implementationInstance;
      return services.AddIntercepted(serviceType, implementationFactory, ServiceLifetime.Singleton, interceptors);
    }

    /// <summary>
    /// Adds a singleton service of the specified type to the dependency injection container using an existing instance
    /// and the provided interceptor types.
    /// </summary>
    /// <remarks>
    /// This method registers an existing instance as a singleton service with interceptor types that will be resolved
    /// from the dependency injection container. The instance will be wrapped in a proxy that applies the interceptors
    /// to method calls. This is useful when you have a pre-configured instance that requires interception for
    /// cross-cutting concerns.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationInstance">An existing instance of the service to be registered as a singleton.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      object implementationInstance,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      Func<IServiceProvider, object> implementationFactory = (_) => implementationInstance;
      return services.AddIntercepted(serviceType, implementationFactory, ServiceLifetime.Singleton, interceptorTypes);
    }

    /// <summary>
    /// Adds a singleton service of the specified type and implementation to the dependency injection container with
    /// the provided interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with a separate implementation type, allowing the service to be resolved via an interface or base class
    /// while using a concrete implementation. Interceptors are applied to the implementation.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      params IInterceptor[] interceptors) =>
      services.AddIntercepted(serviceType, implementationType, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of the specified type and implementation to the dependency injection container with
    /// the provided interceptor types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with interceptor types that will be resolved from the dependency injection container. Use this when you
    /// need to separate the service interface from its implementation and apply interceptors.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.AddIntercepted(serviceType, implementationType, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of the specified type to the dependency injection container using a factory method and
    /// the provided interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance, allowing for custom initialization logic. The factory is invoked once
    /// when the service is first requested, and the resulting instance is wrapped with the specified interceptors.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> implementationFactory,
      params IInterceptor[] interceptors) =>
      services.AddIntercepted(serviceType, implementationFactory, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of the specified type to the dependency injection container using a factory method and
    /// the provided interceptor types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance, with interceptors resolved from the dependency injection container.
    /// The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.AddIntercepted(serviceType, implementationFactory, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container with the
    /// provided interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service so that the single instance will be wrapped with the specified interceptors. The service type and
    /// implementation type are the same. Use this to enable cross-cutting concerns such as logging, validation, or
    /// authorization.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddIntercepted<TService>(ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container with the
    /// provided interceptor types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with interceptor types that will be resolved from the dependency injection container when needed.
    /// Interceptors are applied in the order specified.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddIntercepted<TService>(ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container with the
    /// specified interceptor type.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with an interceptor type that will be resolved from the dependency injection container. Use this for
    /// cross-cutting concerns such as logging, validation, or authorization.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.AddIntercepted<TService, TInterceptor>(ServiceLifetime.Singleton);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container using a
    /// factory method and the provided interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance, allowing for custom initialization logic. The factory is invoked once
    /// when the service is first requested, and the resulting instance is wrapped with the specified interceptors.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> implementationFactory,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddIntercepted(implementationFactory, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container using a
    /// factory method and the provided interceptor types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance, with interceptors resolved from the dependency injection container.
    /// The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddIntercepted(implementationFactory, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container using a
    /// factory method and the specified interceptor type.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance and applies the specified interceptor type, which will be resolved from
    /// the dependency injection container. The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> implementationFactory)
      where TService : class
      where TInterceptor : IInterceptor => services.AddIntercepted<TService, TInterceptor>(implementationFactory, ServiceLifetime.Singleton);

    /// <summary>
    /// Adds a singleton service of the specified type to the service collection, applying the given interceptors to the
    /// implementation instance.
    /// </summary>
    /// <remarks>Interceptors allow additional behavior, such as logging or validation, to be injected into
    /// the service's method calls. The service is registered as a singleton, so the same intercepted instance will be
    /// returned for all requests.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The service collection to which the singleton service will be added.</param>
    /// <param name="implementationInstance">The instance of the service to register as a singleton. This instance will be intercepted by the specified
    /// interceptors.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service instance. Can be empty if no interception is required.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService>(
      this IServiceCollection services,
      TService implementationInstance,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddInterceptedSingleton(typeof(TService), implementationInstance, interceptors);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container using an
    /// existing instance and the provided interceptor types.
    /// </summary>
    /// <remarks>
    /// This method registers an existing instance as a singleton service with interceptor types that will be resolved
    /// from the dependency injection container. The instance will be wrapped in a proxy that applies the interceptors
    /// to method calls. This is useful when you have a pre-configured instance that requires interception for
    /// cross-cutting concerns.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationInstance">An existing instance of the service to be registered as a singleton.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService>(
      this IServiceCollection services,
      TService implementationInstance,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddInterceptedSingleton(typeof(TService), implementationInstance, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> to the dependency injection container using an
    /// existing instance and the specified interceptor type.
    /// </summary>
    /// <remarks>
    /// This method registers an existing instance as a singleton service with an interceptor type that will be
    /// resolved from the dependency injection container. The instance will be wrapped in a proxy that applies the
    /// interceptor to method calls. This is useful when you have a pre-configured instance that requires interception
    /// for cross-cutting concerns.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationInstance">An existing instance of the service to be registered as a singleton.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      TService implementationInstance)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.AddInterceptedSingleton(typeof(TService), implementationInstance, typeof(TInterceptor));

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with a separate implementation type, allowing the service to be resolved via an interface or base class
    /// while using a concrete implementation. Interceptors are applied to the implementation.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted<TService, TImplementation>(ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptor
    /// types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with interceptor types that will be resolved from the dependency injection container. Use this when you
    /// need to separate the service interface from its implementation and apply interceptors.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted<TService, TImplementation>(ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the specified interceptor
    /// type.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method registers the
    /// service with an interceptor type that will be resolved from the dependency injection container. Use this for
    /// cross-cutting concerns such as logging, validation, or authorization.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.AddIntercepted<TService, TImplementation, TInterceptor>(ServiceLifetime.Singleton);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container using a factory method and the
    /// provided interceptors.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance, allowing for custom initialization logic. The factory is invoked once
    /// when the service is first requested, and the resulting instance is wrapped with the specified interceptors.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the implementation. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, TImplementation>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> implementationFactory,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted<TService, TImplementation>(implementationFactory, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container using a factory method and the
    /// provided interceptor types.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance, with interceptors resolved from the dependency injection container.
    /// The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the implementation. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, TImplementation>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted<TService, TImplementation>(implementationFactory, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Adds a singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container using a factory method and the
    /// specified interceptor type.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. This method uses a factory
    /// function to create the service instance and applies the specified interceptor type, which will be resolved from
    /// the dependency injection container. The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the implementation. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted singleton service registration added.</returns>
    public static IServiceCollection AddInterceptedSingleton<TService, TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> implementationFactory)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.AddIntercepted<TService, TImplementation, TInterceptor>(implementationFactory, ServiceLifetime.Singleton);
  }
}
