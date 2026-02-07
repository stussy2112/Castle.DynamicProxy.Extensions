// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddInterceptedScoped.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddInterceptedScoped extension methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region AddInterceptedScoped(Type) with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedScoped with Type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddInterceptedScoped_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddInterceptedScoped(serviceType, _interceptor));
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with Type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddInterceptedScoped_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.AddInterceptedScoped(serviceType!, _interceptor));
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with Type registers service with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_type_registers_service_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddInterceptedScoped(serviceType, _interceptor);
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
    /// Tests that AddInterceptedScoped with Type applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddInterceptedScoped_type_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);

      // Act
      _ = _services.AddInterceptedScoped(serviceType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
    }

    #endregion

    #region AddInterceptedScoped<TService> with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedScoped generic single-type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddInterceptedScoped_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddInterceptedScoped<TestService>(_interceptor));
    }

    /// <summary>
    /// Tests that AddInterceptedScoped generic single-type registers service with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedScoped_single_type_registers_service_with_scoped_lifetime()
    {
      // Arrange & Act
      _ = _services.AddInterceptedScoped<TestClassService>(_interceptor);
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
    /// Tests that AddInterceptedScoped generic single-type applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_generic_AddInterceptedScoped_single_type_applies_interceptors()
    {
      // Arrange & Act
      _ = _services.AddInterceptedScoped<TestClassService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped generic single-type returns service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedScoped_single_type_returns_service_collection_for_chaining()
    {
      // Arrange & Act
      IServiceCollection result = _services.AddInterceptedScoped<TestClassService>(_interceptor);

      // Assert
      Assert.AreSame(_services, result, "Should return the same service collection for chaining");
    }

    #endregion

    #region AddInterceptedScoped<TService, TImplementation> with IInterceptor[]

    /// <summary>
    /// Tests that AddInterceptedScoped with service and implementation registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_service_implementation_registers_with_scoped_lifetime()
    {
      // Arrange & Act
      _ = _services.AddInterceptedScoped<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with service and implementation applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddInterceptedScoped_service_implementation_applies_interceptors()
    {
      // Arrange & Act
      _ = _services.AddInterceptedScoped<ITestService, TestService>(_interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    #endregion

    #region AddInterceptedScoped<TService, TInterceptor>

    /// <summary>
    /// Tests that AddInterceptedScoped with generic interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_generic_interceptor_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<TestClassService, TestInterceptor>();
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

    #endregion

    #region AddInterceptedScoped<TService> with factory

    /// <summary>
    /// Tests that AddInterceptedScoped with factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddInterceptedScoped_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Func<IServiceProvider, TestClassService> factory = sp => new TestClassService();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddInterceptedScoped(factory, _interceptor));
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_AddInterceptedScoped_factory_throws_ArgumentNullException()
    {
      // Arrange
      Func<IServiceProvider, TestClassService>? factory = null;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.AddInterceptedScoped(factory!, _interceptor));
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_factory_registers_with_scoped_lifetime()
    {
      // Arrange
      int factoryCallCount = 0;
      Func<IServiceProvider, TestClassService> factory = sp =>
      {
        factoryCallCount++;
        return new TestClassService();
      };

      // Act
      _ = _services.AddInterceptedScoped(factory, _interceptor);
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
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with factory applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptors_provided_AddInterceptedScoped_factory_applies_interceptors()
    {
      // Arrange
      TestClassService instance = new();
      Func<IServiceProvider, TestClassService> factory = sp => instance;

      // Act
      _ = _services.AddInterceptedScoped(factory, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(instance.WasCalled, "Factory instance should be used");
    }

    #endregion

    #region AddInterceptedScoped(Type) with Type implementationType

    /// <summary>
    /// Tests that AddInterceptedScoped with Type and implementationType registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_type_implementation_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act
      _ = _services.AddInterceptedScoped(serviceType, implementationType, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with Type and implementationType throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddInterceptedScoped_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddInterceptedScoped(serviceType, implementationType, _interceptor));
    }

    #endregion

    #region AddInterceptedScoped(Type) with factory

    /// <summary>
    /// Tests that AddInterceptedScoped with Type and factory registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_type_factory_registers_with_scoped_lifetime()
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
      _ = _services.AddInterceptedScoped(serviceType, factory, _interceptor);
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
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    #endregion

    #region AddInterceptedScoped with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedScoped with interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<TestClassService>(typeof(TestInterceptor));
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
    /// Tests that AddInterceptedScoped with interceptor types applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_provided_AddInterceptedScoped_applies_interceptors()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<TestClassService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    #endregion

    #region AddInterceptedScoped<TService, TImplementation, TInterceptor>

    /// <summary>
    /// Tests that AddInterceptedScoped with service, implementation, and interceptor registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_service_implementation_interceptor_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<ITestService, TestService, TestInterceptor>();
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    #endregion

    #region AddInterceptedScoped(Type, Type implementationType) with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedScoped with Type, implementationType, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_type_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with Type, implementationType, and interceptor types applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_provided_AddInterceptedScoped_type_implementation_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped(serviceType, implementationType, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    #endregion

    #region AddInterceptedScoped(Type) with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedScoped with Type, factory, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_type_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.AddInterceptedScoped(serviceType, factory, typeof(TestInterceptor));
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
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with Type, factory, and interceptor types applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_provided_AddInterceptedScoped_type_factory_applies_interceptors()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService instance = new();
      Func<IServiceProvider, object> factory = sp => instance;
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped(serviceType, factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      string result = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(instance.WasCalled, "Factory instance should be used");
    }

    #endregion

    #region AddInterceptedScoped<TService> with factory and Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedScoped generic with factory and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddInterceptedScoped_factory_interceptor_types_registers_with_scoped_lifetime()
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
      _ = _services.AddInterceptedScoped(factory, typeof(TestInterceptor));
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
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped generic with factory and interceptor types applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_provided_generic_AddInterceptedScoped_factory_applies_interceptors()
    {
      // Arrange
      TestClassService instance = new();
      Func<IServiceProvider, TestClassService> factory = sp => instance;
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped(factory, typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      string result = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(instance.WasCalled, "Factory instance should be used");
    }

    #endregion

    #region AddInterceptedScoped<TService, TInterceptor> with factory

    /// <summary>
    /// Tests that AddInterceptedScoped with generic service, factory, and interceptor type registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_generic_service_factory_interceptor_registers_with_scoped_lifetime()
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
      _ = _services.AddInterceptedScoped<TestClassService, TestInterceptor>(factory);
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
      Assert.AreEqual(2, factoryCallCount, "Factory should be called once per scope");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with generic service, factory, and interceptor type applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_generic_interceptor_provided_AddInterceptedScoped_generic_service_factory_applies_interceptors()
    {
      // Arrange
      TestClassService instance = new();
      Func<IServiceProvider, TestClassService> factory = sp => instance;
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<TestClassService, TestInterceptor>(factory);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      string result = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(instance.WasCalled, "Factory instance should be used");
    }

    #endregion

    #region AddInterceptedScoped<TService, TImplementation> with Type[] interceptorTypes

    /// <summary>
    /// Tests that AddInterceptedScoped with service, implementation, and interceptor types registers with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_called_AddInterceptedScoped_service_implementation_interceptor_types_registers_with_scoped_lifetime()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      ITestService? service1a = scope1.ServiceProvider.GetService<ITestService>();
      ITestService? service1b = scope1.ServiceProvider.GetService<ITestService>();

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service2 = scope2.ServiceProvider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
    }

    /// <summary>
    /// Tests that AddInterceptedScoped with service, implementation, and interceptor types applies interceptors.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_provided_AddInterceptedScoped_service_implementation_applies_interceptors()
    {
      // Arrange
      _ = _services.AddSingleton(_interceptor);

      // Act
      _ = _services.AddInterceptedScoped<ITestService, TestService>(typeof(TestInterceptor));
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    #endregion

    #region AddInterceptedScoped with no interceptors

    /// <summary>
    /// Tests that AddInterceptedScoped with no interceptors registers service without proxy but with scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddInterceptedScoped_registers_service_without_proxy_with_scoped_lifetime()
    {
      // Arrange
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services.AddInterceptedScoped<TestService>(interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();

      using IServiceScope scope1 = provider.CreateScope();
      TestService? service1a = scope1.ServiceProvider.GetService<TestService>();
      TestService? service1b = scope1.ServiceProvider.GetService<TestService>();

      using IServiceScope scope2 = provider.CreateScope();
      TestService? service2 = scope2.ServiceProvider.GetService<TestService>();

      // Assert
      Assert.IsNotNull(service1a, "First service in scope 1 should be registered");
      Assert.IsNotNull(service1b, "Second service in scope 1 should be registered");
      Assert.IsNotNull(service2, "Service in scope 2 should be registered");
      Assert.AreSame(service1a, service1b, "Should return same instance within same scope");
      Assert.AreNotSame(service1a, service2, "Should return different instances across different scopes");
      _ = Assert.IsInstanceOfType<TestService>(service1a, "Service should be concrete type, not proxy");
    }

    #endregion
  }
}
