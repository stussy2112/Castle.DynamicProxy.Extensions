// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.TryAdd.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
  public static partial class ServiceCollectionExtensions
  {
    /// <summary>
    /// Adds the specified service type to the service collection with interception support if it has not already been
    /// registered.
    /// </summary>
    /// <remarks>If the service type has already been registered, this method does not add a duplicate
    /// registration. Interception allows additional behavior to be injected when the service's methods are
    /// invoked.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service will be instantiated and reused.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Interceptors are invoked when the service's methods are
    /// called.</param>
    /// <returns>The original service collection with the intercepted service registered if it was not already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddIntercepted<TService, TService>(lifetime, interceptors);

    /// <summary>
    /// Adds the specified service type to the service collection with interception support if it has not already been
    /// registered.
    /// </summary>
    /// <remarks>Use this method to register a service with interception capabilities, allowing interceptors
    /// to be applied to service operations. If the service type is already registered, no changes are made.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long the service instance should exist.</param>
    /// <param name="interceptorTypes">An array of interceptor types to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The updated service collection with the intercepted service registered. Returns the original collection if the
    /// service was already registered.</returns>
    public static IServiceCollection TryAddIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddIntercepted<TService, TService>(lifetime, interceptorTypes);

    /// <summary>
    /// Adds the specified service type to the service collection with interception support, if it has not already been
    /// registered.
    /// </summary>
    /// <remarks>This method enables interception for the specified service type by associating it with the
    /// provided interceptor. If the service type is already registered, no changes are made. Use this method to add
    /// cross-cutting behavior, such as logging or validation, to services via interceptors.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must have a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long the service instance should exist.</param>
    /// <returns>The updated service collection with the intercepted service registered. Returns the original collection if the
    /// service was already registered.</returns>
    public static IServiceCollection TryAddIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted(typeof(TService), typeof(TService), lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a service of the specified type to the service collection with interception support, using the
    /// provided factory and interceptors.
    /// </summary>
    /// <remarks>If a service of the specified type is already registered, this method does not add a new
    /// registration. Interceptors allow additional behavior to be injected into the service's method calls, such as
    /// logging or validation.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="factory">A factory function that creates an instance of the service using the provided service provider.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Can be empty if no interception is required.</param>
    /// <returns>The original service collection with the intercepted service registration added if it was not already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="factory"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted<TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddIntercepted(typeof(TService), factory, lifetime, interceptors);

    /// <summary>
    /// Adds a service of type <typeparamref name="TService"/> to the service collection with interception support, if
    /// it has not already been registered.
    /// </summary>
    /// <remarks>If the service type is already registered, this method does not overwrite the existing
    /// registration. Interceptors are applied in the order specified in <paramref name="interceptorTypes"/>.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="factory">A factory function used to create instances of <typeparamref name="TService"/>. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be managed by the dependency
    /// injection container.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have a public constructor
    /// and will be used to intercept service calls.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted service registered. Returns the same instance
    /// for chaining.</returns>
    public static IServiceCollection TryAddIntercepted<TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddIntercepted(typeof(TService), factory, lifetime, interceptorTypes);

    /// <summary>
    /// Adds the specified service to the collection with interception support using the provided interceptor type, if
    /// it has not already been registered.
    /// </summary>
    /// <remarks>This method enables interception for the registered service by applying the specified
    /// interceptor. If the service type is already registered, no changes are made. Use this method to add
    /// cross-cutting concerns such as logging or validation to services via interceptors.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have public constructors.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="factory">A factory function used to create instances of the service. The function receives the current <see
    /// cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long the service instance should live.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted service added, or unchanged if the service
    /// was already registered.</returns>
    public static IServiceCollection TryAddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted(typeof(TService), factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a service of the specified type with interception to the dependency injection container, if it
    /// has not already been registered.
    /// </summary>
    /// <remarks>If a service of the specified type is already registered, this method does not add a new
    /// registration. Interceptors allow additional behavior to be injected into service method calls, such as logging
    /// or validation.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must be a class with public constructors and implement or
    /// derive from TService.</typeparam>
    /// <param name="services">The IServiceCollection to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Can be empty if no interception is required.</param>
    /// <returns>The IServiceCollection instance with the intercepted service registration added if it was not already present;
    /// otherwise, the original IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddIntercepted(typeof(TService), typeof(TImplementation), lifetime, interceptors);

    /// <summary>
    /// Adds the specified service type and implementation to the service collection with interception support, if it
    /// has not already been registered.
    /// </summary>
    /// <remarks>Interceptors are applied to the registered service implementation, enabling cross-cutting
    /// concerns such as logging or validation. The method does not overwrite existing registrations for the specified
    /// service type.</remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement or inherit from
    /// TService.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and reused.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The updated IServiceCollection instance with the intercepted service registration added, or unchanged if the
    /// service was already registered.</returns>
    public static IServiceCollection TryAddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddIntercepted(typeof(TService), typeof(TImplementation), lifetime, interceptorTypes);

    /// <summary>
    /// Adds the specified service type, implementation type, and interceptor to the service collection if they have not
    /// already been registered.
    /// </summary>
    /// <remarks>This method ensures that the service and its interceptor are only added if they are not
    /// already present in the collection. The interceptor will be applied to instances of the specified implementation
    /// type when resolving the service.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type for the service. Must be a class and implement <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the service and interceptor will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long the service instance should live.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> containing the registered service and interceptor.</returns>
    public static IServiceCollection TryAddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted(typeof(TService), typeof(TImplementation), lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a service of the specified type with interception support to the service collection, using a
    /// factory for implementation creation.
    /// </summary>
    /// <remarks>If a service of the specified type is already registered, this method does not add a new
    /// registration. Interceptors allow additional behavior to be injected into service method calls, such as logging
    /// or validation.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must be a reference type and implement TService.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="factory">A factory function used to create instances of the implementation type. The function receives the current
    /// IServiceProvider.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Can be empty if no interception is required.</param>
    /// <returns>The IServiceCollection instance with the intercepted service registration added, or unchanged if the service
    /// type was already registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="factory"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddIntercepted<TService>(factory, lifetime, interceptors);

    /// <summary>
    /// Adds the specified service to the collection with interception support, using a factory for implementation
    /// creation and applying the given interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service with custom interception logic, such as logging or
    /// validation, by specifying interceptor types. The interceptors are applied in the order provided. If the service
    /// is already registered, this method does not overwrite the existing registration.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to instantiate. Must be a class and implement <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="factory">A factory function used to create instances of <typeparamref name="TImplementation"/>. Receives the current <see
    /// cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long instances of the service are retained.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The original <see cref="IServiceCollection"/> instance with the intercepted service registration added.</returns>
    public static IServiceCollection TryAddIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddIntercepted<TService>(factory, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a service of the specified type to the collection if it is not already present, applying the given
    /// interceptor to the implementation. This method enables interception of service calls for cross-cutting concerns
    /// such as logging or validation.
    /// </summary>
    /// <remarks>Use this method to enable interception for services registered via a factory. The interceptor
    /// allows for additional behavior to be injected into service calls, such as logging, authorization, or exception
    /// handling. If the service type is already registered, no changes are made.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type for the service. Must inherit from <typeparamref name="TService"/> and be a
    /// class.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the service and interceptor will be added.</param>
    /// <param name="factory">A factory function used to create instances of <typeparamref name="TImplementation"/>. Receives the current <see
    /// cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be managed within the dependency
    /// injection container.</param>
    /// <returns>The original <see cref="IServiceCollection"/> instance with the service and interceptor registration added, or
    /// unchanged if the service was already registered.</returns>
    public static IServiceCollection TryAddIntercepted<TService, TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted<TService>(factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a service of the specified type to the service collection with the given lifetime and applies
    /// the provided interceptors. If the service type is already registered, no action is taken.
    /// </summary>
    /// <remarks>This method only adds the service if it has not already been registered. Interceptors allow
    /// additional behavior to be injected when the service is resolved, such as logging or validation.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service will be instantiated and reused.</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors will be invoked when the service is
    /// resolved.</param>
    /// <returns>The original service collection with the intercepted service registered if it was not already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="serviceType"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors) =>
      services.TryAddIntercepted(serviceType, serviceType, lifetime, interceptors);

    /// <summary>
    /// Adds the specified service type to the service collection with interception support if it has not already been
    /// registered.
    /// </summary>
    /// <remarks>If the service type is already registered, this method does not add a new registration.
    /// Interceptors are applied in the order specified in the array.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long the service instance should exist.</param>
    /// <param name="interceptorTypes">An array of interceptor types to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The updated service collection containing the intercepted service registration.</returns>
    public static IServiceCollection TryAddIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddIntercepted(serviceType, serviceType, lifetime, interceptorTypes);

    /// <summary>
    /// Attempts to add a service of the specified type to the service collection, optionally wrapping the
    /// implementation with one or more interceptors if provided.
    /// </summary>
    /// <remarks>If one or more interceptors are provided, the service implementation will be wrapped in a
    /// dynamic proxy that applies the specified interceptors. If no interceptors are specified, the service is
    /// registered as usual without interception. This method does not replace existing registrations for the specified
    /// service type.</remarks>
    /// <param name="services">The service collection to which the service will be added. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be null.</param>
    /// <param name="factory">A factory function that creates the service implementation instance. Cannot be null.</param>
    /// <param name="lifetime">The lifetime with which to register the service (e.g., Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. If null or empty, the service is registered without
    /// interception.</param>
    /// <returns>The same IServiceCollection instance, to allow for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="factory"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedServiceDescriptor(services, serviceType, factory, lifetime, interceptors);
      services.TryAdd(interceptedDescriptor);
      return services;
    }

    /// <summary>
    /// Adds the specified service to the collection if it is not already present, configuring it to be intercepted by
    /// the provided interceptor types.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in the <paramref name="interceptorTypes"/>
    /// array. This method does not replace existing registrations for the specified service type.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register for interception.</param>
    /// <param name="factory">A factory function that creates an instance of the service when it is requested.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how long the service instance should be retained.</param>
    /// <param name="interceptorTypes">One or more types of interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The service collection with the intercepted service added. Returns the original collection if the service was
    /// already present.</returns>
    public static IServiceCollection TryAddIntercepted(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedServiceDescriptor(services, serviceType, factory, lifetime, interceptorTypes);
      services.TryAdd(interceptedDescriptor);
      return services;
    }

    /// <summary>
    /// Attempts to add a service of the specified type with interception support to the service collection, if it has
    /// not already been registered.
    /// </summary>
    /// <remarks>If a service of the specified type is already registered, this method does not add a new
    /// registration. Interceptors allow additional behavior (such as logging or validation) to be injected into service
    /// method calls.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class that consumers will depend on.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors will be invoked for each method call on the
    /// service.</param>
    /// <returns>The original service collection with the intercepted service registered if it was not already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="implementationType"/> is null.</exception>
    public static IServiceCollection TryAddIntercepted(
      this IServiceCollection services,
      Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(implementationType);

      Func<IServiceProvider, object> implementationFactory = (sp) => ActivatorUtilities.CreateInstance(sp, implementationType);
      return services.TryAddIntercepted(serviceType, implementationFactory, lifetime, interceptors);
    }

    /// <summary>
    /// Adds a service of the specified type to the service collection with interception support, if it has not already
    /// been registered.
    /// </summary>
    /// <remarks>If the service type is already registered, this method does not overwrite the existing
    /// registration. Interceptors are applied in the order specified in the array.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be null.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have public constructors. Cannot be null.</param>
    /// <param name="lifetime">The lifetime with which to register the service, such as Singleton, Scoped, or Transient.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have public constructors.</param>
    /// <returns>The original service collection with the intercepted service registered. Returns the same instance for chaining.</returns>
    public static IServiceCollection TryAddIntercepted(
      this IServiceCollection services,
      Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(implementationType);

      Func<IServiceProvider, object> implementationFactory = (sp) => ActivatorUtilities.CreateInstance(sp, implementationType);
      return services.TryAddIntercepted(serviceType, implementationFactory, lifetime, interceptorTypes);
    }

    /// <summary>
    /// Attempts to add a keyed, intercepted service of type TService to the service collection with the specified
    /// lifetime and interceptors.
    /// </summary>
    /// <remarks>If a service of type TService with the specified key is already registered, this method does
    /// not add a new registration. Interceptors allow additional behavior to be injected when the service is resolved,
    /// such as logging or validation.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service is instantiated and reused.</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors will be invoked when the service is
    /// resolved.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained. If a service with the specified
    /// key and type already exists, no registration is added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddKeyedIntercepted<TService, TService>(serviceKey, lifetime, interceptors);

    /// <summary>
    /// Attempts to register a keyed intercepted service of type TService with the specified lifetime and interceptors
    /// if it has not already been registered.
    /// </summary>
    /// <remarks>If a service with the specified key and type is already registered, this method does not add
    /// a duplicate registration. Interceptors are applied in the order specified in the interceptorTypes
    /// array.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceKey">The key used to identify the service registration. Can be null if no key is required.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered. Specifies how the service will be instantiated and
    /// managed.</param>
    /// <param name="interceptorTypes">An array of interceptor types to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The IServiceCollection instance with the intercepted service registration added if it was not already present.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddKeyedIntercepted<TService, TService>(serviceKey, lifetime, interceptorTypes);

    /// <summary>
    /// Attempts to register a keyed service of type TService with an interceptor of type TInterceptor in the specified
    /// service collection, using the given service lifetime. If a registration for the keyed service already exists, no
    /// action is taken.
    /// </summary>
    /// <remarks>This method is useful for scenarios where multiple implementations of a service are
    /// registered under different keys and require interception. If a service with the specified key is already
    /// registered, the method does not overwrite the existing registration.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must implement IInterceptor and have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceKey">The key used to identify the service registration. Can be null to indicate a default or unkeyed registration.</param>
    /// <param name="lifetime">The lifetime with which the service and interceptor are registered. Specifies how long the service instance
    /// should live.</param>
    /// <returns>The IServiceCollection instance with the keyed intercepted service registration added, or unchanged if the
    /// registration already exists.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService, TService>(serviceKey, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a keyed, intercepted service of the specified type to the service collection with the given
    /// lifetime and interceptors.
    /// </summary>
    /// <remarks>If a service with the specified type and key already exists in the collection, this method
    /// does not add a duplicate registration. Interceptors are applied in the order provided.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a class with public constructors and implement
    /// TService.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. At least one interceptor must be provided.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted(typeof(TService), serviceKey, typeof(TImplementation), lifetime, interceptors);

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection with interception support, if it has not
    /// already been registered.
    /// </summary>
    /// <remarks>This method enables interception for the registered service, allowing additional behavior to
    /// be injected via the specified interceptors. If a service with the same key and type already exists, no action is
    /// taken.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a class and implement <typeparamref
    /// name="TService"/>.</typeparam>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceKey">The key used to identify the service registration. Can be null to indicate a default registration.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered, such as singleton, scoped, or transient.</param>
    /// <param name="interceptorTypes">The types of interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance with the keyed intercepted service registration added, or
    /// unchanged if the service was already registered.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted(typeof(TService), serviceKey, typeof(TImplementation), lifetime, interceptorTypes);

    /// <summary>
    /// Attempts to register a keyed service with interception support for the specified service, implementation, and
    /// interceptor types using the given lifetime.
    /// </summary>
    /// <remarks>If a service with the specified key and types is already registered, this method does not add
    /// a duplicate registration. Interception allows additional behavior to be applied to the service implementation,
    /// such as logging or validation.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The type that implements the service. Must be a class and implement <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and be a class.</typeparam>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceKey">The key used to identify the service registration. Can be <see langword="null"/> for unkeyed registration.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered. Specifies how the service will be managed by the
    /// dependency injection container.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the keyed intercepted service registration added if it was
    /// not already present.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted(typeof(TService), serviceKey, typeof(TImplementation), lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a keyed, intercepted service of the specified type to the service collection if it has not
    /// already been registered.
    /// </summary>
    /// <remarks>If a service of the specified type and key is already registered, this method does not add a
    /// new registration. Interceptors allow additional behavior to be injected into the service's method calls, such as
    /// logging or validation.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="factory">A factory function used to create instances of the service. Receives the service provider and the service key as
    /// parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors will be invoked for each service instance
    /// created.</param>
    /// <returns>The original service collection, with the service registered if it was not already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="factory"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted<TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddKeyedIntercepted(typeof(TService), serviceKey, factory, lifetime, interceptors);

    /// <summary>
    /// Attempts to register a keyed intercepted service of type TService in the service collection using the specified
    /// factory, lifetime, and interceptors. If a matching service is already registered, no action is taken.
    /// </summary>
    /// <remarks>This method enables interception of service instances by applying the specified interceptors.
    /// Interceptors are resolved from the service provider and must be registered beforehand. If a service with the
    /// same key and type is already registered, the method does not overwrite the existing registration.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceKey">The key used to identify the service registration. Can be null if no key is required.</param>
    /// <param name="factory">A factory function that creates an instance of TService. Receives the service provider and the service key as
    /// parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be managed within the dependency
    /// injection container.</param>
    /// <param name="interceptorTypes">An array of interceptor types to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The service collection with the intercepted service registration added, or unchanged if a matching registration
    /// already exists.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddKeyedIntercepted(typeof(TService), serviceKey, factory, lifetime, interceptorTypes);

    /// <summary>
    /// Attempts to register a keyed service of type <typeparamref name="TService"/> with interception using the
    /// specified interceptor type. The service is added to the collection only if it has not already been registered
    /// for the given key.
    /// </summary>
    /// <remarks>This method enables interception for keyed service registrations, allowing cross-cutting
    /// concerns such as logging or validation to be applied. If a service with the specified key is already registered,
    /// no action is taken.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceKey">The key associated with the service registration. Can be <see langword="null"/> to indicate an unkeyed
    /// registration.</param>
    /// <param name="factory">A factory function used to create instances of <typeparamref name="TService"/>. Receives the service provider
    /// and the service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered, such as singleton, scoped, or transient.</param>
    /// <returns>The original <see cref="IServiceCollection"/> instance with the keyed intercepted service registration added if
    /// it was not already present.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted(typeof(TService), serviceKey, factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a keyed, intercepted service of the specified type to the service collection using a factory for
    /// the implementation type.
    /// </summary>
    /// <remarks>If a service with the specified key and type already exists in the collection, this method
    /// does not add a duplicate registration. Interceptors are applied in the order provided.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must be a class that implements or derives from TService.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="factory">A factory function that creates an instance of the implementation type. Receives the service provider and the
    /// service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Interceptors can modify or extend the behavior of the service.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="factory"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, factory, lifetime, interceptors);

    /// <summary>
    /// Attempts to add a keyed, intercepted registration for the specified service and implementation types to the
    /// service collection using the provided factory and lifetime.
    /// </summary>
    /// <remarks>This method enables interception for keyed service registrations, allowing additional
    /// behavior to be injected via interceptors. If a registration with the specified key and service type already
    /// exists, the method does not overwrite it.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to register. Must be a class and derive from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The service collection to which the registration will be added.</param>
    /// <param name="serviceKey">The key used to identify the service registration. Can be null to indicate an unkeyed registration.</param>
    /// <param name="factory">A factory function that creates instances of <typeparamref name="TImplementation"/>. Receives the service
    /// provider and the service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered. Specifies how long the service instance should live.</param>
    /// <param name="interceptorTypes">An array of interceptor types to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The service collection with the attempted registration added. If a matching registration already exists, no
    /// changes are made.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, factory, lifetime, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed intercepted registration for the specified service type, implementation type, and
    /// interceptor to the service collection. If a registration with the given key already exists, no action is taken.
    /// </summary>
    /// <remarks>This method enables interception for services registered with a specific key, allowing
    /// cross-cutting concerns such as logging or validation to be applied. If a service with the specified key is
    /// already registered, the method does not overwrite the existing registration.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must inherit from <typeparamref name="TService"/> and be a
    /// class.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have public constructors.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service registration will be added.</param>
    /// <param name="serviceKey">The key used to uniquely identify the service registration. Can be <see langword="null"/> if no key is required.</param>
    /// <param name="factory">A factory function that creates instances of <typeparamref name="TImplementation"/>. Receives the service
    /// provider and the service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be managed by the dependency
    /// injection container.</param>
    /// <returns>The original <see cref="IServiceCollection"/> instance, allowing for method chaining.</returns>
    public static IServiceCollection TryAddKeyedIntercepted<TService, TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddKeyedIntercepted<TService>(serviceKey, factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Attempts to add a keyed service of the specified type to the service collection with interception support, if it
    /// has not already been registered.
    /// </summary>
    /// <remarks>If a service with the specified type and key is already registered, this method does not add
    /// a duplicate registration. Interceptors allow additional behavior to be injected into the service's method
    /// calls.</remarks>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. At least one interceptor must be provided.</param>
    /// <returns>The original service collection with the keyed intercepted service registered if it was not already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="serviceType"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, serviceType, lifetime, interceptors);

    /// <summary>
    /// Attempts to register a keyed service with the specified interceptors in the dependency injection container if it
    /// has not already been registered.
    /// </summary>
    /// <remarks>If a service with the specified key and type is already registered, this method does not add
    /// a duplicate registration. Interceptors are applied in the order specified in the array.</remarks>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key used to identify the service instance. Can be null if no key is required.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered. Specifies how long the service instance should exist.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The original service collection with the keyed intercepted service registration added if it was not already
    /// present.</returns>
    public static IServiceCollection TryAddKeyedIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddKeyedIntercepted(serviceType, serviceKey, serviceType, lifetime, interceptorTypes);

    /// <summary>
    /// Attempts to add a keyed service of the specified type to the service collection with optional interception. If
    /// interceptors are provided, the service is registered as a proxy that applies the specified interceptors;
    /// otherwise, the service is registered directly.
    /// </summary>
    /// <remarks>If a service with the specified type and key already exists in the collection, this method
    /// does not add a duplicate registration. When interceptors are provided, the service is registered as a proxy that
    /// applies the interceptors in the order specified. This method is typically used to enable aspect-oriented
    /// programming scenarios, such as logging or validation, for keyed services.</remarks>
    /// <param name="services">The service collection to which the service will be added. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be null.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="factory">A factory function that creates the service instance. The function receives the service provider and the service
    /// key as parameters. Cannot be null.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. If null or empty, the service is registered without
    /// interception.</param>
    /// <returns>The same IServiceCollection instance, to allow for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="factory"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      Func<IServiceProvider, object?, object> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedKeyedServiceDescriptor(services, serviceType, serviceKey, factory, lifetime, interceptors);
      services.TryAdd(interceptedDescriptor);

      return services;
    }

    /// <summary>
    /// Attempts to add a keyed, intercepted service to the service collection if it has not already been registered.
    /// </summary>
    /// <remarks>If a service with the specified type and key is already registered, this method does not add
    /// a new registration. Interceptors are applied in the order specified in the interceptorTypes array.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key used to identify the service instance. Can be null if no key is required.</param>
    /// <param name="factory">A factory function that creates the service instance. Receives the service provider and the service key as
    /// parameters.</param>
    /// <param name="lifetime">The lifetime with which the service should be registered. Specifies how long the service instance should live.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The service collection with the intercepted service registration added if it was not already present.</returns>
    public static IServiceCollection TryAddKeyedIntercepted(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      Func<IServiceProvider, object?, object> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedKeyedServiceDescriptor(services, serviceType, serviceKey, factory, lifetime, interceptorTypes);
      services.TryAdd(interceptedDescriptor);

      return services;
    }

    /// <summary>
    /// Attempts to add a keyed, intercepted service of the specified type and implementation to the service collection
    /// if it has not already been registered.
    /// </summary>
    /// <remarks>If a service with the specified type and key is already registered, this method does not add
    /// a new registration. Interceptors allow additional behavior to be injected when the service is
    /// resolved.</remarks>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key that uniquely identifies the keyed service registration. Pass null to use null as the key.
    /// Note: Keyed services with null keys are still resolved as keyed services, not as non-keyed services.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Interceptors are invoked when the service is resolved.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="implementationType"/> is null.</exception>
    public static IServiceCollection TryAddKeyedIntercepted(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(implementationType);

      Func<IServiceProvider, object?, object> implementationFactory = (sp, _) => ActivatorUtilities.CreateInstance(sp, implementationType);
      return services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationFactory, lifetime, interceptors);
    }

    /// <summary>
    /// Attempts to register a keyed service with the specified implementation type and interceptors in the service
    /// collection if it has not already been registered.
    /// </summary>
    /// <remarks>If a service with the specified key and type is already registered, this method does not
    /// overwrite the existing registration. Interceptors are applied in the order specified in the interceptorTypes
    /// array.</remarks>
    /// <param name="services">The service collection to which the keyed intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This determines the contract for the resolved service.</param>
    /// <param name="serviceKey">The key used to identify the service instance. Can be null if no key is required.</param>
    /// <param name="implementationType">The concrete type that will be instantiated for the service. Must have public constructors.</param>
    /// <param name="lifetime">The lifetime with which the service will be registered, such as Singleton, Scoped, or Transient.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have public constructors.</param>
    /// <returns>The original service collection with the keyed intercepted service registration added if it was not already
    /// present.</returns>
    public static IServiceCollection TryAddKeyedIntercepted(
      this IServiceCollection services,
      Type serviceType,
      object? serviceKey,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(implementationType);

      Func<IServiceProvider, object?, object> implementationFactory = (sp, _) => ActivatorUtilities.CreateInstance(sp, implementationType);
      return services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationFactory, lifetime, interceptorTypes);
    }
  }
}