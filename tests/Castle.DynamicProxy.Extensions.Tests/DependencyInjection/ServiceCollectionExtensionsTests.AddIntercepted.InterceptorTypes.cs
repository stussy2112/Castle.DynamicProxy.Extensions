// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.AddIntercepted.InterceptorTypes.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for AddIntercepted with interceptor types overload.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_AddIntercepted_interceptor_types_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes));
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_AddIntercepted_interceptor_types_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType!, factory, lifetime, interceptorTypes));
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_AddIntercepted_interceptor_types_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services!.AddIntercepted(serviceType, factory!, lifetime, interceptorTypes));
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types and no interceptors registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_no_interceptor_types_provided_AddIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] emptyInterceptorTypes = [];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, emptyInterceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types and null array registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_is_null_AddIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[]? interceptorTypes = null;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes!);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types and empty array registers service without proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_types_is_empty_AddIntercepted_registers_service_without_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = Assert.IsInstanceOfType<TestService>(service, "Service should be concrete type, not proxy");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types creates intercepted interface proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_provided_AddIntercepted_creates_intercepted_interface_proxy()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.AreEqual("TestService", result, "Method should return correct value");
      Assert.IsTrue(service.WasCalled, "Original method should be called");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types creates intercepted class proxy.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_provided_AddIntercepted_creates_intercepted_class_proxy()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      Func<IServiceProvider, object> factory = (sp) => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.AreEqual("TestClassService", result, "Method should return correct value");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types instantiates interceptor with dependencies.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_has_dependencies_AddIntercepted_resolves_dependencies()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(InterceptorWithDependency)];

      _ = _services!.AddSingleton<TestDependency>();

      // Act
      _ = _services.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      string result = service.GetName();

      Assert.AreEqual("TestService", result, "Method should return correct value");
      TestDependency? dependency = provider.GetService<TestDependency>();
      Assert.IsNotNull(dependency, "Dependency should be registered");
      Assert.IsTrue(dependency.WasUsed, "Interceptor should have used dependency");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types applies multiple interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_interceptor_types_provided_AddIntercepted_applies_in_order()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(OrderTrackingInterceptor1), typeof(OrderTrackingInterceptor2)];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();

      Assert.HasCount(2, OrderTrackingInterceptor1.ExecutionOrder, "Both interceptors should execute");
      Assert.AreEqual("Interceptor1", OrderTrackingInterceptor1.ExecutionOrder[0], "First interceptor should execute first");
      Assert.AreEqual("Interceptor2", OrderTrackingInterceptor1.ExecutionOrder[1], "Second interceptor should execute second");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types respects Singleton lifetime.
    /// </summary>
    [TestMethod]
    public void When_singleton_lifetime_AddIntercepted_interceptor_types_returns_same_instance()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Singleton;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreSame(service1, service2, "Singleton should return same instance");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types respects Transient lifetime.
    /// </summary>
    [TestMethod]
    public void When_transient_lifetime_AddIntercepted_interceptor_types_returns_different_instances()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.AreNotSame(service1, service2, "Transient should return different instances");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types respects Scoped lifetime.
    /// </summary>
    [TestMethod]
    public void When_scoped_lifetime_AddIntercepted_interceptor_types_returns_same_instance_in_scope()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Scoped;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();

      // Assert
      using (IServiceScope scope1 = provider.CreateScope())
      {
        ITestService? service1 = scope1.ServiceProvider.GetService<ITestService>();
        ITestService? service2 = scope1.ServiceProvider.GetService<ITestService>();
        Assert.IsNotNull(service1, "First service in scope should be registered");
        Assert.IsNotNull(service2, "Second service in scope should be registered");
        Assert.AreSame(service1, service2, "Scoped should return same instance within scope");
      }

      using IServiceScope scope2 = provider.CreateScope();
      ITestService? service3 = scope2.ServiceProvider.GetService<ITestService>();
      Assert.IsNotNull(service3, "Service in new scope should be registered");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentException when interceptor type does not implement IInterceptor.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_does_not_implement_IInterceptor_AddIntercepted_throws_ArgumentException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(NotAnInterceptor)];

      // Act & Assert
      ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(() => _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes));
      Assert.AreEqual("interceptorTypes", ex.ParamName, "Exception should specify correct parameter name");
      Assert.Contains("NotAnInterceptor", ex.Message, "Exception message should contain type name");
      Assert.Contains(nameof(IInterceptor), ex.Message, "Exception message should mention IInterceptor");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentException when interceptor type is interface.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_is_interface_AddIntercepted_throws_ArgumentException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(IInterceptor)];

      // Act & Assert
      ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(() => _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes));
      Assert.AreEqual("interceptorTypes", ex.ParamName, "Exception should specify correct parameter name");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentException when interceptor type is abstract.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_is_abstract_AddIntercepted_throws_ArgumentException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(AbstractInterceptorBase)];

      // Act & Assert
      ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(() => _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes));
      Assert.AreEqual("interceptorTypes", ex.ParamName, "Exception should specify correct parameter name");
      Assert.Contains("AbstractInterceptorBase", ex.Message, "Exception message should contain type name");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types throws ArgumentException when interceptor type has no public constructor.
    /// </summary>
    [TestMethod]
    public void When_interceptor_type_has_no_public_constructor_AddIntercepted_throws_ArgumentException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(InterceptorWithPrivateConstructor)];

      // Act & Assert
      ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(() => _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes));
      Assert.AreEqual("interceptorTypes", ex.ParamName, "Exception should specify correct parameter name");
      Assert.Contains("InterceptorWithPrivateConstructor", ex.Message, "Exception message should contain type name");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types validates all types before registration.
    /// </summary>
    [TestMethod]
    public void When_one_of_multiple_interceptor_types_is_invalid_AddIntercepted_throws_ArgumentException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor), typeof(NotAnInterceptor), typeof(SecondTestInterceptor)];

      // Act & Assert
      ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(() => _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes));
      Assert.AreEqual("interceptorTypes", ex.ParamName, "Exception should specify correct parameter name");
      Assert.Contains("NotAnInterceptor", ex.Message, "Exception message should contain invalid type name");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types can be chained.
    /// </summary>
    [TestMethod]
    public void When_AddIntercepted_with_interceptor_types_called_it_returns_service_collection_for_chaining()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(TestInterceptor)];

      // Act
      IServiceCollection result = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);

      // Assert
      Assert.AreSame(_services, result, "Should return same service collection for chaining");
    }

    /// <summary>
    /// Tests that AddIntercepted with interceptor types creates new interceptor instances per service resolution.
    /// </summary>
    [TestMethod]
    public void When_transient_service_resolved_multiple_times_AddIntercepted_creates_new_interceptor_instances()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Func<IServiceProvider, object> factory = (sp) => new TestService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;
      Type[] interceptorTypes = [typeof(InstanceTrackingInterceptor)];

      InstanceTrackingInterceptor.InstanceCount = 0;

      // Act
      _ = _services!.AddIntercepted(serviceType, factory, lifetime, interceptorTypes);
      IServiceProvider provider = _services.BuildServiceProvider();

      ITestService? service1 = provider.GetService<ITestService>();
      ITestService? service2 = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service1, "First service should be registered");
      Assert.IsNotNull(service2, "Second service should be registered");
      Assert.IsGreaterThanOrEqualTo(2, InstanceTrackingInterceptor.InstanceCount, "Should create new interceptor instances");
    }
  }

  /// <summary>
  /// Test class that does not implement IInterceptor.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class NotAnInterceptor
  {
  }

  /// <summary>
  /// Abstract interceptor base class for testing validation.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public abstract class AbstractInterceptorBase : IInterceptor
  {
    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public abstract void Intercept(IInvocation invocation);
  }

  /// <summary>
  /// Interceptor with private constructor for testing validation.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class InterceptorWithPrivateConstructor : IInterceptor
  {
    private InterceptorWithPrivateConstructor()
    {
    }

    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      invocation.Proceed();
    }
  }

  /// <summary>
  /// Interceptor with dependency for testing dependency resolution.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class InterceptorWithDependency : IInterceptor
  {
    private readonly TestDependency _dependency;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterceptorWithDependency"/> class.
    /// </summary>
    /// <param name="dependency">The test dependency.</param>
    public InterceptorWithDependency(TestDependency dependency) => _dependency = dependency;

    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      _dependency.WasUsed = true;
      invocation!.Proceed();
    }
  }

  /// <summary>
  /// First order tracking interceptor.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class OrderTrackingInterceptor1 : IInterceptor
  {
    /// <summary>
    /// Gets the execution order of interceptors.
    /// </summary>
    public static Collection<string> ExecutionOrder { get; } = [];

    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      ExecutionOrder.Add("Interceptor1");
      invocation!.Proceed();
    }
  }

  /// <summary>
  /// Second order tracking interceptor.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class OrderTrackingInterceptor2 : IInterceptor
  {
    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      OrderTrackingInterceptor1.ExecutionOrder.Add("Interceptor2");
      invocation!.Proceed();
    }
  }

  /// <summary>
  /// Interceptor that tracks instance creation.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class InstanceTrackingInterceptor : IInterceptor
  {
    /// <summary>
    /// Gets or sets the instance count.
    /// </summary>
    public static int InstanceCount { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceTrackingInterceptor"/> class.
    /// </summary>
    public InstanceTrackingInterceptor() => InstanceCount++;

    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      invocation!.Proceed();
    }
  }
}
