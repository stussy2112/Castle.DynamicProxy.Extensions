// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.DynamicProxy.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
  /// <summary>
  /// Provides extension methods for registering services with dynamic interception support in an IServiceCollection.
  /// </summary>
  /// <remarks>These extension methods enable the registration of services with interceptors, allowing
  /// cross-cutting concerns such as logging, validation, or authorization to be applied transparently. Interceptors are
  /// applied in the order provided. Methods support both keyed and non-keyed service registrations, as well as
  /// factory-based and type-based implementations.</remarks>
  public static partial class ServiceCollectionExtensions
  {
    private const string PROXY_GENERATOR_KEY = "Castle.DynamicProxy.Extensions.ProxyGenerator";

    /// <summary>
    /// Adds a service of type <typeparamref name="TService"/> to the dependency injection container with the specified
    /// lifetime and applies the provided interceptors to its implementation.
    /// </summary>
    /// <remarks>This method registers the service so that all resolved instances will be wrapped with the
    /// specified interceptors. Use this to enable cross-cutting concerns such as logging, validation, or authorization
    /// for the service. The service type and implementation type are the same.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> that specifies how the service will be instantiated within the container.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation. Interceptors are
    /// invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted service registration added.</returns>
    public static IServiceCollection AddIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddIntercepted<TService, TService>(lifetime, interceptors);

    /// <summary>
    /// Adds the specified service type to the dependency injection container with the given lifetime and applies the
    /// provided interceptors to it.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in the interceptorTypes array. This method is
    /// typically used to enable cross-cutting concerns such as logging, validation, or authorization for the registered
    /// service.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The service collection to which the service and its interceptors will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service is instantiated and shared.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddIntercepted<TService, TService>(lifetime, interceptorTypes);

    /// <summary>
    /// Adds the specified service type to the dependency injection container with interception support using the given
    /// interceptor and service lifetime.
    /// </summary>
    /// <remarks>Use this method to register a service so that all resolved instances are automatically
    /// intercepted by the specified interceptor. This is useful for cross-cutting concerns such as logging, validation,
    /// or authorization. The service and interceptor types must both have public constructors.</remarks>
    /// <typeparam name="TService">The type of the service to register for interception. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance, to allow for method chaining.</returns>
    public static IServiceCollection AddIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.AddIntercepted(typeof(TService), typeof(TService), lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a service of the specified type to the dependency injection container with the given lifetime, using a
    /// factory for instantiation and applying the specified interceptors.
    /// </summary>
    /// <remarks>This method enables interception of service method calls by applying the specified
    /// interceptors. Interceptors are invoked in the order provided. This is useful for cross-cutting concerns such as
    /// logging, validation, or authorization.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="factory">A factory function that creates an instance of the service. The function receives the current <see
    /// cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service is instantiated and shared.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Interceptors can modify or extend the behavior of the
    /// service's methods.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddIntercepted(typeof(TService), factory, lifetime, interceptors);

    /// <summary>
    /// Adds a service of type <typeparamref name="TService"/> to the dependency injection container with the specified factory, lifetime, and
    /// interceptors.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in interceptorTypes. All interceptor types
    /// must be compatible with the interception mechanism used by the container. This method enables advanced scenarios
    /// such as aspect-oriented programming by allowing cross-cutting concerns to be injected into service
    /// calls.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="factory">A factory function that creates an instance of TService using the provided IServiceProvider.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service will be instantiated and reused.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddIntercepted(typeof(TService), factory, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a service of the specified type to the dependency injection container with interception using the specified
    /// interceptor type.
    /// </summary>
    /// <remarks>The registered service will be wrapped with the specified interceptor, allowing interception
    /// of method calls. The interceptor type must have a public constructor. This method is typically used to enable
    /// cross-cutting concerns such as logging, validation, or authorization via interception.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="factory">A factory function that creates an instance of the service. The function receives the <see
    /// cref="IServiceProvider"/> for resolving dependencies.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, singleton, scoped, or transient).</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.AddIntercepted(typeof(TService), factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a service of the specified type to the dependency injection container with the given lifetime and applies
    /// the provided interceptors to its implementation.
    /// </summary>
    /// <remarks>Use this method to register a service with interception capabilities, allowing custom logic
    /// to be executed before or after service method calls. The order of interceptors in the array determines the order
    /// in which they are applied.</remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must have a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service is instantiated and shared.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service implementation. Interceptors can be used to add cross-cutting
    /// behavior such as logging or validation.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted(typeof(TService), typeof(TImplementation), lifetime, interceptors);

    /// <summary>
    /// Registers the specified service type and implementation type with interception support in the dependency
    /// injection container.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in the interceptorTypes array. This method
    /// enables cross-cutting concerns such as logging, validation, or authorization to be injected into service
    /// calls.</remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted(typeof(TService), typeof(TImplementation), lifetime, interceptorTypes);

    /// <summary>
    /// Adds a service of the specified type with interception support to the dependency injection container.
    /// </summary>
    /// <remarks>Use this method to register a service so that calls to its methods can be intercepted by the
    /// specified interceptor. This is useful for cross-cutting concerns such as logging, validation, or authorization.
    /// The interceptor must implement IInterceptor and be able to be constructed by the dependency injection
    /// container.</remarks>
    /// <typeparam name="TService">The interface or base type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must have a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The interceptor type to apply to the service. Must implement IInterceptor and have a public constructor.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <returns>The IServiceCollection instance with the intercepted service registration added. This enables method
    /// interception for the specified service type.</returns>
    public static IServiceCollection AddIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.AddIntercepted(typeof(TService), typeof(TImplementation), lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a service of the specified type to the dependency injection container with support for method interception
    /// using the provided interceptors.
    /// </summary>
    /// <remarks>This method enables interception of service methods by registering the implementation with
    /// the specified interceptors. The service will be resolved using the provided factory and lifetime. Interceptors
    /// are applied in the order specified in the <paramref name="interceptors"/> array.</remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must have public constructors.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="factory">A factory function that creates an instance of <typeparamref name="TImplementation"/> using the provided <see
    /// cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. Interceptors can be used to add cross-cutting behavior such as
    /// logging or validation.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted<TService>(factory, lifetime, interceptors);

    /// <summary>
    /// Registers a service with interception support using a factory delegate and the specified lifetime.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in the interceptorTypes array. This method
    /// enables cross-cutting concerns such as logging, validation, or authorization to be injected into service
    /// calls.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must be a reference type and implement or derive from TService.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="factory">A delegate that creates an instance of the implementation type using the provided service provider.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.AddIntercepted<TService>(factory, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a service of the specified type with interception support to the dependency injection container using a
    /// factory method and the specified lifetime.
    /// </summary>
    /// <remarks>Use this method to register a service with an interceptor, allowing cross-cutting concerns
    /// such as logging or validation to be applied to the service implementation.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete type that implements the service.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must have a public constructor.</typeparam>
    /// <param name="services">The dependency injection service collection to add the intercepted service to.</param>
    /// <param name="factory">A factory function that creates an instance of the implementation type using the provided service provider.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted<TService, TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TImplementation> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.AddIntercepted<TService, TInterceptor>(factory, lifetime);

    /// <summary>
    /// Adds a service of the specified type to the dependency injection container with the given lifetime, applying the
    /// provided interceptors to its implementation.
    /// </summary>
    /// <remarks>This method enables interception for services registered in the dependency injection
    /// container, allowing behaviors to be injected at runtime. The order of interceptors in the array determines the
    /// order in which they are applied.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which the service will be registered, such as Singleton, Scoped, or Transient.</param>
    /// <param name="interceptors">An array of interceptors to apply to the service implementation. Interceptors can be used to add cross-cutting
    /// behavior such as logging or validation.</param>
    /// <returns>The updated IServiceCollection instance with the intercepted service registration included.</returns>
    public static IServiceCollection AddIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors) =>
      services.AddIntercepted(serviceType, serviceType, lifetime, interceptors);

    /// <summary>
    /// Adds a service of the specified type to the dependency injection container with the given lifetime and applies
    /// the specified interceptors to it.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in <paramref name="interceptorTypes"/>. This
    /// method registers the service such that calls to the service will be intercepted by the provided
    /// interceptors.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service and interceptors are added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service is instantiated and shared.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.AddIntercepted(serviceType, serviceType, lifetime, interceptorTypes);

    /// <summary>
    /// Registers a service of the specified type with interception support in the dependency injection container.
    /// </summary>
    /// <remarks>This method enables dynamic interception of service calls by registering a proxy for the
    /// specified service type. Interceptors can be used to add cross-cutting concerns such as logging, validation, or
    /// authorization. The order of interceptors in the array determines the order in which they are applied.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be <see langword="null"/>.</param>
    /// <param name="factory">A factory function that creates the service implementation instance. Cannot be <see langword="null"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. If no interceptors are specified, the service will be
    /// registered without interception.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddIntercepted(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedServiceDescriptor(services, serviceType, factory, lifetime, interceptors);
      services.Add(interceptedDescriptor);
      return services;
    }

    /// <summary>
    /// Registers a service of the specified type with interception support in the dependency injection container.
    /// </summary>
    /// <remarks>If one or more interceptor types are specified, the service will be wrapped in a dynamic
    /// proxy that applies the provided interceptors to method calls. If no interceptors are specified, the service is
    /// registered as usual without interception. Interceptors are created per service instance using dependency
    /// injection. This method is typically used to add cross-cutting concerns such as logging, validation, or
    /// authorization to services.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be null.</param>
    /// <param name="factory">A factory function that creates the service implementation instance. Cannot be null.</param>
    /// <param name="lifetime">The lifetime with which to register the service (e.g., Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">One or more types implementing IInterceptor to apply to the service. If no types are provided, the service is
    /// registered without interception. Each type must have a public constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedServiceDescriptor(services, serviceType, factory, lifetime, interceptorTypes);
      services.Add(interceptedDescriptor);
      return services;
    }

    /// <summary>
    /// Adds a service of the specified type with interception support to the dependency injection container.
    /// </summary>
    /// <remarks>This method enables interception of service method calls by registering the service with the
    /// specified interceptors. Interceptors are applied in the order provided. Use this method when you want to add
    /// cross-cutting concerns such as logging, validation, or authorization to a service.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be <see langword="null"/>.</param>
    /// <param name="implementationType">The type that implements the service. An instance of this type will be created for the service. Cannot be <see langword="null"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors will be invoked for calls to the service's
    /// methods.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="implementationType"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddIntercepted(
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
      return services.AddIntercepted(serviceType, implementationFactory, lifetime, interceptors);
    }

    /// <summary>
    /// Adds a service of the specified type with the given implementation and lifetime to the service collection,
    /// applying the specified interceptors to the implementation.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in the interceptorTypes array. This method
    /// enables cross-cutting concerns, such as logging or validation, to be injected into service implementations via
    /// interception.</remarks>
    /// <param name="services">The service collection to which the service and interceptors are added. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. This is the contract type that consumers will request. Cannot be null.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor. Cannot be null.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service is instantiated and shared.</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service implementation. Each type must have a public
    /// constructor. If no interceptors are specified, the service is registered without interception.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddIntercepted(
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
      return services.AddIntercepted(serviceType, implementationFactory, lifetime, interceptorTypes);
    }

    /// <summary>
    /// Adds a keyed service of type <typeparamref name="TService"/> to the service collection with the specified
    /// lifetime and applies the provided interceptors to its activation.
    /// </summary>
    /// <remarks>Use this method to register services that require interception logic, such as logging or
    /// validation, and need to be distinguished by a key. The service will be resolved with the specified interceptors
    /// applied whenever the key is used.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with public constructors.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">An object that uniquely identifies the service registration. Can be <see langword="null"/> if no key is
    /// required.</param>
    /// <param name="lifetime">The lifetime with which the service will be registered. Specifies how the service will be instantiated and
    /// managed.</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These will be invoked during service activation.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the keyed, intercepted service registration added.</returns>
    public static IServiceCollection AddKeyedIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddKeyedIntercepted<TService, TService>(serviceKey, lifetime, interceptors);

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type to the dependency injection container with the given
    /// lifetime and interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service implementation that is associated with a specific key
    /// and is subject to interception by the specified interceptors. This is useful for scenarios where multiple
    /// implementations of the same service type are registered with different keys and require interception (e.g., for
    /// logging, validation, or other cross-cutting concerns).</remarks>
    /// <typeparam name="TService">The type of the service to register. Must have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null to register the default keyed service.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Determines how the service will be instantiated and reused.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddKeyedIntercepted<TService, TService>(serviceKey, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection and configures it to be intercepted by the
    /// specified interceptor type.
    /// </summary>
    /// <remarks>Use this method to register a service with a specific key and have all resolutions of that
    /// service intercepted by the specified interceptor. This is useful for scenarios where multiple implementations of
    /// a service are registered with different keys and require interception.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be <see langword="null"/> to register an unkeyed
    /// service.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted, keyed service registration added.</returns>
    public static IServiceCollection AddKeyedIntercepted<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.AddKeyedIntercepted<TService, TService>(serviceKey, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection with interception support using the
    /// provided interceptors.
    /// </summary>
    /// <remarks>This method enables interception for a keyed service registration, allowing cross-cutting
    /// concerns such as logging, validation, or caching to be applied. The service will be resolved with the specified
    /// interceptors whenever it is requested by the given key.</remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must have a public constructor.</typeparam>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceKey">An optional key that uniquely identifies the service registration. Can be null to register the default service.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. At least one interceptor must be provided.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.AddKeyedIntercepted(typeof(TService), serviceKey, typeof(TImplementation), lifetime, interceptors);

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type with the given implementation and interceptors to the
    /// service collection.
    /// </summary>
    /// <remarks>This method enables interception for a keyed service registration, allowing cross-cutting
    /// concerns such as logging or validation to be applied via the specified interceptors. The service is registered
    /// with the provided key and lifetime. Interceptors are applied in the order specified in the interceptorTypes
    /// array.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a class that implements or derives from
    /// TService and have public constructors.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null to register an unkeyed service.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">One or more types of interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.AddKeyedIntercepted(typeof(TService), serviceKey, typeof(TImplementation), lifetime, interceptorTypes);

    /// <summary>
    /// Adds a keyed service of the specified type with an interceptor to the dependency injection container.
    /// </summary>
    /// <remarks>Use this method to register a service implementation with a specific key and have an
    /// interceptor applied to it. This is useful for scenarios where multiple implementations of the same service type
    /// are registered with different keys and require interception.</remarks>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must have a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must have a public constructor.</typeparam>
    /// <param name="services">The collection of service descriptors to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null to register an unkeyed service.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService =>
      services.AddKeyedIntercepted(typeof(TService), serviceKey, typeof(TImplementation), lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type to the service collection using the provided factory and
    /// interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service with interception logic and an optional key, allowing
    /// for advanced scenarios such as multiple implementations or dynamic proxying. Interceptors are applied in the
    /// order specified.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">An optional key that uniquely identifies the service registration. Can be null to register an unkeyed service.</param>
    /// <param name="factory">A factory function that creates an instance of the service. Receives the service provider and the service key as
    /// parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and reused.</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors can modify or extend the behavior of the
    /// service.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.AddKeyedIntercepted(typeof(TService), serviceKey, factory, lifetime, interceptors);

    /// <summary>
    /// Adds a keyed, intercepted service of type TService to the specified IServiceCollection using the provided
    /// factory and interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service with interception logic and an optional key, allowing
    /// multiple implementations of the same service type to be distinguished by key. Interceptors are applied in the
    /// order specified.</remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type.</typeparam>
    /// <param name="services">The IServiceCollection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null for unkeyed registrations.</param>
    /// <param name="factory">A factory function that creates an instance of TService. Receives the IServiceProvider and the service key as
    /// parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The IServiceCollection instance with the intercepted, keyed service registration added. This enables method
    /// chaining.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.AddKeyedIntercepted(typeof(TService), serviceKey, factory, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection and configures it to be intercepted by the
    /// specified interceptor type.
    /// </summary>
    /// <remarks>Use this method to register a service with a specific key and have all resolutions of that
    /// service intercepted by the specified interceptor. This is useful for scenarios where multiple implementations of
    /// a service are registered with different keys and require interception logic such as logging, validation, or
    /// authorization.</remarks>
    /// <typeparam name="TService">The type of the service to register. This must be a class.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply to the service. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be <see langword="null"/> for unkeyed services.</param>
    /// <param name="factory">A factory function that creates an instance of the service. Receives the <see cref="IServiceProvider"/> and the
    /// service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service. Specifies how the service will be instantiated and shared.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TService> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.AddKeyedIntercepted(typeof(TService), serviceKey, factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type to the service collection using the provided factory and
    /// interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service with a specific key and have its method calls
    /// intercepted by the provided interceptors. This is useful for scenarios where multiple implementations of a
    /// service are registered under different keys and require interception logic such as logging, validation, or
    /// authorization.</remarks>
    /// <typeparam name="TService">The type of the service to register. This must be a class type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. This must be a class type with public constructors and must
    /// implement or derive from TService.</typeparam>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceKey">An optional key that uniquely identifies the service registration. Can be null to register the default keyed
    /// service.</param>
    /// <param name="factory">A factory function that creates an instance of the implementation type. Receives the service provider and the
    /// service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. These interceptors will be invoked for each service method
    /// call.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> factory,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.AddKeyedIntercepted<TService>(serviceKey, factory, lifetime, interceptors);

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type to the dependency injection container using a factory
    /// and the given interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service with a specific key and have method calls on the
    /// service intercepted by the specified interceptors. This is useful for scenarios where multiple implementations
    /// of a service are registered under different keys and require interception (such as logging, validation, or
    /// authorization). The interceptors are applied in the order specified in the interceptorTypes array.</remarks>
    /// <typeparam name="TService">The type of the service to register. This must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. This must be a class that implements or derives from TService.</typeparam>
    /// <param name="services">The IServiceCollection to which the service will be added.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null to register the default service.</param>
    /// <param name="factory">A factory function that creates an instance of the implementation type. Receives the IServiceProvider and the
    /// service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a public
    /// constructor.</param>
    /// <returns>The IServiceCollection instance with the intercepted, keyed service registration added.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, TImplementation>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> factory,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.AddKeyedIntercepted<TService>(serviceKey, factory, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type to the dependency injection container using the provided
    /// factory and interceptor.
    /// </summary>
    /// <remarks>Use this method to register a service with a specific key and have calls to the service
    /// intercepted by the specified interceptor. This is useful for scenarios where multiple implementations of a
    /// service are registered with different keys and require interception logic.</remarks>
    /// <typeparam name="TService">The service type to register. Must be a class.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type of the service. Must inherit from or implement TService and be a class.</typeparam>
    /// <typeparam name="TInterceptor">The interceptor type to apply to the service. Must implement IInterceptor and have a public constructor.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null for unkeyed registrations.</param>
    /// <param name="factory">A factory function that creates an instance of the implementation type. Receives the IServiceProvider and the
    /// service key as parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service (e.g., Singleton, Scoped, or Transient).</param>
    /// <returns>The IServiceCollection instance with the intercepted, keyed service registration added.</returns>
    public static IServiceCollection AddKeyedIntercepted<TService, TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      object? serviceKey,
      Func<IServiceProvider, object?, TImplementation> factory,
      ServiceLifetime lifetime)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.AddKeyedIntercepted<TService>(serviceKey, factory, lifetime, typeof(TInterceptor));

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection with interception support, using the
    /// provided interceptors and service lifetime.
    /// </summary>
    /// <remarks>Use this method to register services that require interception logic, such as logging,
    /// validation, or authorization, and need to be distinguished by a key. The service will be resolved with the
    /// specified key and have the provided interceptors applied to its method calls.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">An object that uniquely identifies the service registration. Can be null if no key is required.</param>
    /// <param name="lifetime">The lifetime with which the service will be registered, such as Singleton, Scoped, or Transient.</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These will be invoked when the service's methods are called.</param>
    /// <returns>The updated service collection containing the registered keyed intercepted service.</returns>
    public static IServiceCollection AddKeyedIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      ServiceLifetime lifetime,
      params IInterceptor[] interceptors) =>
      services.AddKeyedIntercepted(serviceType, serviceKey, serviceType, lifetime, interceptors);

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection and configures it to be intercepted by the
    /// provided interceptors.
    /// </summary>
    /// <remarks>Use this method to register a service implementation that is associated with a specific key
    /// and is subject to interception by the specified interceptors. This is useful for scenarios where multiple
    /// implementations of the same service type are registered with different keys and require interception (such as
    /// logging, validation, or authorization).</remarks>
    /// <param name="services">The service collection to which the keyed, intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null to indicate the default registration.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">One or more types of interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      object? serviceKey,
      ServiceLifetime lifetime,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.AddKeyedIntercepted(serviceType, serviceKey, serviceType, lifetime, interceptorTypes);

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection with dynamic interception applied using the
    /// provided interceptors.
    /// </summary>
    /// <remarks>This method enables advanced scenarios where services are registered with a key and require
    /// interception, such as for logging, validation, or other cross-cutting concerns. The service implementation is
    /// wrapped in a dynamic proxy that applies the specified interceptors. The serviceKey parameter must be unique
    /// within the service collection for the given serviceType.</remarks>
    /// <param name="services">The service collection to which the intercepted service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This type will be proxied to enable interception.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be <see langword="null"/>.</param>
    /// <param name="factory">A factory function that creates the service implementation. Receives the service provider and the service key as
    /// parameters.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">One or more interceptors to apply to the service. These interceptors will be invoked for each method call on the
    /// proxied service.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="factory"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddKeyedIntercepted(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> factory, ServiceLifetime lifetime, params IInterceptor[] interceptors)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedKeyedServiceDescriptor(services, serviceType, serviceKey, factory, lifetime, interceptors);
      services.Add(interceptedDescriptor);

      return services;
    }

    /// <summary>
    /// Adds a keyed service of the specified type to the service collection with interception support using the
    /// provided interceptors.
    /// </summary>
    /// <remarks>If one or more interceptor types are provided, the service will be wrapped in a dynamic proxy
    /// that applies the specified interceptors. If no interceptors are specified, the service is registered directly
    /// without interception. All interceptor types must implement IInterceptor and be constructible via public
    /// constructors. This method enables advanced scenarios such as aspect-oriented programming or cross-cutting
    /// concerns (e.g., logging, validation) for keyed services.</remarks>
    /// <param name="services">The service collection to which the service and its interceptors are added. Cannot be null.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be null.</param>
    /// <param name="serviceKey">The key associated with the service registration. May be null for unkeyed registrations.</param>
    /// <param name="factory">A factory function that creates the service instance. The function receives the service provider and the service
    /// key as parameters. Cannot be null.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">An array of types implementing interception logic to be applied to the service. Each type must implement
    /// IInterceptor and have a public constructor. If null or empty, the service is registered without interception.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> factory, ServiceLifetime lifetime, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ServiceDescriptor interceptedDescriptor = CreateInterceptedKeyedServiceDescriptor(services, serviceType, serviceKey, factory, lifetime, interceptorTypes);
      services.Add(interceptedDescriptor);

      return services;
    }

    /// <summary>
    /// Adds a keyed, intercepted service of the specified type and implementation to the service collection with the
    /// given lifetime.
    /// </summary>
    /// <remarks>This method enables registration of a service with a specific key and applies the provided
    /// interceptors to the service implementation. Use this when you need to distinguish between multiple registrations
    /// of the same service type by key and require interception (such as for logging, validation, or other
    /// cross-cutting concerns).</remarks>
    /// <param name="services">The service collection to which the service will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceType">The type of the service to register. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be <see langword="null"/>.</param>
    /// <param name="implementationType">The concrete type that implements the service. Cannot be <see langword="null"/>.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptors">An array of interceptors to apply to the service. May be empty.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/>, <paramref name="serviceType"/>, or <paramref name="implementationType"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddKeyedIntercepted(this IServiceCollection services, Type serviceType, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime, params IInterceptor[] interceptors)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(implementationType);

      Func<IServiceProvider, object?, object> implementationFactory = (sp, _) => ActivatorUtilities.CreateInstance(sp, implementationType);
      return services.AddKeyedIntercepted(serviceType, serviceKey, implementationFactory, lifetime, interceptors);
    }

    /// <summary>
    /// Adds a keyed service of the specified type and implementation to the service collection with the given lifetime,
    /// applying the specified interceptors.
    /// </summary>
    /// <remarks>Interceptors are applied in the order specified in the interceptorTypes array. This method
    /// enables advanced scenarios such as aspect-oriented programming by allowing interception of service
    /// calls.</remarks>
    /// <param name="services">The service collection to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="serviceKey">The key that uniquely identifies the service registration. Can be null to register an unkeyed service.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="lifetime">The lifetime with which to register the service (for example, Singleton, Scoped, or Transient).</param>
    /// <param name="interceptorTypes">An array of types representing interceptors to apply to the service. Each type must have a public constructor.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedIntercepted(this IServiceCollection services, Type serviceType, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(implementationType);

      Func<IServiceProvider, object?, object> implementationFactory = (sp, _) => ActivatorUtilities.CreateInstance(sp, implementationType);
      return services.AddKeyedIntercepted(serviceType, serviceKey, implementationFactory, lifetime, interceptorTypes);
    }

    private static ServiceDescriptor CreateInterceptedKeyedServiceDescriptor(IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> factory, ServiceLifetime lifetime, IInterceptor[] interceptors)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(factory);

      if (interceptors is null || interceptors.Length == 0)
      {
        // No interception needed - register directly
        return new ServiceDescriptor(serviceType, serviceKey, factory, lifetime);
      }

      Func<IServiceProvider, IEnumerable<IInterceptor>> interceptorsFactory = (_) => interceptors;
      return CreateInterceptedKeyedServiceDescriptor(services, serviceType, serviceKey, factory, lifetime, interceptorsFactory);
    }

    private static ServiceDescriptor CreateInterceptedKeyedServiceDescriptor(IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> factory, ServiceLifetime lifetime, Type[] interceptorTypes)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(factory);

      if (interceptorTypes is null || interceptorTypes.Length == 0)
      {
        // No interception needed - register directly
        return new ServiceDescriptor(serviceType, serviceKey, factory, lifetime);
      }

      ValidateInterceptorTypes(interceptorTypes);

      Func<IServiceProvider, IEnumerable<IInterceptor>> interceptorsFactory =
        (sp) => interceptorTypes.Select((interceptorType) => (IInterceptor)ActivatorUtilities.GetServiceOrCreateInstance(sp, interceptorType));
      return CreateInterceptedKeyedServiceDescriptor(services, serviceType, serviceKey, factory, lifetime, interceptorsFactory);
    }

    private static ServiceDescriptor CreateInterceptedKeyedServiceDescriptor(IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> factory, ServiceLifetime lifetime, Func<IServiceProvider, IEnumerable<IInterceptor>> interceptorsFactory)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(factory);

      // Need a Proxy Generator to create the proxy.  Using a singleton instance to improve performance by not recreating it each time.
      services.TryAddKeyedSingleton(PROXY_GENERATOR_KEY, new ProxyGenerator());

      Func<IServiceProvider, object?, object> proxyFactory = (sp, key) =>
      {
        object implementation = factory(sp, key);
        IInterceptor[] interceptors = [.. interceptorsFactory(sp)];
        return CreateInterceptedServiceProxy(sp, serviceType, implementation, interceptors);
      };
      
      ServiceDescriptor interceptedDescriptor = new(serviceType, serviceKey, proxyFactory, lifetime);
      return interceptedDescriptor;
    }

    private static ServiceDescriptor CreateInterceptedServiceDescriptor(IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime, IInterceptor[] interceptors)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(factory);

      if (interceptors is null || interceptors.Length == 0)
      {
        // No interception needed - register directly
        return new ServiceDescriptor(serviceType, factory, lifetime);
      }

      Func<IServiceProvider, IEnumerable<IInterceptor>> interceptorsFactory = (_) => interceptors;
      return CreateInterceptedServiceDescriptor(services, serviceType, factory, lifetime, interceptorsFactory);
    }

    private static ServiceDescriptor CreateInterceptedServiceDescriptor(IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime, Type[] interceptorTypes)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(factory);

      if (interceptorTypes is null || interceptorTypes.Length == 0)
      {
        // No interception needed - register directly
        return new ServiceDescriptor(serviceType, factory, lifetime);
      }

      ValidateInterceptorTypes(interceptorTypes);

      Func<IServiceProvider, IEnumerable<IInterceptor>> interceptorsFactory =
        (sp) => interceptorTypes.Select((interceptorType) => (IInterceptor)ActivatorUtilities.GetServiceOrCreateInstance(sp, interceptorType));
      return CreateInterceptedServiceDescriptor(services, serviceType, factory, lifetime, interceptorsFactory);
    }

    private static ServiceDescriptor CreateInterceptedServiceDescriptor(IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime, Func<IServiceProvider, IEnumerable<IInterceptor>> interceptorsFactory)
    {
      ArgumentNullException.ThrowIfNull(services);
      ArgumentNullException.ThrowIfNull(serviceType);
      ArgumentNullException.ThrowIfNull(factory);

      // Need a Proxy Generator to create the proxy.  Using a singleton instance to improve performance by not recreating it each time. 
      services.TryAddKeyedSingleton(PROXY_GENERATOR_KEY, new ProxyGenerator());

      Func<IServiceProvider, object> interceptedFactory = (sp) =>
      {
        // Get the implementation
        object implementation = factory(sp);
        IInterceptor[] interceptors = [.. interceptorsFactory(sp)];
        return CreateInterceptedServiceProxy(sp, serviceType, implementation, interceptors);
      };

      ServiceDescriptor interceptedDescriptor = new(serviceType, interceptedFactory, lifetime);
      return interceptedDescriptor;
    }

    private static object CreateInterceptedServiceProxy(IServiceProvider sp, Type serviceType, object implementation, params IInterceptor[] interceptors)
    {
      if (interceptors.Length == 0)
      {
        return implementation;
      }

      ProxyGenerator proxyGenerator = sp.GetRequiredKeyedService<ProxyGenerator>(PROXY_GENERATOR_KEY);
      return serviceType.IsInterface
        ? proxyGenerator.CreateInterfaceProxyWithTarget(serviceType, implementation, interceptors)
        : proxyGenerator.CreateClassProxyWithTarget(sp, serviceType, implementation, interceptors);
    }

    private static bool IsInterceptorType(Type interceptorType) =>
      interceptorType is not null
        && interceptorType.IsAssignableTo(typeof(IInterceptor))
        && interceptorType.IsClass
        && !interceptorType.IsAbstract
        && interceptorType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length > 0;

    private static void ValidateInterceptorTypes(Type[] interceptorTypes)
    {
      foreach (Type interceptorType in interceptorTypes)
      {
        if (!IsInterceptorType(interceptorType))
        {
          // Throw an exception here at configuration/startup
          string exMsg = $"The type '{interceptorType.Name}' must be a concrete class that implements {nameof(IInterceptor)}.  Ensure the type is public, non-abstract and has a public constructor";
          throw new ArgumentException(exMsg, nameof(interceptorTypes));
        }
      }
    }
  }
}