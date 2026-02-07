// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddKeyedInterceptedScoped.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddKeyedInterceptedScoped extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region AddKeyedInterceptedScoped(Type) with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with Type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_type_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddKeyedInterceptedScoped_type_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddKeyedInterceptedScoped(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with Type and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_type_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, interceptorTypes: typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with Type and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_type_implementation_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service1b = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with Type, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_type_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service1b = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with Type and factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_type_factory_registers_with_scoped_lifetime()
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
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region AddKeyedInterceptedScoped(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with Type, factory, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_type_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService> with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped generic registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedInterceptedScoped_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedScoped<TestClassService>(serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped generic with interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddKeyedInterceptedScoped_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedScoped<TestClassService>(serviceKey, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService, TInterceptor>

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped generic with interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_generic_interceptor_type_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedScoped<TestClassService, TestInterceptor>(serviceKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped generic with factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_generic_factory_registers_with_scoped_lifetime()
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
      _ = _services.AddKeyedInterceptedScoped(serviceKey, factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped generic with factory and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_generic_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.AddKeyedInterceptedScoped(serviceKey, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped generic with factory and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_generic_factory_interceptor_type_registers_with_scoped_lifetime()
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
      _ = _services.AddKeyedInterceptedScoped<TestClassService, TestInterceptor>(serviceKey, factory);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestClassService? service1a = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);
      TestClassService? service1b = scope1.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      TestClassService? service2 = scope2.ServiceProvider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with service and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_service_implementation_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.AddKeyedInterceptedScoped<ITestService, TestService>(serviceKey, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service1b = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with service, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_service_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedScoped<ITestService, TestService>(serviceKey, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service1b = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped with service, implementation, and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddKeyedInterceptedScoped_service_implementation_interceptor_type_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddKeyedInterceptedScoped<ITestService, TestService, TestInterceptor>(serviceKey);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);
      ITestService? service1b = scope1.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetKeyedService<ITestService>(serviceKey);

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddKeyedInterceptedScoped with different keys

    /// <summary>
    /// Tests that AddKeyedInterceptedScoped allows different keys for same service type.
    /// </summary>
    [TestMethod]
    public void When_different_keys_used_AddKeyedInterceptedScoped_registers_separate_services()
    {
      // Arrange
      object? serviceKey1 = "key-1";
      object? serviceKey2 = "key-2";

      // Act
      _ = _services.AddKeyedInterceptedScoped<TestClassService>(serviceKey1, _interceptor);
      _ = _services.AddKeyedInterceptedScoped<TestClassService>(serviceKey2, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope = provider.CreateScope();
      TestClassService? service1 = scope.ServiceProvider.GetKeyedService<TestClassService>(serviceKey1);
      TestClassService? service2 = scope.ServiceProvider.GetKeyedService<TestClassService>(serviceKey2);

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Services with different keys should be different instances");
    }

    #endregion
  }
}
