// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddInterceptedSingleton.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for TryAddInterceptedSingleton extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddInterceptedSingleton(Type) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with Type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_type_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      _ = _services.AddSingleton(existingInstance);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance, not the intercepted one");
      Assert.IsFalse(_interceptor.WasInvoked, "Interceptor should not be invoked since existing registration was used");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with Type and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with interceptor types does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_type_interceptor_types_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      _ = _services.AddSingleton(existingInstance);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type) with instance and IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with existing instance registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_instance_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService instance = new();

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, instance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with instance does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_type_instance_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      TestClassService newInstance = new();
      _ = _services.AddSingleton(existingInstance);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, newInstance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreNotSame(newInstance, service, "Should not use the new instance");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type) with instance and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with instance and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_instance_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService instance = new();
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, instance, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with Type and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_implementation_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with implementation does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_type_implementation_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      TestService existingInstance = new();
      _ = _services.AddSingleton<ITestService>(existingInstance);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with Type, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with Type and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      int factoryCallCount = 0;
      Func<IServiceProvider, object> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with factory does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_type_factory_does_not_add_duplicate()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService existingInstance = new();
      _ = _services.AddSingleton(existingInstance);
      int factoryCallCount = 0;
      Func<IServiceProvider, object> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreEqual(0, factoryCallCount, "Factory should not be called when service already registered");
    }

    #endregion

    #region TryAddInterceptedSingleton(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with Type, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_type_factory_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      int factoryCallCount = 0;
      Func<IServiceProvider, object> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(serviceType, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddInterceptedSingleton_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddInterceptedSingleton<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_generic_TryAddInterceptedSingleton_does_not_add_duplicate()
    {
      // Arrange
      TestClassService existingInstance = new();
      _ = _services.AddSingleton(existingInstance);

      // Act
      _ = _services.TryAddInterceptedSingleton<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddInterceptedSingleton_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<TestClassService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<TestClassService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService> with factory

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddInterceptedSingleton(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with factory does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_generic_factory_does_not_add_duplicate()
    {
      // Arrange
      TestClassService existingInstance = new();
      _ = _services.AddSingleton(existingInstance);
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddInterceptedSingleton(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreEqual(0, factoryCallCount, "Factory should not be called when service already registered");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with factory and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_factory_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with factory and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_factory_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<TestClassService, TestInterceptor>(factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService> with instance and IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with instance registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_instance_registers_with_singleton_lifetime()
    {
      // Arrange
      TestClassService instance = new();

      // Act
      _ = _services.TryAddInterceptedSingleton(instance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with instance does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_generic_instance_does_not_add_duplicate()
    {
      // Arrange
      TestClassService existingInstance = new();
      TestClassService newInstance = new();
      _ = _services.AddSingleton(existingInstance);

      // Act
      _ = _services.TryAddInterceptedSingleton(newInstance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreNotSame(newInstance, service, "Should not use the new instance");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService> with instance and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with instance and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_instance_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      TestClassService instance = new();
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton(instance, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TInterceptor> with instance

    /// <summary>
    /// Tests that TryAddInterceptedSingleton generic with instance and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_generic_instance_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      TestClassService instance = new();
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<TestClassService, TestInterceptor>(instance);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_service_implementation_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service and implementation does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_service_implementation_does_not_add_duplicate()
    {
      // Arrange
      TestService existingInstance = new();
      _ = _services.AddSingleton<ITestService>(existingInstance);

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_service_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service, implementation, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_service_implementation_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TImplementation> with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service, implementation, and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_service_implementation_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestService> factory = sp =>
      {
        factoryCallCount++;
        return new TestService();
      };

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService>(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service, implementation, and factory does not add duplicate registrations.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedSingleton_service_implementation_factory_does_not_add_duplicate()
    {
      // Arrange
      TestService existingInstance = new();
      _ = _services.AddSingleton<ITestService>(existingInstance);
      int factoryCallCount = 0;
      Func<IServiceProvider, TestService> factory = sp =>
      {
        factoryCallCount++;
        return new TestService();
      };

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService>(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.AreSame(existingInstance, service, "Should return the first registered instance");
      Assert.AreEqual(0, factoryCallCount, "Factory should not be called when service already registered");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TImplementation> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service, implementation, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_service_implementation_factory_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestService> factory = sp =>
      {
        factoryCallCount++;
        return new TestService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService>(factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion

    #region TryAddInterceptedSingleton<TService, TImplementation, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddInterceptedSingleton with service, implementation, factory, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedSingleton_service_implementation_factory_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestService> factory = sp =>
      {
        factoryCallCount++;
        return new TestService();
      };
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedSingleton<ITestService, TestService, TestInterceptor>(factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
      Assert.AreEqual(1, factoryCallCount, "Factory should be called only once for singleton");
    }

    #endregion
  }
}
