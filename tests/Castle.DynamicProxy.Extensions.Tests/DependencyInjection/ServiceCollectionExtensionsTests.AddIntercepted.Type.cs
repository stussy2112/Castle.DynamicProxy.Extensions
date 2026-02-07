// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddIntercepted.Type.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddIntercepted with implementation type overload.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddIntercepted with single type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted(serviceType, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with single type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddIntercepted_single_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with single type and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddIntercepted_single_type_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted(serviceType, lifetime, interceptors);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestClassService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with single type creates intercepted class proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_AddIntercepted_single_type_creates_intercepted_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, lifetime, _interceptor!);
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
    /// Tests that AddIntercepted with single type respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddIntercepted_single_type_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted(serviceType, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted with single type respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddIntercepted_single_type_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, lifetime, _interceptor!);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service1 = provider.GetService<TestClassService>();
      TestClassService? service2 = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted with single type applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptors_provided_AddIntercepted_single_type_applies_in_order()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      SecondTestInterceptor secondInterceptor = new();

      // Act
      _ = _services!.AddIntercepted(serviceType, lifetime, _interceptor!, secondInterceptor);
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
    /// Tests that AddIntercepted with single type returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_AddIntercepted_single_type_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted(serviceType, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddIntercepted with type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType!, implementationType, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with type throws ArgumentNullException when implementationType is null.
    /// </summary>
    [TestMethod]
    public void When_implementationType_is_null_AddIntercepted_with_type_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type? implementationType = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType, implementationType!, lifetime, _interceptor!));
    }

    /// <summary>
    /// Tests that AddIntercepted with type and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptors_provided_AddIntercepted_with_type_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      IInterceptor[] interceptors = null!;

      // Act
      _ = _services!.AddIntercepted(serviceType, implementationType, lifetime, interceptors);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with type creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_provided_AddIntercepted_with_type_creates_intercepted_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!);
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
    /// Tests that AddIntercepted with type creates class proxy for non-interface types.
    /// </summary>
    [TestMethod]
    public void When_class_type_provided_AddIntercepted_with_type_creates_class_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      Type implementationType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!);
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
    /// Tests that AddIntercepted with type respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddIntercepted_with_type_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Singleton;

      // Act
      _ = _services!.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted with type respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddIntercepted_with_type_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services!.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted with type returns the service collection for chaining.
    /// </summary>
    [TestMethod]
    public void When_called_AddIntercepted_with_type_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      IServiceCollection result = _services!.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!);

      // Assert
      Assert.AreSame(_services, result, "Method should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddIntercepted with type uses ActivatorUtilities to create instances with dependencies.
    /// </summary>
    [TestMethod]
    public void When_activator_utilities_used_AddIntercepted_with_type_injects_dependencies()
    {
      // Arrange
      _ = _services!.AddSingleton<IDependency, Dependency>();
      Type serviceType = typeof(IServiceWithDependency);
      Type implementationType = typeof(ServiceWithDependency);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.AddIntercepted(serviceType, implementationType, lifetime, _interceptor!);
      ServiceProvider provider = _services.BuildServiceProvider();
      IServiceWithDependency? service = provider.GetService<IServiceWithDependency>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      Assert.IsNotNull(service.Dependency, "Dependency should be injected");
      _ = Assert.IsInstanceOfType<Dependency>(service.Dependency, "Dependency should be correct type");
    }
  }

  /// <summary>
  /// Test dependency interface.
  /// </summary>
  public interface IDependency
  {
    /// <summary>
    /// Gets the name of the dependency.
    /// </summary>
    /// <returns>The name.</returns>
    string GetName();
  }

  /// <summary>
  /// Test dependency implementation.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class Dependency : IDependency
  {
    /// <summary>
    /// Gets the name of the dependency.
    /// </summary>
    /// <returns>The name.</returns>
    public string GetName() => "Dependency";
  }

  /// <summary>
  /// Test service interface with dependency.
  /// </summary>
  public interface IServiceWithDependency
  {
    /// <summary>
    /// Gets the dependency.
    /// </summary>
    IDependency Dependency { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <returns>The name.</returns>
    string GetName();
  }

  /// <summary>
  /// Test service implementation with dependency.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class ServiceWithDependency : IServiceWithDependency
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceWithDependency"/> class.
    /// </summary>
    /// <param name="dependency">The dependency.</param>
    public ServiceWithDependency(IDependency dependency) => Dependency = dependency;

    /// <summary>
    /// Gets the dependency.
    /// </summary>
    public IDependency Dependency { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <returns>The name.</returns>
    public string GetName() => $"{ClassName}-{Dependency.GetName()}";

    public virtual string ClassName { get; set; } = "ServiceWithDependency";

    public virtual void DoSomething()
    {
      // Test method
    }
  }
}