// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddKeyedInterceptedTransient.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for TryAddKeyedInterceptedTransient extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddKeyedInterceptedTransient(Type) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_type_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedTransient_type_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_type_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_type_implementation_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_type_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type and factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_type_factory_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, object> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with Type, factory, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_type_factory_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, object> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceType, serviceKey, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient generic registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddKeyedInterceptedTransient_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<TestClassService>(serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient generic with interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddKeyedInterceptedTransient_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<TestClassService>(serviceKey, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient generic with interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_generic_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<TestClassService, TestInterceptor>(serviceKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient generic with factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_generic_factory_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient generic with factory and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_generic_factory_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient(serviceKey, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient generic with factory and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_generic_factory_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<TestClassService, TestInterceptor>(serviceKey, factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with service and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_service_implementation_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<ITestService, TestService>(serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with service, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_service_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<ITestService, TestService>(serviceKey, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient with service, implementation, and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedTransient_service_implementation_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<ITestService, TestService, TestInterceptor>(serviceKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddKeyedInterceptedTransient with different keys

    /// <summary>
    /// Tests that TryAddKeyedInterceptedTransient allows different keys but respects Try semantics per key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_used_TryAddKeyedInterceptedTransient_registers_separate_services()
    {
      // Arrange
      object? serviceKey1 = "key-1";
      object? serviceKey2 = "key-2";

      // Act
      _ = _services.TryAddKeyedInterceptedTransient<TestClassService>(serviceKey1, _interceptor);
      _ = _services.TryAddKeyedInterceptedTransient<TestClassService>(serviceKey2, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey1);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    #endregion
  }
}
