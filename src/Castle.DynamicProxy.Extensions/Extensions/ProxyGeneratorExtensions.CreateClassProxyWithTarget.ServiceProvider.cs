// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensions.CreateClassProxyWithTarget.ServiceProvider.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Castle.DynamicProxy.Extensions
{
  public static partial class ProxyGeneratorExtensions
  {
    /// <summary>
    /// Creates a proxy for the specified class type using the given target instance and interceptors.
    /// </summary>
    /// <remarks>This method is typically used to create proxies for classes that require dependency injection
    /// or custom interception logic. The returned proxy will use the provided target instance for method invocations,
    /// allowing interception of calls to virtual members.</remarks>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies for the proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which method calls are delegated. Can be <see langword="null"/>.</param>
    /// <param name="interceptors">An array of interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TClass that delegates calls to the specified target and applies the provided
    /// interceptors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/> or <paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxyWithTarget<TClass>(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, TClass? target, params IInterceptor[] interceptors) where TClass : class =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, target, ProxyGenerationOptions.Default, interceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given proxy generation options and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method creates a proxy with custom generation options that delegates method calls to the
    /// target object. The proxy will invoke the provided asynchronous interceptors for intercepted method calls.</remarks>
    /// <typeparam name="TClass">The type of the class to proxy. Must be a reference type.</typeparam>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies for the proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="interceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>A proxy instance of type TClass that delegates to the target with the specified options and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static TClass CreateClassProxyWithTarget<TClass>(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, TClass? target, ProxyGenerationOptions options, params IInterceptor[] interceptors) where TClass : class
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(serviceProvider);
      ArgumentNullException.ThrowIfNull(options);
      return (TClass)proxyGenerator.CreateClassProxyWithTarget(serviceProvider, typeof(TClass), Type.EmptyTypes, target, options, [], interceptors);
    }

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// optionally implementing additional interfaces and applying the given asynchronous interceptors.
    /// </summary>
    /// <remarks>The returned proxy delegates method calls to the target object and can be cast to the class
    /// type and any additional interfaces specified. This is useful for adding interface implementations to existing
    /// objects.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies for the proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types that the proxy should implement, or <see langword="null"/> if no additional interfaces are required.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="interceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target, implementing any additional interfaces and applying the provided interceptors.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, params IInterceptor[] interceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, additionalInterfacesToProxy, target, ProxyGenerationOptions.Default, [], interceptors);

    /// <summary>
    /// Creates a proxy instance for the specified class type that forwards calls to the provided target instance,
    /// applying the given proxy generation options, constructor arguments, and asynchronous interceptors.
    /// </summary>
    /// <remarks>This method allows specification of both proxy options and constructor arguments when creating
    /// a target-delegating proxy. The proxy will intercept method calls before delegating to the target.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies required by the proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class.</param>
    /// <param name="target">The target instance to which the proxy will delegate calls. Can be <see langword="null"/>.</param>
    /// <param name="options">The options that control proxy generation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> to use the default constructor.</param>
    /// <param name="interceptors">One or more asynchronous interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object instance that is a proxy delegating to the target with the specified options, constructor arguments, and interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, ProxyGenerationOptions options, object?[]? constructorArguments, params IInterceptor[] interceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, options, constructorArguments, interceptors);

    /// <summary>
    /// Creates a proxy for the specified class type, using the given target object and interceptors, with optional
    /// constructor arguments.
    /// </summary>
    /// <remarks>This method is typically used to create proxies that delegate calls to an existing target
    /// object, allowing interception of method calls for scenarios such as logging, validation, or authorization. The
    /// proxy will use the provided service provider to resolve any required services for interceptors or constructor
    /// injection.</remarks>
    /// <param name="proxyGenerator">The proxy generator instance used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies required by the proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract class type. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target object to which method calls will be delegated. Can be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the constructor of the proxied class. Can be <see langword="null"/> if the default constructor
    /// should be used.</param>
    /// <param name="interceptors">An array of interceptors to apply to the proxy. At least one interceptor must be provided.</param>
    /// <returns>An object representing the created proxy instance. The returned object is of the type specified by <paramref name="classToProxy"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, object?[]? constructorArguments, params IInterceptor[] interceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, constructorArguments, interceptors);

    /// <summary>
    /// Creates a proxy for the specified class type, using the given target object and interceptors.
    /// </summary>
    /// <remarks>This method creates a proxy that delegates calls to the provided target object while allowing
    /// method interception. The proxy is constructed using dependencies resolved from the specified service provider.
    /// Use this overload when you want to proxy an existing object instance rather than creating a new one.</remarks>
    /// <param name="proxyGenerator">The proxy generator instance used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies for the proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target object to which method calls will be delegated. Can be <see langword="null"/>.</param>
    /// <param name="interceptors">An array of interceptors to apply to the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/> elements.</param>
    /// <returns>An object representing the created proxy instance. The returned object is of the specified class type.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/> or <paramref name="classToProxy"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, params IInterceptor[] interceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, ProxyGenerationOptions.Default, [], interceptors);

    /// <summary>
    /// Creates a proxy for the specified class type, using the provided target object and interceptors.
    /// </summary>
    /// <remarks>The created proxy delegates method calls to the specified target object, allowing
    /// interception of method invocations. Use this method when you need to proxy an existing object instance rather
    /// than creating a new one.</remarks>
    /// <param name="proxyGenerator">The proxy generator instance used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve constructor dependencies for the proxy instance. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract, non-sealed class type. Cannot be <see langword="null"/>.</param>
    /// <param name="target">The target object to which method calls will be delegated. Can be <see langword="null"/>.</param>
    /// <param name="options">The proxy generation options that control proxy creation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="interceptors">An array of interceptors to apply to the proxy. Cannot be <see langword="null"/> and must not contain <see langword="null"/> elements.</param>
    /// <returns>An instance of the proxied class type with the specified interceptors applied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/>, <paramref name="classToProxy"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, object? target, ProxyGenerationOptions options, params IInterceptor[] interceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, Type.EmptyTypes, target, options, [], interceptors);

    /// <summary>
    /// Creates a proxy for the specified class type, using the given target object and interceptors, and supporting
    /// additional interfaces as needed.
    /// </summary>
    /// <remarks>The proxy delegates method calls to the specified target object, allowing interception of
    /// calls for cross-cutting concerns such as logging or authorization. The returned proxy can be cast to the class
    /// type and any additional interfaces specified.</remarks>
    /// <param name="proxyGenerator">The proxy generator instance used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies required during proxy creation. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-abstract class type.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types to be implemented by the proxy, or <see langword="null"/> if no additional interfaces are
    /// required.</param>
    /// <param name="target">The target object to which method calls will be delegated. Can be <see langword="null"/>.</param>
    /// <param name="options">The proxy generation options that control proxy creation behavior. Cannot be <see langword="null"/>.</param>
    /// <param name="interceptors">An array of interceptors to be applied to method invocations on the proxy. Cannot be <see langword="null"/> or contain <see langword="null"/>
    /// elements.</param>
    /// <returns>An object representing the generated proxy instance. The returned object implements the specified class type and any additional interfaces provided.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/>, <paramref name="classToProxy"/>, <paramref name="target"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, ProxyGenerationOptions options, params IInterceptor[] interceptors) =>
      proxyGenerator.CreateClassProxyWithTarget(serviceProvider, classToProxy, additionalInterfacesToProxy, target, options, [], interceptors);

    /// <summary>
    /// Creates a proxy object for the specified class type, using the given target instance and interceptors, with
    /// support for additional interfaces and custom proxy generation options.
    /// </summary>
    /// <remarks>The proxy created by this method delegates method calls to the specified target object,
    /// allowing interception of method invocations for scenarios such as logging, validation, or dependency injection.
    /// The <paramref name="serviceProvider" /> is used to resolve any constructor dependencies required by the proxied class. All types
    /// provided in <paramref name="additionalInterfacesToProxy" /> must be interfaces and not generic type definitions.</remarks>
    /// <param name="proxyGenerator">The proxy generator used to create the class proxy. Cannot be <see langword="null"/>.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies required by the proxy's constructor. Cannot be <see langword="null"/>.</param>
    /// <param name="classToProxy">The type of the class to proxy. Must be a non-generic class type. Cannot be <see langword="null"/>.</param>
    /// <param name="additionalInterfacesToProxy">An array of additional interface types to be implemented by the proxy, or <see langword="null"/> if no additional interfaces are
    /// required. Each type must be a non-generic interface.</param>
    /// <param name="target">The target object to which method calls will be delegated. Can be <see langword="null"/> if the proxy does not require a target instance.</param>
    /// <param name="options">The options used to customize proxy generation. Cannot be <see langword="null"/>.</param>
    /// <param name="constructorArguments">An array of arguments to pass to the proxied class's constructor, or <see langword="null"/> if no arguments are required.</param>
    /// <param name="interceptors">The interceptors to apply to method invocations on the proxy. At least one interceptor must be provided.</param>
    /// <returns>An instance of the proxy object implementing the specified class and any additional interfaces. The returned
    /// object can be cast to the type specified by <paramref name="classToProxy"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="classToProxy"/> is not a class type.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="proxyGenerator"/>, <paramref name="serviceProvider"/>, <paramref name="classToProxy"/>, <paramref name="target"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
    [SuppressMessage("Not Focused", "S107", Justification = "Necessary for extension method syntax.")]
    public static object CreateClassProxyWithTarget(this IProxyGenerator proxyGenerator, IServiceProvider serviceProvider, Type classToProxy, Type[]? additionalInterfacesToProxy, object? target, ProxyGenerationOptions options, object?[]? constructorArguments, params IInterceptor[] interceptors)
    {
      ArgumentNullException.ThrowIfNull(proxyGenerator);
      ArgumentNullException.ThrowIfNull(serviceProvider);
      ArgumentNullException.ThrowIfNull(classToProxy);
      ArgumentNullException.ThrowIfNull(options);

      if (!classToProxy.IsClass)
      {
        throw new ArgumentException("'classToProxy' must be a class", nameof(classToProxy));
      }

      IsNotGenericTypeDefinition(classToProxy);
      AreNotGenericTypeDefinitions(additionalInterfacesToProxy);
      Type proxyType = proxyGenerator.ProxyBuilder.CreateClassProxyTypeWithTarget(classToProxy, additionalInterfacesToProxy, options);
      List<object?> proxyConstructorArgs = BuildConstructorArgumentsForClassProxyWithTarget(target, options, constructorArguments, interceptors);

      try
      {
        // Ensure that the constructor args is an array, not a List<object?> instance
        return ActivatorUtilities.CreateInstance(serviceProvider, proxyType, [.. proxyConstructorArgs!]);
      }
      catch (Exception innerException) when (innerException is InvalidOperationException or MissingMethodException)
      {
        StringBuilder sb = new();
        _ = sb.AppendFormat("Can not instantiate proxy of class: {0}.", classToProxy.FullName).AppendLine();
        if (constructorArguments == null || constructorArguments.Length == 0)
        {
          _ = sb.Append("Could not find a parameterless constructor.");
        }
        else
        {
          _ = sb.AppendLine("Could not find a constructor that would match given arguments:");
          foreach (object? obj in constructorArguments)
          {
            _ = sb.AppendLine((obj == null) ? "<null>" : obj.GetType().ToString());
          }
        }

        throw new ArgumentException(sb.ToString(), nameof(constructorArguments), innerException);
      }
    }

    private static void AreNotGenericTypeDefinitions(IEnumerable<Type>? types, [CallerArgumentExpression(nameof(types))] string? paramName = null)
    {
      if (types == null)
      {
        return;
      }

      foreach (Type type in types)
      {
        IsNotGenericTypeDefinition(type, paramName);
      }
    }

    private static List<object?> BuildConstructorArgumentsForClassProxyWithTarget(object? target, ProxyGenerationOptions options, object?[]? classToProxyConstructorArgs, IInterceptor[] interceptors)
    {
      // SAMPLE: .ctor({intercepted instance}, Castle.DynamicProxy.IInterceptor[], {intercepted type constructor args})
      List<object?> args = [target, .. options.MixinData.Mixins, interceptors];

      if (options.Selector is not null)
      {
        args.Add(options.Selector);
      }

      if (classToProxyConstructorArgs is not null && classToProxyConstructorArgs.Length != 0)
      {
        args.AddRange(classToProxyConstructorArgs);
      }

      return args;
    }

    private static void IsNotGenericTypeDefinition(Type type, [CallerArgumentExpression(nameof(type))] string? paramName = null)
    {
      if (type is null || !type.IsGenericTypeDefinition)
      {
        return;
      }

      throw new ArgumentException($"Can not create proxy for type {type.FullName ?? type.Name} because it is an open generic type.", paramName);
    }
  }
}
