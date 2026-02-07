// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddKeyedInterceptedSingleton.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddKeyedInterceptedSingleton extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    private const string TestKey = "test-key";

    #region AddKeyedInterceptedSingleton(Type) with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with Type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_type_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddKeyedInterceptedSingleton_type_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddKeyedInterceptedSingleton(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with Type and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_type_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with Type and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_type_implementation_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with Type, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_type_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, implementationType, interceptorTypes:  typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with Type and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_type_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, object> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestClassService();
      };

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with Type, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_type_factory_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, object> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(serviceType, TestKey, factory, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService> with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton generic registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedInterceptedSingleton_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.AddKeyedInterceptedSingleton<TestClassService>(TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton generic with interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedInterceptedSingleton_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<TestClassService>(TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TInterceptor>

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton generic with interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_generic_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<TestClassService, TestInterceptor>(TestKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton generic with factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_generic_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestClassService();
      };

      // Act
      _ = _services.AddKeyedInterceptedSingleton(TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton generic with factory and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_generic_factory_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton(TestKey, factory, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton generic with factory and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_generic_factory_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<TestClassService, TestInterceptor>(TestKey, factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with service and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_service_implementation_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.AddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with service, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_service_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with service, implementation, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_service_implementation_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<ITestService, TestService, TestInterceptor>(TestKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TImplementation> with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with service, implementation, and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_service_implementation_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestService> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestService();
      };

      // Act
      _ = _services.AddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TImplementation> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with service, implementation, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_service_implementation_factory_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestService> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, factory, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region AddKeyedInterceptedSingleton<TService, TImplementation, TInterceptor> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedSingleton with service, implementation, factory, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedSingleton_service_implementation_factory_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestService> factory = (sp, key) =>
      {
        factoryCallCount++;
        Assert.AreEqual(TestKey, key, "Factory should receive the correct service key");
        return new TestService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedSingleton<ITestService, TestService, TestInterceptor>(TestKey, factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region Keyed Service Key Isolation Tests

    /// <summary>
    /// Tests that services with different keys are isolated.
    /// </summary>
    [TestMethod]
    public void When_different_keys_used_AddKeyedInterceptedSingleton_services_are_isolated()
    {
      // Arrange
      const string key1 = "key1";
      const string key2 = "key2";

      // Act
      _ = _services.AddKeyedInterceptedSingleton<TestClassService>(key1, _interceptor);
      _ = _services.AddKeyedInterceptedSingleton<TestClassService>(key2, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetKeyedService<TestClassService>(key1);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(key2);

      // Assert
      Assert.IsNotNull(service1, "Service for key1 should be registered");
      Assert.IsNotNull(service2, "Service for key2 should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    /// <summary>
    /// Tests that null key is supported.
    /// </summary>
    [TestMethod]
    public void When_null_key_used_AddKeyedInterceptedSingleton_registers_successfully()
    {
      // Arrange & Act
      _ = _services.AddKeyedInterceptedSingleton<TestClassService>(null, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service = provider.GetKeyedService<TestClassService>(null);

      // Assert
      Assert.IsNotNull(service, "Service with null key should be registered");
    }

    #endregion
  }
}
