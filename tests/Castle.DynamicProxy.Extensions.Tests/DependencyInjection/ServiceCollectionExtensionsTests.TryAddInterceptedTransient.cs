// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddInterceptedTransient.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for TryAddInterceptedTransient extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddInterceptedTransient(Type) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_type_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.TryAddInterceptedTransient(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedTransient_type_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      _ = _services.AddInterceptedTransient(serviceType, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddInterceptedTransient(serviceType, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddInterceptedTransient(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_type_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedTransient(serviceType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_type_implementation_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.TryAddInterceptedTransient(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_type_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedTransient(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type and factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_type_factory_registers_with_transient_lifetime()
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
      _ = _services.TryAddInterceptedTransient(serviceType, factory, _interceptor);
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

    #region TryAddInterceptedTransient(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedTransient with Type, factory, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_type_factory_interceptor_types_registers_with_transient_lifetime()
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
      _ = _services.TryAddInterceptedTransient(serviceType, factory, typeof(TestInterceptor));
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

    #region TryAddInterceptedTransient<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedTransient generic registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddInterceptedTransient_registers_with_transient_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddInterceptedTransient<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedTransient generic with interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddInterceptedTransient_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedTransient<TestClassService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddInterceptedTransient generic with interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_generic_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedTransient<TestClassService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient<TService> with factory

    /// <summary>
    /// Tests that TryAddInterceptedTransient generic with factory registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_generic_factory_registers_with_transient_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddInterceptedTransient(factory, _interceptor);
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

    #region TryAddInterceptedTransient<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedTransient generic with factory and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_generic_factory_interceptor_types_registers_with_transient_lifetime()
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
      _ = _services.TryAddInterceptedTransient(factory, typeof(TestInterceptor));
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

    #region TryAddInterceptedTransient<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddInterceptedTransient generic with factory and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_generic_factory_interceptor_type_registers_with_transient_lifetime()
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
      _ = _services.TryAddInterceptedTransient<TestClassService, TestInterceptor>(factory);
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

    #region TryAddInterceptedTransient<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedTransient with service and implementation registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_service_implementation_registers_with_transient_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddInterceptedTransient<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedTransient with service, implementation, and interceptor types registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_service_implementation_interceptor_types_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedTransient<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient services should return different instances each time");
    }

    #endregion

    #region TryAddInterceptedTransient<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddInterceptedTransient with service, implementation, and interceptor type registers with transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedTransient_service_implementation_interceptor_type_registers_with_transient_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedTransient<ITestService, TestService, TestInterceptor>();
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
