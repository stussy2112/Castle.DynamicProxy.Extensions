// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddKeyedIntercepted.Generic.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for generic AddKeyedIntercepted methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddKeyedIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_generic_AddKeyedIntercepted_single_type_registers_service()
    {
      // Arrange
      object? serviceKey = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method with no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddKeyedIntercepted_single_type_registers_service_without_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestClassService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method creates intercepted proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddKeyedIntercepted_single_type_creates_intercepted_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsFalse(_interceptor!.WasInvoked, "Interceptor should not be invoked yet");

      string result = service.GetName();

      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_generic_AddKeyedIntercepted_single_type_registers_separate_services()
    {
      // Arrange
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey1, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted<TestClassService>(serviceKey2, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey1);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddKeyedIntercepted_single_type_returns_same_instance()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddKeyedIntercepted_single_type_returns_different_instances()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddKeyedIntercepted_single_type_applies_in_order()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type method returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedIntercepted_single_type_returns_service_collection_for_chaining()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted<TestClassService>(serviceKey, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddKeyedIntercepted_single_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_generic_AddKeyedIntercepted_single_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted(serviceKey, factory!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_generic_AddKeyedIntercepted_single_type_factory_registers_service()
    {
      // Arrange
      object? serviceKey = null;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method with no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddKeyedIntercepted_single_type_factory_registers_service_without_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestClassService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method creates intercepted proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddKeyedIntercepted_single_type_factory_creates_intercepted_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsFalse(_interceptor!.WasInvoked, "Interceptor should not be invoked yet");

      string result = service.GetName();

      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_generic_AddKeyedIntercepted_single_type_factory_registers_separate_services()
    {
      // Arrange
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      Func<IServiceProvider, object?, TestClassService> factory1 = (sp, key) => new TestClassService();
      Func<IServiceProvider, object?, TestClassService> factory2 = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey1, factory1, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted(serviceKey2, factory2, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey1);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddKeyedIntercepted_single_type_factory_returns_same_instance()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddKeyedIntercepted_single_type_factory_returns_different_instances()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method passes service key to factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_generic_AddKeyedIntercepted_single_type_factory_passes_service_key_to_factory()
    {
      // Arrange
      object serviceKey = "test-key";
      object? capturedKey = null;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        capturedKey = key;
        return new TestClassService();
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreEqual(serviceKey, capturedKey, "Factory should receive the service key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddKeyedIntercepted_single_type_factory_applies_in_order()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedIntercepted_single_type_factory_returns_service_collection_for_chaining()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic single-type factory method can inject dependencies via factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_generic_AddKeyedIntercepted_single_type_factory_injects_dependencies_via_factory()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, ServiceWithDependency> factory = (sp, key) =>
      {
        IDependency dependency = sp.GetRequiredService<IDependency>();
        return new ServiceWithDependency(dependency);
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddKeyedIntercepted(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ServiceWithDependency? service = provider.GetKeyedService<ServiceWithDependency>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddKeyedIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_generic_AddKeyedIntercepted_with_type_registers_service()
    {
      // Arrange
      object? serviceKey = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddKeyedIntercepted_with_type_registers_service_without_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddKeyedIntercepted_with_type_creates_intercepted_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsFalse(_interceptor!.WasInvoked, "Interceptor should not be invoked yet");

      string result = service.GetName();

      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_generic_AddKeyedIntercepted_with_type_registers_separate_services()
    {
      // Arrange
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey1, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted<ITestService, TestService>(serviceKey2, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey1);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddKeyedIntercepted_with_type_returns_same_instance()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddKeyedIntercepted_with_type_returns_different_instances()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddKeyedIntercepted_with_type_applies_in_order()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedIntercepted_with_type_returns_service_collection_for_chaining()
    {
      // Arrange
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with type uses ActivatorUtilities to create instances with dependencies.
    /// </summary>
    [TestMethod]
    public void When_activator_utilities_used_generic_AddKeyedIntercepted_with_type_injects_dependencies()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddKeyedIntercepted<IServiceWithDependency, ServiceWithDependency>(serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      IServiceWithDependency? service = provider.GetKeyedService<IServiceWithDependency>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddKeyedIntercepted_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_generic_AddKeyedIntercepted_factory_throws_ArgumentNullException()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_generic_AddKeyedIntercepted_factory_registers_service()
    {
      // Arrange
      object? serviceKey = null;
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddKeyedIntercepted_factory_registers_service_without_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddKeyedIntercepted_factory_creates_intercepted_proxy()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsFalse(_interceptor!.WasInvoked, "Interceptor should not be invoked yet");

      string result = service.GetName();

      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_generic_AddKeyedIntercepted_factory_registers_separate_services()
    {
      // Arrange
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      Func<IServiceProvider, object?, TestService> factory1 = (sp, key) => new TestService();
      Func<IServiceProvider, object?, TestService> factory2 = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey1, factory1, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted<ITestService, TestService>(serviceKey2, factory2, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey1);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddKeyedIntercepted_factory_returns_same_instance()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddKeyedIntercepted_factory_returns_different_instances()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory passes service key to factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_generic_AddKeyedIntercepted_factory_passes_service_key_to_factory()
    {
      // Arrange
      object serviceKey = "test-key";
      object? capturedKey = null;
      Func<IServiceProvider, object?, TestService> factory = (sp, key) =>
      {
        capturedKey = key;
        return new TestService();
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreEqual(serviceKey, capturedKey, "Factory should receive the service key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddKeyedIntercepted_factory_applies_in_order()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedIntercepted_factory_returns_service_collection_for_chaining()
    {
      // Arrange
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, TestService> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted<ITestService, TestService>(serviceKey, factory, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted generic method with factory can inject dependencies via factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_generic_AddKeyedIntercepted_factory_injects_dependencies_via_factory()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, ServiceWithDependency> factory = (sp, key) =>
      {
        IDependency dependency = sp.GetRequiredService<IDependency>();
        return new ServiceWithDependency(dependency);
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddKeyedIntercepted<IServiceWithDependency, ServiceWithDependency>(serviceKey, factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      IServiceWithDependency? service = provider.GetKeyedService<IServiceWithDependency>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }
  }
}
