// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddKeyedIntercepted.Type.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for non-generic TryAddKeyedIntercepted methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddKeyedIntercepted(Type serviceType) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_TryAddKeyedIntercepted_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_TryAddKeyedIntercepted_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddKeyedIntercepted(serviceType!, serviceKey, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type registers service when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_TryAddKeyedIntercepted_type_registers_service()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_type_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted(serviceType, serviceKey, lifetime, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted(serviceType, serviceKey, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedIntercepted(Type serviceType, factory) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_TryAddKeyedIntercepted_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddKeyedIntercepted(serviceType, serviceKey, factory, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and factory throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_TryAddKeyedIntercepted_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      object? serviceKey = "test-key";
      Func<IServiceProvider, object?, object> factory = (sp, key) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddKeyedIntercepted(serviceType!, serviceKey, factory, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_TryAddKeyedIntercepted_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      Func<IServiceProvider, object?, object>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddKeyedIntercepted(serviceType, serviceKey, factory!, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and factory does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_type_factory_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      TestClassService originalInstance = new();
      Func<IServiceProvider, object?, object> originalFactory = (sp, key) => originalInstance;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted(serviceType, serviceKey, originalFactory, lifetime, _interceptor);

      // Act
      TestClassService newInstance = new();
      Func<IServiceProvider, object?, object> newFactory = (sp, key) => newInstance;
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted(serviceType, serviceKey, newFactory, lifetime, secondInterceptor);
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

    #region TryAddKeyedIntercepted(Type serviceType, Type implementationType) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and implementationType throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_TryAddKeyedIntercepted_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      object? serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and implementationType throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_TryAddKeyedIntercepted_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      object? serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddKeyedIntercepted(serviceType!, serviceKey, implementationType, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and implementationType throws ArgumentNullException when implementationType is null.
    /// </summary>
    [TestMethod]
    public void When_implementationType_is_null_TryAddKeyedIntercepted_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object? serviceKey = "test-key";
      Type? implementationType = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType!, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and implementationType registers service when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_TryAddKeyedIntercepted_type_implementation_registers_service()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object? serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted with Type and implementationType does not register when already registered with same key.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedIntercepted_type_implementation_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object? serviceKey = "test-key";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedIntercepted(serviceType, serviceKey, implementationType, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    /// <summary>
    /// Tests that TryAddKeyedIntercepted allows different keys for same service type.
    /// </summary>
    [TestMethod]
    public void When_different_keys_used_TryAddKeyedIntercepted_type_implementation_registers_both()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      object? serviceKey1 = "key-1";
      object? serviceKey2 = "key-2";
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      _ = _services.AddKeyedIntercepted(serviceType, serviceKey1, implementationType, lifetime, _interceptor);

      // Act
      _ = _services.TryAddKeyedIntercepted(serviceType, serviceKey2, implementationType, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey1);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");

      _ = service1.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");

      _ = service2.GetName();
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
    }

    #endregion
  }
}
