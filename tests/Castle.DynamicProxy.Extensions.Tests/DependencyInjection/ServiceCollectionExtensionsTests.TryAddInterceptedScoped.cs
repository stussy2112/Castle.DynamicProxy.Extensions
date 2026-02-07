// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddInterceptedScoped.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for TryAddInterceptedScoped extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddInterceptedScoped(Type) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_type_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.TryAddInterceptedScoped(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddInterceptedScoped_type_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      _ = _services.AddInterceptedScoped(serviceType, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddInterceptedScoped(serviceType, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddInterceptedScoped(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_type_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedScoped(serviceType, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_type_implementation_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.TryAddInterceptedScoped(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_type_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedScoped(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type and factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_type_factory_registers_with_scoped_lifetime()
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
      _ = _services.TryAddInterceptedScoped(serviceType, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region TryAddInterceptedScoped(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedScoped with Type, factory, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_type_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.TryAddInterceptedScoped(serviceType, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region TryAddInterceptedScoped<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedScoped generic registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddInterceptedScoped_registers_with_scoped_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddInterceptedScoped<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedScoped generic with interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddInterceptedScoped_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedScoped<TestClassService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddInterceptedScoped generic with interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_generic_interceptor_type_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedScoped<TestClassService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped<TService> with factory

    /// <summary>
    /// Tests that TryAddInterceptedScoped generic with factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_generic_factory_registers_with_scoped_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.TryAddInterceptedScoped(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region TryAddInterceptedScoped<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedScoped generic with factory and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_generic_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.TryAddInterceptedScoped(factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region TryAddInterceptedScoped<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddInterceptedScoped generic with factory and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_generic_factory_interceptor_type_registers_with_scoped_lifetime()
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
      _ = _services.TryAddInterceptedScoped<TestClassService, TestInterceptor>(factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetService<TestClassService>();
      TestClassService? service1b = scope1.ServiceProvider.GetService<TestClassService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region TryAddInterceptedScoped<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddInterceptedScoped with service and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_service_implementation_registers_with_scoped_lifetime()
    {
      // Arrange & Act
      _ = _services.TryAddInterceptedScoped<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddInterceptedScoped with service, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_service_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedScoped<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region TryAddInterceptedScoped<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddInterceptedScoped with service, implementation, and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddInterceptedScoped_service_implementation_interceptor_type_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddInterceptedScoped<ITestService, TestService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion
  }
}
