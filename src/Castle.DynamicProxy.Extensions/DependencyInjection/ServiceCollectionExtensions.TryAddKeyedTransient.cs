// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.TryAddKeyedTransient.cs" company="Karma, LLC">
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
    /// Attempts to add a keyed transient service of the specified type to the dependency injection container with the
    /// provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. If the service type with the specified key has
    /// already been registered, this method does not add a duplicate registration. Interception allows additional
    /// behavior to be injected when the service's methods are invoked.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, ServiceLifetime.Transient, interceptors);

    /// <summary>
    /// Attempts to add a keyed transient service of the specified type to the dependency injection container with the
    /// provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container when needed. Interceptors are applied in
    /// the order specified. If the service type with the specified key is already registered, no changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, ServiceLifetime.Transient, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed transient service of the specified type and implementation to the dependency injection
    /// container with the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with a separate
    /// implementation type, allowing the service to be resolved via an interface or base class while using a concrete
    /// implementation. Interceptors are applied to the implementation. If the service type with the specified key is
    /// already registered, no changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, ServiceLifetime.Transient, interceptors);

    /// <summary>
    /// Attempts to add a keyed transient service of the specified type and implementation to the dependency injection
    /// container with the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container. Use this when you need to separate the
    /// service interface from its implementation and apply interceptors. If the service type with the specified key is
    /// already registered, no changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, ServiceLifetime.Transient, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed transient service of the specified type to the dependency injection container using a
    /// factory method and the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method uses a factory function to create the
    /// service instance, allowing for custom initialization logic. The factory is invoked each time the service is
    /// requested, and the resulting instance is wrapped with the specified interceptors. If the service type with the
    /// specified key is already registered, no changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      Func<IServiceProvider, object?, object> implementationFactory,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationFactory, ServiceLifetime.Transient, interceptors);

    /// <summary>
    /// Attempts to add a keyed transient service of the specified type to the dependency injection container using a
    /// factory method and the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method uses a factory function to create the
    /// service instance, with interceptors resolved from the dependency injection container. The factory is invoked
    /// each time the service is requested. If the service type with the specified key is already registered, no
    /// changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      Func<IServiceProvider, object?, object> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationFactory, ServiceLifetime.Transient, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> to the dependency injection
    /// container using a factory method and the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method uses a factory function to create the
    /// service instance, allowing for custom initialization logic. The factory is invoked each time the service is
    /// requested, and the resulting instance is wrapped with the specified interceptors. If the service type with the
    /// specified key is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> implementationFactory,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddKeyedIntercepted(serviceKey, implementationFactory, ServiceLifetime.Transient, interceptors);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> to the dependency injection
    /// container using a factory method and the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method uses a factory function to create the
    /// service instance, with interceptors resolved from the dependency injection container. The factory is invoked
    /// each time the service is requested. If the service type with the specified key is already registered, no
    /// changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddKeyedIntercepted(serviceKey, implementationFactory, ServiceLifetime.Transient, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> to the dependency injection
    /// container using a factory method and the specified interceptor type if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method uses a factory function to create the
    /// service instance and applies the specified interceptor type, which will be resolved from the dependency
    /// injection container. If the service type with the specified key is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/> and the service key.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> implementationFactory)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TInterceptor>(serviceKey, implementationFactory, ServiceLifetime.Transient);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> to the dependency injection
    /// container with the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service so that each
    /// resolved instance will be wrapped with the specified interceptors. The service type and implementation type are
    /// the same. Use this to enable cross-cutting concerns such as logging, validation, or authorization. If the
    /// service type with the specified key is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, ServiceLifetime.Transient, interceptors);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> to the dependency injection
    /// container with the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container when needed. Interceptors are applied in
    /// the order specified. If the service type with the specified key is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, ServiceLifetime.Transient, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> to the dependency injection
    /// container with the specified interceptor type if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with an
    /// interceptor type that will be resolved from the dependency injection container. Use this for cross-cutting
    /// concerns such as logging, validation, or authorization. If the service type with the specified key is already
    /// registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TInterceptor>(serviceKey, ServiceLifetime.Transient);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptors if
    /// it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with a separate
    /// implementation type, allowing the service to be resolved via an interface or base class while using a concrete
    /// implementation. Interceptors are applied to the implementation. If the service type with the specified key is
    /// already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService, TImplementation>(serviceKey, ServiceLifetime.Transient, interceptors);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptor
    /// types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container. Use this when you need to separate the
    /// service interface from its implementation and apply interceptors. If the service type with the specified key is
    /// already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService, TImplementation>(serviceKey, ServiceLifetime.Transient, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed transient service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the specified interceptor
    /// type if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Transient services are created each time they are requested. This method registers the service with an
    /// interceptor type that will be resolved from the dependency injection container. Use this for cross-cutting
    /// concerns such as logging, validation, or authorization. If the service type with the specified key is already
    /// registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted keyed transient service registration added if it
    /// was not already present.</returns>
    public static IServiceCollection TryAddKeyedInterceptedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TImplementation, TInterceptor>(serviceKey, ServiceLifetime.Transient);
  }
}
