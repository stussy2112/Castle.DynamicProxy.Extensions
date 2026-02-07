// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddIntercepted.Generic.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for generic AddIntercepted methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddIntercepted generic single-type method throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted<TestService>(lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type method with no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddIntercepted_single_type_registers_service_without_proxy()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted<TestService>(lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestService? service = provider.GetService<TestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type method creates intercepted proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddIntercepted_single_type_creates_intercepted_proxy()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<TestClassService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddIntercepted generic single-type method respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddIntercepted_single_type_returns_same_instance()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted<TestClassService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type method respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddIntercepted_single_type_returns_different_instances()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<TestClassService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type method applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddIntercepted_single_type_applies_in_order()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddIntercepted<TestClassService>(lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type method returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddIntercepted_single_type_returns_service_collection_for_chaining()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted<TestClassService>(lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddIntercepted_single_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted(factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_generic_AddIntercepted_single_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      Func<IServiceProvider, TestClassService>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(factory!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method with no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddIntercepted_single_type_factory_registers_service_without_proxy()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted(factory, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestClassService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method creates intercepted proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddIntercepted_single_type_factory_creates_intercepted_proxy()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddIntercepted generic single-type factory method respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddIntercepted_single_type_factory_returns_same_instance()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddIntercepted_single_type_factory_returns_different_instances()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddIntercepted_single_type_factory_applies_in_order()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddIntercepted(factory, lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddIntercepted_single_type_factory_returns_service_collection_for_chaining()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted(factory, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    // Example manual implementation if not automatically registered
    public class MyServiceProviderIsService : IServiceProviderIsService
    {
      private readonly IServiceCollection _services;
      public MyServiceProviderIsService(IServiceCollection services) => _services = services;
      public bool IsService(Type serviceType) => _services.Any(d => d.ServiceType == serviceType);
    }

    /// <summary>
    /// Tests that AddIntercepted generic single-type factory method can inject dependencies via factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_generic_AddIntercepted_single_type_factory_injects_dependencies_via_factory()
    {
      _ = _services!.AddSingleton<IDependency, Dependency>();
      Func<IServiceProvider, ServiceWithDependency> factory = (sp) =>
      {
        IDependency dependency = sp.GetRequiredService<IDependency>();
        return new ServiceWithDependency(dependency);
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddIntercepted(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ServiceWithDependency? service = provider.GetService<ServiceWithDependency>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted<ITestService, TestService>(lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddIntercepted_with_type_registers_service_without_proxy()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddIntercepted_with_type_creates_intercepted_proxy()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddIntercepted generic method with type creates class proxy for non-interface types.
    /// </summary>
    [TestMethod]
    public void When_class_type_provided_generic_AddIntercepted_with_type_creates_class_proxy()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<TestClassService, TestClassService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddIntercepted generic method with type respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddIntercepted_with_type_returns_same_instance()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddIntercepted_with_type_returns_different_instances()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddIntercepted_with_type_applies_in_order()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddIntercepted_with_type_returns_service_collection_for_chaining()
    {
      // Arrange
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted<ITestService, TestService>(lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with type uses ActivatorUtilities to create instances with dependencies.
    /// </summary>
    [TestMethod]
    public void When_activator_utilities_used_generic_AddIntercepted_with_type_injects_dependencies()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddIntercepted<IServiceWithDependency, ServiceWithDependency>(lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      IServiceWithDependency? service = provider.GetService<IServiceWithDependency>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_generic_AddIntercepted_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted<ITestService, TestService>(factory, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_generic_AddIntercepted_factory_throws_ArgumentNullException()
    {
      // Arrange
      Func<IServiceProvider, TestService>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted<ITestService, TestService>(factory!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_generic_AddIntercepted_factory_registers_service_without_proxy()
    {
      // Arrange
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(factory, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_generic_AddIntercepted_factory_creates_intercepted_proxy()
    {
      // Arrange
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddIntercepted generic method with factory creates class proxy for non-interface types.
    /// </summary>
    [TestMethod]
    public void When_class_type_provided_generic_AddIntercepted_factory_creates_class_proxy()
    {
      // Arrange
      Func<IServiceProvider, TestClassService> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<TestClassService, TestClassService>(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
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
    /// Tests that AddIntercepted generic method with factory respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_generic_AddIntercepted_factory_returns_same_instance()
    {
      // Arrange
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_generic_AddIntercepted_factory_returns_different_instances()
    {
      // Arrange
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_generic_AddIntercepted_factory_applies_in_order()
    {
      // Arrange
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddIntercepted<ITestService, TestService>(factory, lifetime, _interceptor!, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.IsTrue(_interceptor!.WasInvoked, "First interceptor should be invoked");
      Assert.IsTrue(secondInterceptor.WasInvoked, "Second interceptor should be invoked");
      Assert.AreEqual("TestService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_generic_AddIntercepted_factory_returns_service_collection_for_chaining()
    {
      // Arrange
      Func<IServiceProvider, TestService> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted<ITestService, TestService>(factory, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddIntercepted generic method with factory can inject dependencies via factory.
    /// </summary>
    [TestMethod]
    public void When_factory_used_generic_AddIntercepted_factory_injects_dependencies_via_factory()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      Func<IServiceProvider, ServiceWithDependency> factory = (sp) =>
      {
        IDependency dependency = sp.GetRequiredService<IDependency>();
        return new ServiceWithDependency(dependency);
      };
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddIntercepted<IServiceWithDependency, ServiceWithDependency>(factory, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      IServiceWithDependency? service = provider.GetService<IServiceWithDependency>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }
  }
}
