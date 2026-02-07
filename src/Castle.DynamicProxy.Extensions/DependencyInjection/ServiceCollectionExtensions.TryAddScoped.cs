// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.TryAddScoped.cs" company="Karma, LLC">
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
    /// Attempts to add a scoped service of the specified type to the dependency injection container with the provided
    /// interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). If the service type has already been registered,
    /// this method does not add a duplicate registration. Interception allows additional behavior to be injected when
    /// the service's methods are invoked.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      params IInterceptor[] interceptors) =>
      services.TryAddIntercepted(serviceType, ServiceLifetime.Scoped, interceptors);

    /// <summary>
    /// Attempts to add a scoped service of the specified type to the dependency injection container with the provided
    /// interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container when needed. Interceptors are applied in the
    /// order specified. If the service type is already registered, no changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. Must have a public constructor.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddIntercepted(serviceType, ServiceLifetime.Scoped, interceptorTypes);

    /// <summary>
    /// Attempts to add a scoped service of the specified type and implementation to the dependency injection container
    /// with the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with a separate
    /// implementation type, allowing the service to be resolved via an interface or base class while using a concrete
    /// implementation. Interceptors are applied to the implementation. If the service type is already registered, no
    /// changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped(
      this IServiceCollection services,
      Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      params IInterceptor[] interceptors) =>
      services.TryAddIntercepted(serviceType, implementationType, ServiceLifetime.Scoped, interceptors);

    /// <summary>
    /// Attempts to add a scoped service of the specified type and implementation to the dependency injection container
    /// with the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container. Use this when you need to separate the
    /// service interface from its implementation and apply interceptors. If the service type is already registered, no
    /// changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register. This is typically an interface or base class.</param>
    /// <param name="implementationType">The concrete type that implements the service. Must have a public constructor.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped(
      this IServiceCollection services,
      Type serviceType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddIntercepted(serviceType, implementationType, ServiceLifetime.Scoped, interceptorTypes);

    /// <summary>
    /// Attempts to add a scoped service of the specified type to the dependency injection container using a factory
    /// method and the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method uses a factory function to create the
    /// service instance, allowing for custom initialization logic. The factory is invoked once per scope, and the
    /// resulting instance is wrapped with the specified interceptors. If the service type is already registered, no
    /// changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> implementationFactory,
      params IInterceptor[] interceptors) =>
      services.TryAddIntercepted(serviceType, implementationFactory, ServiceLifetime.Scoped, interceptors);

    /// <summary>
    /// Attempts to add a scoped service of the specified type to the dependency injection container using a factory
    /// method and the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method uses a factory function to create the
    /// service instance, with interceptors resolved from the dependency injection container. The factory is invoked once
    /// per scope. If the service type is already registered, no changes are made.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped(
      this IServiceCollection services,
      Type serviceType,
      Func<IServiceProvider, object> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes) =>
      services.TryAddIntercepted(serviceType, implementationFactory, ServiceLifetime.Scoped, interceptorTypes);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> to the dependency injection container
    /// using a factory method and the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method uses a factory function to create the
    /// service instance, allowing for custom initialization logic. The factory is invoked once per scope, and the
    /// resulting instance is wrapped with the specified interceptors. If the service type is already registered, no
    /// changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> implementationFactory,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddIntercepted(implementationFactory, ServiceLifetime.Scoped, interceptors);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> to the dependency injection container
    /// using a factory method and the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method uses a factory function to create the
    /// service instance, with interceptors resolved from the dependency injection container. The factory is invoked once
    /// per scope. If the service type is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> implementationFactory,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddIntercepted(implementationFactory, ServiceLifetime.Scoped, interceptorTypes);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> to the dependency injection container
    /// using a factory method and the specified interceptor type if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method uses a factory function to create the
    /// service instance and applies the specified interceptor type, which will be resolved from the dependency injection
    /// container. If the service type is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="implementationFactory">A factory function that creates an instance of the service. The function receives the
    /// current <see cref="IServiceProvider"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(
      this IServiceCollection services,
      Func<IServiceProvider, TService> implementationFactory)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted<TService, TInterceptor>(implementationFactory, ServiceLifetime.Scoped);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> to the dependency injection container
    /// with the provided interceptors if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service so that all
    /// resolved instances within the same scope will be wrapped with the specified interceptors. The service type and
    /// implementation type are the same. Use this to enable cross-cutting concerns such as logging, validation, or
    /// authorization. If the service type is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      params IInterceptor[] interceptors)
      where TService : class =>
      services.TryAddIntercepted<TService>(ServiceLifetime.Scoped, interceptors);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> to the dependency injection container
    /// with the provided interceptor types if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container when needed. Interceptors are applied in the
    /// order specified. If the service type is already registered, no changes are made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class =>
      services.TryAddIntercepted<TService>(ServiceLifetime.Scoped, interceptorTypes);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> to the dependency injection container
    /// with the specified interceptor type if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with an
    /// interceptor type that will be resolved from the dependency injection container. Use this for cross-cutting
    /// concerns such as logging, validation, or authorization. If the service type is already registered, no changes are
    /// made.
    /// </remarks>
    /// <typeparam name="TService">The type of the service to register. Must be a reference type with a public constructor.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(this IServiceCollection services)
      where TService : class
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted<TService, TInterceptor>(ServiceLifetime.Scoped);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptors if it
    /// has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with a separate
    /// implementation type, allowing the service to be resolved via an interface or base class while using a concrete
    /// implementation. Interceptors are applied to the implementation. If the service type is already registered, no
    /// changes are made.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptors">An array of <see cref="IInterceptor"/> instances to be applied to the service implementation.
    /// Interceptors are invoked in the order provided.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      params IInterceptor[] interceptors)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddIntercepted<TService, TImplementation>(ServiceLifetime.Scoped, interceptors);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the provided interceptor types
    /// if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with interceptor
    /// types that will be resolved from the dependency injection container. Use this when you need to separate the
    /// service interface from its implementation and apply interceptors. If the service type is already registered, no
    /// changes are made.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <param name="interceptorTypes">An array of types representing the interceptors to apply to the service. Each type must have a
    /// public constructor.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
      this IServiceCollection services,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] params Type[] interceptorTypes)
      where TService : class
      where TImplementation : class, TService =>
      services.TryAddIntercepted<TService, TImplementation>(ServiceLifetime.Scoped, interceptorTypes);

    /// <summary>
    /// Attempts to add a scoped service of type <typeparamref name="TService"/> with implementation type
    /// <typeparamref name="TImplementation"/> to the dependency injection container with the specified interceptor type
    /// if it has not already been registered.
    /// </summary>
    /// <remarks>
    /// Scoped services are created once per client request (scope). This method registers the service with an
    /// interceptor type that will be resolved from the dependency injection container. Use this for cross-cutting
    /// concerns such as logging, validation, or authorization. If the service type is already registered, no changes are
    /// made.
    /// </remarks>
    /// <typeparam name="TService">The interface or base class type of the service to register. Must be a reference type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type to use for the service. Must be a reference type and implement
    /// or inherit from <typeparamref name="TService"/>.</typeparam>
    /// <typeparam name="TInterceptor">The type of the interceptor to apply. Must implement <see cref="IInterceptor"/> and have a public
    /// constructor.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the service will be added.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the intercepted scoped service registration added if it was not
    /// already present.</returns>
    public static IServiceCollection TryAddInterceptedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInterceptor>(this IServiceCollection services)
      where TService : class
      where TImplementation : class, TService
      where TInterceptor : IInterceptor =>
      services.TryAddIntercepted<TService, TImplementation, TInterceptor>(ServiceLifetime.Scoped);
  }
}
