// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddInterceptedSingleton.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddInterceptedSingleton extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region AddInterceptedSingleton(Type) with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton with Type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that AddInterceptedSingleton applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddInterceptedSingleton_type_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddInterceptedSingleton(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton with Type and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton(Type) with instance and IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton with existing instance registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_instance_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService instance = new();

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, instance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    /// <summary>
    /// Tests that AddInterceptedSingleton with existing instance applies interceptors to the instance.
    /// </summary>
    [TestMethod]
    public void When_instance_provided_AddInterceptedSingleton_type_applies_interceptors_to_instance()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService instance = new();

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, instance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddInterceptedSingleton(Type) with instance and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton with instance and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_instance_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService instance = new();
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, instance, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton with Type and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_implementation_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton with Type, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton with Type and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_factory_registers_with_singleton_lifetime()
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
      _ = _services.AddInterceptedSingleton(serviceType, factory, _interceptor);
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

    #region AddInterceptedSingleton(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton with Type, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_type_factory_interceptor_types_registers_with_singleton_lifetime()
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
      _ = _services.AddInterceptedSingleton(serviceType, factory, typeof(TestInterceptor));
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

    #region AddInterceptedSingleton<TService> with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton generic registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedSingleton_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.AddInterceptedSingleton<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedSingleton_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton<TestClassService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService, TInterceptor>

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton<TestClassService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService> with factory

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.AddInterceptedSingleton(factory, _interceptor);
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

    #region AddInterceptedSingleton<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with factory and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_factory_interceptor_types_registers_with_singleton_lifetime()
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
      _ = _services.AddInterceptedSingleton(factory, typeof(TestInterceptor));
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

    #region AddInterceptedSingleton<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with factory and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_factory_interceptor_type_registers_with_singleton_lifetime()
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
      _ = _services.AddInterceptedSingleton<TestClassService, TestInterceptor>(factory);
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

    #region AddInterceptedSingleton<TService> with instance and IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with instance registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_instance_registers_with_singleton_lifetime()
    {
      // Arrange
      TestClassService instance = new();

      // Act
      _ = _services.AddInterceptedSingleton(instance, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService> with instance and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with instance and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_instance_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      TestClassService instance = new();
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton(instance, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService, TInterceptor> with instance

    /// <summary>
    /// Tests that AddInterceptedSingleton generic with instance and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_generic_instance_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      TestClassService instance = new();
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton<TestClassService, TestInterceptor>(instance);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton with service and implementation registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_service_implementation_registers_with_singleton_lifetime()
    {
      // Arrange & Act
      _ = _services.AddInterceptedSingleton<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton with service, implementation, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_service_implementation_interceptor_types_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that AddInterceptedSingleton with service, implementation, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_service_implementation_interceptor_type_registers_with_singleton_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedSingleton<ITestService, TestService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton services should return the same instance each time");
    }

    #endregion

    #region AddInterceptedSingleton<TService, TImplementation> with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedSingleton with service, implementation, and factory registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_service_implementation_factory_registers_with_singleton_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestService> factory = sp =>
      {
        factoryCallCount++;
        return new TestService();
      };

      // Act
      _ = _services.AddInterceptedSingleton<ITestService, TestService>(factory, _interceptor);
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

    #region AddInterceptedSingleton<TService, TImplementation> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedSingleton with service, implementation, factory, and interceptor types registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_service_implementation_factory_interceptor_types_registers_with_singleton_lifetime()
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
      _ = _services.AddInterceptedSingleton<ITestService, TestService>(factory, typeof(TestInterceptor));
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

    #region AddInterceptedSingleton<TService, TImplementation, TInterceptor> with factory

    /// <summary>
    /// Tests that AddInterceptedSingleton with service, implementation, factory, and interceptor type registers with singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedSingleton_service_implementation_factory_interceptor_type_registers_with_singleton_lifetime()
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
      _ = _services.AddInterceptedSingleton<ITestService, TestService, TestInterceptor>(factory);
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
