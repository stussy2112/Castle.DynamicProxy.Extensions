// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddInterceptedTransient.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddInterceptedTransient extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region AddInterceptedTransient(Type) with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedTransient with Type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_type_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddInterceptedTransient(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    /// <summary>
    /// Tests that AddInterceptedTransient applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddInterceptedTransient_type_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddInterceptedTransient(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddInterceptedTransient(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedTransient with Type and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_type_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedTransient(serviceType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedTransient with Type and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_type_implementation_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.AddInterceptedTransient(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedTransient with Type, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_type_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedTransient(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedTransient with Type and factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_type_factory_registers_with_transient_lifetime()
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
      _ = _services.AddInterceptedTransient(serviceType, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region AddInterceptedTransient(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedTransient with Type, factory, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_type_factory_interceptor_types_registers_with_transient_lifetime()
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
      _ = _services.AddInterceptedTransient(serviceType, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region AddInterceptedTransient<TService> with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedTransient generic registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedTransient_registers_with_transient_lifetime()
    {
      // Arrange & Act
      _ = _services.AddInterceptedTransient<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedTransient generic with interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedTransient_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedTransient<TestClassService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient<TService, TInterceptor>

    /// <summary>
    /// Tests that AddInterceptedTransient generic with interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_generic_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedTransient<TestClassService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient<TService> with factory

    /// <summary>
    /// Tests that AddInterceptedTransient generic with factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_generic_factory_registers_with_transient_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.AddInterceptedTransient(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region AddInterceptedTransient<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedTransient generic with factory and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_generic_factory_interceptor_types_registers_with_transient_lifetime()
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
      _ = _services.AddInterceptedTransient(factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region AddInterceptedTransient<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that AddInterceptedTransient generic with factory and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_generic_factory_interceptor_type_registers_with_transient_lifetime()
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
      _ = _services.AddInterceptedTransient<TestClassService, TestInterceptor>(factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called each time service is requested");
    }

    #endregion

    #region AddInterceptedTransient<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedTransient with service and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_service_implementation_registers_with_transient_lifetime()
    {
      // Arrange & Act
      _ = _services.AddInterceptedTransient<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedTransient with service, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_service_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedTransient<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region AddInterceptedTransient<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that AddInterceptedTransient with service, implementation, and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedTransient_service_implementation_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedTransient<ITestService, TestService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion
  }
}
