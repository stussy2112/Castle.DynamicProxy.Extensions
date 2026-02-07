// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddKeyedIntercepted.Type.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddKeyedIntercepted with implementation type overload.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddKeyedIntercepted with single type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddKeyedIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddKeyedIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted(serviceType!, serviceKey, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_AddKeyedIntercepted_single_type_registers_service()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddKeyedIntercepted_single_type_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestClassService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type creates intercepted proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_AddKeyedIntercepted_single_type_creates_intercepted_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!);
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
    /// Tests that AddKeyedIntercepted with single type distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_AddKeyedIntercepted_single_type_registers_separate_services()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey1, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted(serviceType, serviceKey2, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey1);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddKeyedIntercepted_single_type_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddKeyedIntercepted_single_type_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with single type applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_AddKeyedIntercepted_single_type_applies_in_order()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!, secondInterceptor);
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
    /// Tests that AddKeyedIntercepted with single type returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedIntercepted_single_type_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddKeyedIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddKeyedIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted(serviceType!, serviceKey, implementationType, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type throws ArgumentNullException when implementationType is null.
    /// </summary>
    [TestMethod]
    public void When_implementationType_is_null_AddKeyedIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type? implementationType = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_AddKeyedIntercepted_with_type_registers_service()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object? serviceKey = null;
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddKeyedIntercepted_with_type_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, interceptors);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_AddKeyedIntercepted_with_type_creates_intercepted_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddKeyedIntercepted with type distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_AddKeyedIntercepted_with_type_registers_separate_services()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey1, implementationType, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted(serviceType, serviceKey2, implementationType, lifetime, _interceptor);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey1);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddKeyedIntercepted_with_type_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddKeyedIntercepted_with_type_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedIntercepted_with_type_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with type uses ActivatorUtilities to create instances with dependencies.
    /// </summary>
    [TestMethod]
    public void When_activator_utilities_used_AddKeyedIntercepted_with_type_injects_dependencies()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      Type serviceType = typeof(IServiceWithDependency);
      object serviceKey = "test-key";
      Type implementationType = typeof(ServiceWithDependency);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      IServiceWithDependency? service = provider.GetKeyedService<IServiceWithDependency>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }
  }
}