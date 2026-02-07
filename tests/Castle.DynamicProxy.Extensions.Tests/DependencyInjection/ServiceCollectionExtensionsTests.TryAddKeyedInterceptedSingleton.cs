// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddKeyedInterceptedSingleton.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for TryAddKeyedInterceptedSingleton extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddKeyedInterceptedSingleton(Type) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with Type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_type_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_type_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      _ = _services.AddKeyedSingleton(serviceType, TestKey, existingInstance);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance, not the intercepted one");
      Assert.IsFalse(_interceptor.WasInvoked, "Interceptor should not be invoked since existing registration was used");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with Type and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_type_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with interceptor types does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_type_interceptor_types_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      _ = _services.AddKeyedSingleton(serviceType, TestKey, existingInstance);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with Type and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_type_implementation_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with implementation does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_type_implementation_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      TestService existingInstance = new();
      _ = _services.AddKeyedSingleton<ITestService>(TestKey, existingInstance);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with Type, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_type_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, implementationType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      
      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with Type and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_type_factory_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with factory does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_type_factory_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      _ = _services.AddKeyedSingleton(serviceType, TestKey, existingInstance);
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, object> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreEqual(0, factoryCallCount, "Factory should not be called when service already registered");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with Type, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_type_factory_interceptor_types_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton(serviceType, TestKey, factory,  interceptorTypes: typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedSingleton<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddKeyedInterceptedSingleton_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService>(TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_generic_TryAddKeyedInterceptedSingleton_does_not_add_duplicate()
    {
      // Arrange
      TestClassService existingInstance = new();
      _ = _services.AddKeyedSingleton(TestKey, existingInstance);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService>(TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic with interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddKeyedInterceptedSingleton_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService>(TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic with interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_generic_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService, TestInterceptor>(TestKey);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic with factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_generic_factory_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton(TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service1 = provider.GetKeyedService<TestClassService>(TestKey);
      TestClassService? service2 = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic with factory does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_generic_factory_does_not_add_duplicate()
    {
      // Arrange
      TestClassService existingInstance = new();
      _ = _services.AddKeyedSingleton(TestKey, existingInstance);
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestClassService> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton(TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreEqual(0, factoryCallCount, "Factory should not be called when service already registered");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic with factory and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_generic_factory_interceptor_types_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton(TestKey, factory, interceptorTypes: typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedSingleton<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton generic with factory and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_generic_factory_interceptor_type_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService, TestInterceptor>(TestKey, factory);
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

    #region TryAddKeyedInterceptedSingleton<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_service_implementation_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service and implementation does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_service_implementation_does_not_add_duplicate()
    {
      // Arrange
      TestService existingInstance = new();
      _ = _services.AddKeyedSingleton<ITestService>(TestKey, existingInstance);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_service_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      
      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service, implementation, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_service_implementation_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService, TestInterceptor>(TestKey);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService, TImplementation> with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service, implementation, and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_service_implementation_factory_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      ITestService? service1 = provider.GetKeyedService<ITestService>(TestKey);
      ITestService? service2 = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service, implementation, and factory does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedSingleton_service_implementation_factory_does_not_add_duplicate()
    {
      // Arrange
      TestService existingInstance = new();
      _ = _services.AddKeyedSingleton<ITestService>(TestKey, existingInstance);
      int factoryCallCount = 0;
      Func<IServiceProvider, object?, TestService> factory = (sp, key) =>
      {
        factoryCallCount++;
        return new TestService();
      };

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetKeyedService<ITestService>(TestKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreEqual(0, factoryCallCount, "Factory should not be called when service already registered");
    }

    #endregion

    #region TryAddKeyedInterceptedSingleton<TService, TImplementation> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service, implementation, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_service_implementation_factory_interceptor_types_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService>(TestKey, factory, interceptorTypes: typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedSingleton<TService, TImplementation, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedSingleton with service, implementation, factory, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedSingleton_service_implementation_factory_interceptor_type_registers_with_singleton_lifetime()
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
      _ = _services.TryAddKeyedInterceptedSingleton<ITestService, TestService, TestInterceptor>(TestKey, factory);
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
    public void When_different_keys_used_TryAddKeyedInterceptedSingleton_services_are_isolated()
    {
      // Arrange
      const string key1 = "key1";
      const string key2 = "key2";

      // Act
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService>(key1, _interceptor);
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService>(key2, _interceptor);
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
    public void When_null_key_used_TryAddKeyedInterceptedSingleton_registers_successfully()
    {
      // Arrange & Act
      _ = _services.TryAddKeyedInterceptedSingleton<TestClassService>(null, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      
      TestClassService? service = provider.GetKeyedService<TestClassService>(null);

      // Assert
      Assert.IsNotNull(service, "Service with null key should be registered");
    }

    #endregion
  }
}
