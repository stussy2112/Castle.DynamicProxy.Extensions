// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddKeyedInterceptedScoped.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for TryAddKeyedInterceptedScoped extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddKeyedInterceptedScoped(Type) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with Type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_type_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, _interceptor);
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
    /// Tests that TryAddKeyedInterceptedScoped with Type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddKeyedInterceptedScoped_type_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";

      _ = _services.AddKeyedInterceptedScoped(serviceType, serviceKey, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetKeyedService<TestClassService>(serviceKey);

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddKeyedInterceptedScoped(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with Type and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_type_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, interceptorTypes: typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedScoped(Type, Type impl) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with Type and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_type_implementation_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, implementationType, _interceptor);
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

    #region TryAddKeyedInterceptedScoped(Type, Type impl) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with Type, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_type_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, implementationType, typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedScoped(Type) with factory and IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with Type and factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_type_factory_registers_with_scoped_lifetime()
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
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, factory, _interceptor);
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

    #region TryAddKeyedInterceptedScoped(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with Type, factory, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_type_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.TryAddKeyedInterceptedScoped(serviceType, serviceKey, factory, typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedScoped<TService> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped generic registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddKeyedInterceptedScoped_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<TestClassService>(serviceKey, _interceptor);
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

    #region TryAddKeyedInterceptedScoped<TService> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped generic with interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_TryAddKeyedInterceptedScoped_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<TestClassService>(serviceKey, typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedScoped<TService, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped generic with interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_generic_interceptor_type_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<TestClassService, TestInterceptor>(serviceKey);
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

    #region TryAddKeyedInterceptedScoped<TService> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped generic with factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_generic_factory_registers_with_scoped_lifetime()
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
      _ = _services.TryAddKeyedInterceptedScoped(serviceKey, factory, _interceptor);
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

    #region TryAddKeyedInterceptedScoped<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped generic with factory and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_generic_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.TryAddKeyedInterceptedScoped(serviceKey, factory, typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedScoped<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped generic with factory and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_generic_factory_interceptor_type_registers_with_scoped_lifetime()
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
      _ = _services.TryAddKeyedInterceptedScoped<TestClassService, TestInterceptor>(serviceKey, factory);
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

    #region TryAddKeyedInterceptedScoped<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with service and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_service_implementation_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<ITestService, TestService>(serviceKey, _interceptor);
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

    #region TryAddKeyedInterceptedScoped<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with service, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_service_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<ITestService, TestService>(serviceKey, typeof(TestInterceptor));
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

    #region TryAddKeyedInterceptedScoped<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped with service, implementation, and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_TryAddKeyedInterceptedScoped_service_implementation_interceptor_type_registers_with_scoped_lifetime()
    {
      // Arrange
      object? serviceKey = "test-key";
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<ITestService, TestService, TestInterceptor>(serviceKey);
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

    #region TryAddKeyedInterceptedScoped with different keys

    /// <summary>
    /// Tests that TryAddKeyedInterceptedScoped allows different keys but respects Try semantics per key.
    /// </summary>
    [TestMethod]
    public void When_different_keys_used_TryAddKeyedInterceptedScoped_registers_separate_services()
    {
      // Arrange
      object? serviceKey1 = "key-1";
      object? serviceKey2 = "key-2";

      // Act
      _ = _services.TryAddKeyedInterceptedScoped<TestClassService>(serviceKey1, _interceptor);
      _ = _services.TryAddKeyedInterceptedScoped<TestClassService>(serviceKey2, _interceptor);
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
