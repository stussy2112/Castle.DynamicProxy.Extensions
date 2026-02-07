// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.TryAddKeyedSingleton.cs" company="Karma, LLC">
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
    /// Attempts to add a keyed singleton service of the specified type to the dependency injection container with the
    /// provided interceptors, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service so that the single instance will be wrapped with the specified interceptors. Use this
    /// to enable cross-cutting concerns such as logging, validation, or authorization for keyed singleton services.
    /// The service type and implementation type are the same.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of the specified type to the dependency injection container with the
    /// provided interceptor types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with interceptor types that will be resolved from the dependency injection container
    /// when needed. Interceptors are applied in the order specified. Use this for cross-cutting concerns such as
    /// logging, validation, or authorization.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of the specified type and implementation to the dependency injection
    /// container with the provided interceptors, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with a separate implementation type, allowing the service to be resolved via an
    /// interface or base class while using a concrete implementation. Interceptors are applied to the implementation.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of the specified type and implementation to the dependency injection
    /// container with the provided interceptor types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with interceptor types that will be resolved from the dependency injection container.
    /// Use this when you need to separate the service interface from its implementation and apply interceptors.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of the specified type to the dependency injection container using a
    /// factory method and the provided interceptors, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance, allowing for custom initialization logic. The
    /// factory is invoked once when the service is first requested, and the resulting instance is wrapped with the
    /// specified interceptors.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      Func<IServiceProvider, object?, object> implementationFactory,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of the specified type to the dependency injection container using a
    /// factory method and the provided interceptor types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance, with interceptors resolved from the dependency
    /// injection container. The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      Func<IServiceProvider, object?, object> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> to the dependency injection
    /// container with the provided interceptors, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service so that the single instance will be wrapped with the specified interceptors. The
    /// service type and implementation type are the same. Use this to enable cross-cutting concerns such as logging,
    /// validation, or authorization.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> to the dependency injection
    /// container with the provided interceptor types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with interceptor types that will be resolved from the dependency injection container
    /// when needed. Interceptors are applied in the order specified.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> to the dependency injection
    /// container with the specified interceptor type, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with an interceptor type that will be resolved from the dependency injection
    /// container. Use this for cross-cutting concerns such as logging, validation, or authorization.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TInterceptor>(serviceKey, ServiceLifetime.Singleton);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> to the dependency injection
    /// container using a factory method and the provided interceptors, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance, allowing for custom initialization logic. The
    /// factory is invoked once when the service is first requested, and the resulting instance is wrapped with the
    /// specified interceptors.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> implementationFactory,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddKeyedIntercepted(serviceKey, implementationFactory, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> to the dependency injection
    /// container using a factory method and the provided interceptor types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance, with interceptors resolved from the dependency
    /// injection container. The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddKeyedIntercepted(serviceKey, implementationFactory, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> to the dependency injection
    /// container using a factory method and the specified interceptor type, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance and applies the specified interceptor type, which
    /// will be resolved from the dependency injection container. The factory is invoked once when the service is first
    /// requested.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> implementationFactory)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TInterceptor>(serviceKey, implementationFactory, ServiceLifetime.Singleton);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptors, if
    /// it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with a separate implementation type, allowing the service to be resolved via an
    /// interface or base class while using a concrete implementation. Interceptors are applied to the implementation.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService, TImplementation>(serviceKey, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptor
    /// types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with interceptor types that will be resolved from the dependency injection container.
    /// Use this when you need to separate the service interface from its implementation and apply interceptors.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService, TImplementation>(serviceKey, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the specified interceptor
    /// type, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// registers a keyed service with an interceptor type that will be resolved from the dependency injection
    /// container. Use this for cross-cutting concerns such as logging, validation, or authorization.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TImplementation, TInterceptor>(serviceKey, ServiceLifetime.Singleton);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container using a factory method and the
    /// provided interceptors, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance, allowing for custom initialization logic. The
    /// factory is invoked once when the service is first requested, and the resulting instance is wrapped with the
    /// specified interceptors.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the implementation. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> implementationFactory,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Singleton, interceptors);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container using a factory method and the
    /// provided interceptor types, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance, with interceptors resolved from the dependency
    /// injection container. The factory is invoked once when the service is first requested.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the implementation. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Singleton, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed singleton service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container using a factory method and the
    /// specified interceptor type, if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Singleton services are created once and shared throughout the application lifetime. If the service type with
    /// the specified key has already been registered, this method does not add a duplicate registration. This method
    /// uses a factory function to create the keyed service instance and applies the specified interceptor type, which
    /// will be resolved from the dependency injection container. The factory is invoked once when the service is first
    /// requested.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key used to identify this service registration.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the implementation. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed singleton service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedSingleton<TService, TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> implementationFactory)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TImplementation, TInterceptor>(serviceKey, implementationFactory, ServiceLifetime.Singleton);
  }
}
