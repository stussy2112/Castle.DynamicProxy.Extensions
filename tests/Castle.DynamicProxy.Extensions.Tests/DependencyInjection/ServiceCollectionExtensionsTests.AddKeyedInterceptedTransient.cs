// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddKeyedInterceptedTransient.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddKeyedInterceptedTransient extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region AddKeyedInterceptedTransient(Type) with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with Type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_type_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddKeyedInterceptedTransient_type_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddKeyedInterceptedTransient(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with Type and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_type_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with Type and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_type_implementation_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with Type, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_type_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with Type and factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_type_factory_registers_with_transient_lifetime()
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
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, factory, _interceptor);
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

    #region AddKeyedInterceptedTransient(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with Type, factory, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_type_factory_interceptor_types_registers_with_transient_lifetime()
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
      _ = _services.AddKeyedInterceptedTransient(serviceType, serviceKey, factory, typeof(TestInterceptor));
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

    #region AddKeyedInterceptedTransient<TService> with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient generic registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedInterceptedTransient_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedTransient<TestClassService>(serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient generic with interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedInterceptedTransient_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedTransient<TestClassService>(serviceKey, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient<TService, TInterceptor>

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient generic with interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_generic_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedTransient<TestClassService, TestInterceptor>(serviceKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient<TService> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient generic with factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_generic_factory_registers_with_transient_lifetime()
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
      _ = _services.AddKeyedInterceptedTransient(serviceKey, factory, _interceptor);
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

    #region AddKeyedInterceptedTransient<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient generic with factory and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_generic_factory_interceptor_types_registers_with_transient_lifetime()
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
      _ = _services.AddKeyedInterceptedTransient(serviceKey, factory, typeof(TestInterceptor));
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

    #region AddKeyedInterceptedTransient<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient generic with factory and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_generic_factory_interceptor_type_registers_with_transient_lifetime()
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
      _ = _services.AddKeyedInterceptedTransient<TestClassService, TestInterceptor>(serviceKey, factory);
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

    #region AddKeyedInterceptedTransient<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with service and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_service_implementation_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedTransient<ITestService, TestService>(serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with service, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_service_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedTransient<ITestService, TestService>(serviceKey, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient with service, implementation, and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedTransient_service_implementation_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedTransient<ITestService, TestService, TestInterceptor>(serviceKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddKeyedInterceptedTransient with different keys

    /// <summary>
    /// Tests that AddKeyedInterceptedTransient allows different keys for same service type.
    /// </summary>
    [TestMethod]
    public void When_different_keys_used_AddKeyedInterceptedTransient_registers_separate_services()
    {
      // Arrange
      object? serviceKey1 = "key-1";
      object? serviceKey2 = "key-2";

      // Act
      _ = _services.AddKeyedInterceptedTransient<TestClassService>(serviceKey1, _interceptor);
      _ = _services.AddKeyedInterceptedTransient<TestClassService>(serviceKey2, _interceptor);
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
