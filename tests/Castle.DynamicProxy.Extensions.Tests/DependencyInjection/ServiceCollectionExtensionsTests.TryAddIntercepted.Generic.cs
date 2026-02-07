// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddIntercepted.Generic.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for generic TryAddIntercepted methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddIntercepted<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_TryAddIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddIntercepted<TestService>(lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method registers service when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_generic_TryAddIntercepted_single_type_registers_service()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddIntercepted<TestClassService>(lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method does not register service when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_generic_TryAddIntercepted_single_type_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      _ = _services.AddIntercepted<TestClassService>(lifetime, _interceptor);

      // Act
      _ = _services.TryAddIntercepted<TestClassService>(lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked - service not replaced");
    }

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_TryAddIntercepted_single_type_returns_same_instance()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services.TryAddIntercepted<TestClassService>(lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_TryAddIntercepted_single_type_returns_different_instances()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddIntercepted<TestClassService>(lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method with no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_TryAddIntercepted_single_type_registers_service_without_proxy()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services.TryAddIntercepted<TestService>(lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestService? service = provider.GetService<TestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that TryAddIntercepted generic single-type method returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddIntercepted_single_type_returns_service_collection_for_chaining()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services.TryAddIntercepted<TestClassService>(lifetime, _interceptor);

      // Assert
      Assert.AreSame(_services, result, "Should return the same service collection for chaining");
    }

    #endregion

    #region TryAddIntercepted<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_with_interceptor_types_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted<TestClassService>(lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddIntercepted<TestClassService>(lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(new SecondTestInterceptor());
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddIntercepted with generic interceptor type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_with_generic_interceptor_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted<TestClassService, TestInterceptor>(lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddIntercepted<TestClassService, SecondTestInterceptor>(lifetime);
      _ = _services.AddSingleton(new SecondTestInterceptor());
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService> with factory

    /// <summary>
    /// Tests that TryAddIntercepted with factory does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_with_factory_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestClassService originalInstance = new();

      _ = _services.AddIntercepted<TestClassService>(sp => originalInstance, lifetime, _interceptor);

      // Act
      TestClassService newInstance = new();
      _ = _services.TryAddIntercepted<TestClassService>(sp => newInstance, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
    }

    #endregion

    #region TryAddIntercepted<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddIntercepted with service and implementation does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_service_implementation_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted<ITestService, TestService>(lifetime, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<ITestService, TestService>(lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    /// <summary>
    /// Tests that TryAddIntercepted with service and implementation registers when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_TryAddIntercepted_service_implementation_registers_service()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddIntercepted<ITestService, TestService>(lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    #endregion

    #region TryAddIntercepted<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddIntercepted with service, implementation, and interceptor does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_service_implementation_interceptor_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted<ITestService, TestService, TestInterceptor>(lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<ITestService, TestService, SecondTestInterceptor>(lifetime);
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService, TImplementation> with factory

    /// <summary>
    /// Tests that TryAddIntercepted with service, implementation factory does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_service_implementation_factory_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestService originalInstance = new();

      _ = _services.AddIntercepted<ITestService, TestService>(sp => originalInstance, lifetime, _interceptor);

      // Act
      TestService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<ITestService, TestService>(sp => newInstance, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_with_interceptor_types_single_type_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted<TestClassService>(lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<TestClassService>(lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with factory and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_factory_interceptor_types_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestClassService originalInstance = new();

      _ = _services.AddIntercepted<TestClassService>(sp => originalInstance, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestClassService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<TestClassService>(sp => newInstance, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddIntercepted with factory and generic interceptor type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_factory_generic_interceptor_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestClassService originalInstance = new();

      _ = _services.AddIntercepted<TestClassService, TestInterceptor>(sp => originalInstance, lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestClassService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<TestClassService, SecondTestInterceptor>(sp => newInstance, lifetime);
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with service, implementation, and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_service_implementation_interceptor_types_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted<ITestService, TestService>(lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<ITestService, TestService>(lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService, TImplementation> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with service, implementation factory, and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_service_implementation_factory_interceptor_types_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestService originalInstance = new();

      _ = _services.AddIntercepted<ITestService, TestService>(sp => originalInstance, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<ITestService, TestService>(sp => newInstance, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted<TService, TImplementation, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddIntercepted with service, implementation factory, and generic interceptor does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_service_implementation_factory_generic_interceptor_does_not_replace()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestService originalInstance = new();

      _ = _services.AddIntercepted<ITestService, TestService, TestInterceptor>(sp => originalInstance, lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted<ITestService, TestService, SecondTestInterceptor>(sp => newInstance, lifetime);
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion
  }
}
