// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddIntercepted.Factory.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddIntercepted with factory overload.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddIntercepted with factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddIntercepted_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with factory throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddIntercepted_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType!, factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_AddIntercepted_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType, factory!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with factory and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, Array.Empty<IInterceptor>());
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory and null interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptors_is_null_AddIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptors);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory and empty interceptor array registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptors_is_empty_AddIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, Array.Empty<IInterceptor>());
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_AddIntercepted_creates_intercepted_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsFalse(_interceptor!.WasInvoked, "Interceptor should not be invoked yet");

      string result = service.GetName();

      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory creates class proxy for non-interface types.
    /// </summary>
    [TestMethod]
    public void When_class_type_provided_AddIntercepted_creates_class_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      Func<IServiceProvider, object> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsFalse(_interceptor!.WasInvoked, "Interceptor should not be invoked yet");

      string result = service.GetName();

      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.AreEqual("GetName", _interceptor.InterceptedMethodName, "Interceptor should intercept GetName method");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddIntercepted_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddIntercepted_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory respects Scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_scoped_lifetime_AddIntercepted_returns_same_instance_in_scope()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Scoped;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();

      using (IServiceScope scope1 = provider.CreateScope())
      {
        ITestService? service1 = scope1.ServiceProvider.GetService<ITestService>();
        ITestService? service2 = scope1.ServiceProvider.GetService<ITestService>();

        // Assert
        Assert.IsNotNull(service1, "First service should be registered");
        Assert.IsNotNull(service2, "Second service should be registered");
        Assert.AreSame(service1, service2, "Scoped should return same instance within scope");
      }

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service3 = scope2.ServiceProvider.GetService<ITestService>();

      using (IServiceScope scope1 = provider.CreateScope())
      {
        ITestService? service1 = scope1.ServiceProvider.GetService<ITestService>();
        Assert.AreNotSame(service1, service3, "Scoped should return different instances across scopes");
      }
    }

    /// <summary>
    /// Tests that AddIntercepted with factory applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_AddIntercepted_applies_in_order()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!, secondInterceptor);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted with factory returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_AddIntercepted_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted(serviceType, factory, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }
  }
}