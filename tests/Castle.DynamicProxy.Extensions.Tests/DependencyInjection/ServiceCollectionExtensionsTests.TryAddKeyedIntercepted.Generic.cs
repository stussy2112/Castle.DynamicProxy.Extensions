// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddKeyedIntercepted.Generic.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for generic TryAddKeyedIntercepted methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddKeyedIntercepted<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_TryAddKeyedIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddKeyedIntercepted<TestService>(serviceKey, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method registers service when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_generic_TryAddKeyedIntercepted_single_type_registers_service()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method does not register service when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_with_same_key_generic_TryAddKeyedIntercepted_single_type_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      _ = _services.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor);

      // Act
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked - service not replaced");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method registers service with different key.
    /// </summary>
    [TestMethod]
    public void When_service_registered_with_different_key_generic_TryAddKeyedIntercepted_single_type_registers_with_new_key()
    {
      // Arrange
      object? serviceKey1 = "key-1";
      object? serviceKey2 = "key-2";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      _ = _services.AddKeyedIntercepted<TestClassService>(serviceKey1, lifetime, _interceptor);

      // Act
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey2, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey1);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");

      _ = service1.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");

      _ = service2.GetName();
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method works with null key.
    /// </summary>
    [TestMethod]
    public void When_null_key_provided_generic_TryAddKeyedIntercepted_single_type_registers_with_null_key()
    {
      // Arrange
      object? serviceKey = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_TryAddKeyedIntercepted_single_type_returns_same_instance()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted generic single-type method respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_TryAddKeyedIntercepted_single_type_returns_different_instances()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service and implementation does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_service_implementation_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service and implementation registers when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_TryAddKeyedIntercepted_service_implementation_registers_service()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService> with factory

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with factory does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_with_factory_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestClassService originalInstance = new();

      _ = _services.AddKeyedIntercepted<TestClassService>(serviceKey, (sp, key) => originalInstance, lifetime, _interceptor);

      // Act
      TestClassService newInstance = new();
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, (sp, key) => newInstance, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service, implementation, and interceptor does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_service_implementation_interceptor_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted<ITestService, TestService, TestInterceptor>(serviceKey, lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService, SecondTestInterceptor>(serviceKey, lifetime);
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with interceptor types does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_interceptor_types_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service, implementation, and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_service_implementation_interceptor_types_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with factory and interceptor types does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_factory_interceptor_types_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestClassService originalInstance = new();

      _ = _services.AddKeyedIntercepted<TestClassService>(serviceKey, (sp, key) => originalInstance, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestClassService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<TestClassService>(serviceKey, (sp, key) => newInstance, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with factory and generic interceptor type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_factory_generic_interceptor_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestClassService originalInstance = new();

      _ = _services.AddKeyedIntercepted<TestClassService, TestInterceptor>(serviceKey, (sp, key) => originalInstance, lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestClassService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<TestClassService, SecondTestInterceptor>(serviceKey, (sp, key) => newInstance, lifetime);
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TImplementation> with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service, implementation factory does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_service_implementation_factory_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestService originalInstance = new();

      _ = _services.AddKeyedIntercepted<ITestService, TestService>(serviceKey, (sp, key) => originalInstance, lifetime, _interceptor);

      // Act
      TestService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService>(serviceKey, (sp, key) => newInstance, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TImplementation> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service, implementation factory, and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_service_implementation_factory_interceptor_types_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestService originalInstance = new();

      _ = _services.AddKeyedIntercepted<ITestService, TestService>(serviceKey, (sp, key) => originalInstance, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService>(serviceKey, (sp, key) => newInstance, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted<TService, TImplementation, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with service, implementation factory, and generic interceptor does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_service_implementation_factory_generic_interceptor_does_not_replace()
    {
      // Arrange
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      TestService originalInstance = new();

      _ = _services.AddKeyedIntercepted<ITestService, TestService, TestInterceptor>(serviceKey, (sp, key) => originalInstance, lifetime);
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestService newInstance = new();
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted<ITestService, TestService, SecondTestInterceptor>(serviceKey, (sp, key) => newInstance, lifetime);
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

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
