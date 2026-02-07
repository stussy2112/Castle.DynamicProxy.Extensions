// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.TryAddIntercepted.Type.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for non-generic TryAddIntercepted methods.
  /// </summary>
  public partial class ServiceCollectionExtensionsTests
  {
    #region TryAddIntercepted(Type serviceType) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddIntercepted with Type throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_TryAddIntercepted_type_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddIntercepted(serviceType, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_TryAddIntercepted_type_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddIntercepted(serviceType!, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type registers service when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_TryAddIntercepted_type_registers_service()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddIntercepted(serviceType, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_type_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted(serviceType, lifetime, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted(serviceType, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted(Type serviceType, factory) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddIntercepted with Type and factory throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_TryAddIntercepted_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(TestClassService);
      Func<IServiceProvider, object> factory = sp => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddIntercepted(serviceType, factory, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and factory throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_TryAddIntercepted_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      Func<IServiceProvider, object> factory = sp => new TestClassService();
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddIntercepted(serviceType!, factory, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and factory throws ArgumentNullException when factory is null.
    /// </summary>
    [TestMethod]
    public void When_factory_is_null_TryAddIntercepted_type_factory_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      Func<IServiceProvider, object>? factory = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddIntercepted(serviceType, factory!, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and factory does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_type_factory_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService originalInstance = new();
      Func<IServiceProvider, object> originalFactory = sp => originalInstance;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted(serviceType, originalFactory, lifetime, _interceptor);

      // Act
      TestClassService newInstance = new();
      Func<IServiceProvider, object> newFactory = sp => newInstance;
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted(serviceType, newFactory, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted(Type serviceType, Type implementationType) with IInterceptor[]

    /// <summary>
    /// Tests that TryAddIntercepted with Type and implementationType throws ArgumentNullException when services is null.
    /// </summary>
    [TestMethod]
    public void When_services_is_null_TryAddIntercepted_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      IServiceCollection? services = null;
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => services!.TryAddIntercepted(serviceType, implementationType, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and implementationType throws ArgumentNullException when serviceType is null.
    /// </summary>
    [TestMethod]
    public void When_serviceType_is_null_TryAddIntercepted_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      Type? serviceType = null;
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddIntercepted(serviceType!, implementationType, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and implementationType throws ArgumentNullException when implementationType is null.
    /// </summary>
    [TestMethod]
    public void When_implementationType_is_null_TryAddIntercepted_type_implementation_throws_ArgumentNullException()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type? implementationType = null;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _services.TryAddIntercepted(serviceType, implementationType!, lifetime, _interceptor));
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and implementationType registers service when not already registered.
    /// </summary>
    [TestMethod]
    public void When_service_not_registered_TryAddIntercepted_type_implementation_registers_service()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      // Act
      _ = _services.TryAddIntercepted(serviceType, implementationType, lifetime, _interceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "Interceptor should be invoked");
      Assert.IsTrue(service.WasCalled, "Service method should be called");
    }

    /// <summary>
    /// Tests that TryAddIntercepted with Type and implementationType does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_type_implementation_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted(serviceType, implementationType, lifetime, _interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted(serviceType, implementationType, lifetime, secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted(Type) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with Type and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_type_interceptor_types_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted(serviceType, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted(serviceType, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted(Type, factory) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with Type, factory, and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_type_factory_interceptor_types_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(TestClassService);
      TestClassService originalInstance = new();
      Func<IServiceProvider, object> originalFactory = sp => originalInstance;
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted(serviceType, originalFactory, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      TestClassService newInstance = new();
      Func<IServiceProvider, object> newFactory = sp => newInstance;
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted(serviceType, newFactory, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      TestClassService? service = provider.GetService<TestClassService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");
      _ = service.GetName();
      Assert.IsTrue(originalInstance.WasCalled, "Original instance should be used");
      Assert.IsFalse(newInstance.WasCalled, "New instance should not be used");
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion

    #region TryAddIntercepted(Type, Type implementationType) with Type[] interceptorTypes

    /// <summary>
    /// Tests that TryAddIntercepted with Type, implementationType, and interceptor types does not register when already registered.
    /// </summary>
    [TestMethod]
    public void When_service_already_registered_TryAddIntercepted_type_implementation_interceptor_types_does_not_replace()
    {
      // Arrange
      Type serviceType = typeof(ITestService);
      Type implementationType = typeof(TestService);
      ServiceLifetime lifetime = ServiceLifetime.Transient;

      _ = _services.AddIntercepted(serviceType, implementationType, lifetime, typeof(TestInterceptor));
      _ = _services.AddSingleton(_interceptor);

      // Act
      SecondTestInterceptor secondInterceptor = new();
      _ = _services.TryAddIntercepted(serviceType, implementationType, lifetime, typeof(SecondTestInterceptor));
      _ = _services.AddSingleton(secondInterceptor);
      IServiceProvider provider = _services.BuildServiceProvider();
      ITestService? service = provider.GetService<ITestService>();

      // Assert
      Assert.IsNotNull(service, "Service should be registered");

      _ = service.GetName();
      Assert.IsTrue(_interceptor.WasInvoked, "First interceptor should be invoked");
      Assert.IsFalse(secondInterceptor.WasInvoked, "Second interceptor should not be invoked");
    }

    #endregion
  }
}
