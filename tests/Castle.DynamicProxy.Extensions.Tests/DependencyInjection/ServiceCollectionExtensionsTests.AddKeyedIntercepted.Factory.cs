// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddKeyedIntercepted.Factory.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddKeyedIntercepted with factory overload.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddKeyedIntercepted with factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddKeyedIntercepted_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddKeyedIntercepted_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted(serviceType!, serviceKey, factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_AddKeyedIntercepted_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddKeyedIntercepted(serviceType, serviceKey, factory!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory accepts null service key.
    /// </summary>
    [TestMethod]
    public void When_serviceKey_is_null_AddKeyedIntercepted_registers_service()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object? serviceKey = null;
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered with null key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddKeyedIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, interceptors);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_AddKeyedIntercepted_creates_intercepted_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!);
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
    /// Tests that AddKeyedIntercepted with factory distinguishes services by key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_provided_AddKeyedIntercepted_registers_separate_services()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey1 = "key1";
      object serviceKey2 = "key2";
      Func<IServiceProvider, object?, object> factory1 = (sp, key) => new TestService();
      Func<IServiceProvider, object?, object> factory2 = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey1, factory1, lifetime, _interceptor!);
      _ = _services.AddKeyedIntercepted(serviceType, serviceKey2, factory2, lifetime, _interceptor);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey1);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "Service with key1 should be registered");
      Assert.IsNotNull(service2, "Service with key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddKeyedIntercepted_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddKeyedIntercepted_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory passes service key to factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_AddKeyedIntercepted_passes_service_key_to_factory()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      object? capturedKey = null;
      Func<IServiceProvider, object?, object> factory = (sp, key) =>
      {
        capturedKey = key;
        return new TestService();
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreEqual(serviceKey, capturedKey, "Factory should receive the service key");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted with factory returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedIntercepted_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddKeyedIntercepted returns correct instance for each service key when multiple services of same type are registered with same interceptor type.
    /// </summary>
    [TestMethod]
    public void When_multiple_keyed_services_registered_AddKeyedIntercepted_returns_correct_instance_by_key()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object serviceKey1 = "primary-service";
      object serviceKey2 = "secondary-service";
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Create two distinct TestService instances that can be identified
      TestService primaryInstance = new ("First Instance");
      TestService secondaryInstance = new ("Second Instance");

      // Create factories that return the specific instances
      Func<IServiceProvider, object?, object> factory1 = (sp, key) => primaryInstance;
      Func<IServiceProvider, object?, object> factory2 = (sp, key) => secondaryInstance;

      // Use the same interceptor for both
      TestInterceptor sharedInterceptor = new ();

      // Act - Register both keyed services with the same interceptor
      _ = _services!.AddKeyedIntercepted(serviceType, serviceKey1, factory1, lifetime, sharedInterceptor);
      _ = _services.AddKeyedIntercepted(serviceType, serviceKey2, factory2, lifetime, sharedInterceptor);
      ServiceProvider provider = _services.BuildServiceProvider();

      // Retrieve services by their respective keys
      ITestService? retrievedPrimaryService = provider.GetKeyedService<ITestService>(serviceKey1);
      ITestService? retrievedSecondaryService = provider.GetKeyedService<ITestService>(serviceKey2);

      // Assert - Verify both services are registered
      Assert.IsNotNull(retrievedPrimaryService, "Primary service should be registered");
      Assert.IsNotNull(retrievedSecondaryService, "Secondary service should be registered");
      Assert.AreNotSame(retrievedPrimaryService, retrievedSecondaryService, "Services with different keys should be different instances");
      Assert.AreEqual("First Instance", retrievedPrimaryService.Tag);
      Assert.AreEqual("Second Instance", retrievedSecondaryService.Tag);

      // Assert - Verify each service returns correct instance by calling GetName and checking which underlying instance was called
      Assert.IsFalse(primaryInstance.WasCalled, "Primary instance should not be called yet");
      Assert.IsFalse(secondaryInstance.WasCalled, "Secondary instance should not be called yet");

      string result1 = retrievedPrimaryService.GetName();
      Assert.IsTrue(primaryInstance.WasCalled, "Primary instance should be called when retrieving primary keyed service");
      Assert.IsFalse(secondaryInstance.WasCalled, "Secondary instance should not be called when retrieving primary keyed service");
      Assert.AreEqual("TestService", result1, "Primary service should return correct value");

      string result2 = retrievedSecondaryService.GetName();
      Assert.IsTrue(secondaryInstance.WasCalled, "Secondary instance should be called when retrieving secondary keyed service");
      Assert.AreEqual("TestService", result2, "Secondary service should return correct value");

      // Assert - Verify interceptor was invoked for both services
      Assert.IsTrue(sharedInterceptor.WasInvoked, "Shared interceptor should be invoked for both services");
      Assert.AreEqual("GetName", sharedInterceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
    }
  }
}