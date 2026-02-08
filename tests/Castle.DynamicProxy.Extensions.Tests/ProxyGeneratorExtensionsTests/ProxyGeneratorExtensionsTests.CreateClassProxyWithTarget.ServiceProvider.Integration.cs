// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateClassProxyWithTarget.ServiceProvider.Integration.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Integration tests for CreateClassProxyWithTarget extension methods with IServiceProvider and IAsyncInterceptor.
  /// These tests use real ProxyGenerator and ServiceProvider instances since extension methods cannot be mocked.
  /// </summary>
  public partial class ProxyGeneratorExtensionsTests
  {
    #region Generic CreateClassProxyWithTarget<TClass> with IServiceProvider and IAsyncInterceptor

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider and AsyncInterceptor creates a working proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_and_asyncInterceptor_provided_CreateClassProxyWithTarget_generic_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      TestClass target = CreateTestClassInstance();
      IAsyncInterceptor asyncInterceptor = new TestAsyncInterceptor();

      // Act
      TestClass result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, options, and AsyncInterceptor creates a working proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_options_asyncInterceptor_provided_CreateClassProxyWithTarget_generic_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      TestClass target = CreateTestClassInstance();
      IAsyncInterceptor asyncInterceptor = new TestAsyncInterceptor();
      var options = new ProxyGenerationOptions();

      // Act
      TestClass result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        options,
        asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider and AsyncInterceptor intercepts method calls.
    /// </summary>
    [TestMethod]
    public void When_method_called_CreateClassProxyWithTarget_with_serviceProvider_invokes_asyncInterceptor()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      var target = new TestClassWithVirtualMethod();
      var asyncInterceptor = new TestAsyncInterceptor();

      // Act
      TestClassWithVirtualMethod proxy = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        asyncInterceptors: asyncInterceptor);
      string result = proxy.GetValue();

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(asyncInterceptor.WasInvoked, "Interceptor should have been invoked");
    }

    #endregion

    #region Non-Generic CreateClassProxyWithTarget with IServiceProvider and IAsyncInterceptor

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, Type, and AsyncInterceptor creates a working proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_type_asyncInterceptor_provided_CreateClassProxyWithTarget_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      object target = CreateTestClassInstance();
      var asyncInterceptor = new TestAsyncInterceptor();

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClass),
        target,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, Type, options, and AsyncInterceptor creates proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_type_options_asyncInterceptor_provided_CreateClassProxyWithTarget_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      object target = CreateTestClassInstance();
      var asyncInterceptor = new TestAsyncInterceptor();
      var options = new ProxyGenerationOptions();

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClass),
        target,
        options,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, Type, constructorArgs, and AsyncInterceptor creates proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_type_constructorArgs_asyncInterceptor_provided_CreateClassProxyWithTarget_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      var target = new TestClassWithConstructor(42);
      var asyncInterceptor = new TestAsyncInterceptor();
      object[] constructorArgs = [42];

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClassWithConstructor),
        target,
        constructorArgs,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClassWithConstructor>(result);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, Type, options, constructorArgs, and AsyncInterceptor creates proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_type_options_constructorArgs_asyncInterceptor_provided_CreateClassProxyWithTarget_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      var target = new TestClassWithConstructor(42);
      var asyncInterceptor = new TestAsyncInterceptor();
      var options = new ProxyGenerationOptions();
      object[] constructorArgs = [42];

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClassWithConstructor),
        target,
        options,
        constructorArgs,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClassWithConstructor>(result);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, Type, additionalInterfaces, and AsyncInterceptor creates proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_type_additionalInterfaces_asyncInterceptor_provided_CreateClassProxyWithTarget_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      object target = CreateTestClassInstance();
      var asyncInterceptor = new TestAsyncInterceptor();
      Type[] additionalInterfaces = [typeof(IDisposable)];

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClass),
        additionalInterfaces,
        target,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
      _ = Assert.IsInstanceOfType<IDisposable>(result, "Proxy should implement IDisposable");
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with ServiceProvider, Type, additionalInterfaces, options, and AsyncInterceptor creates proxy.
    /// </summary>
    [TestMethod]
    public void When_serviceProvider_type_additionalInterfaces_options_asyncInterceptor_provided_CreateClassProxyWithTarget_creates_proxy()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      object target = CreateTestClassInstance();
      var asyncInterceptor = new TestAsyncInterceptor();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      var options = new ProxyGenerationOptions();

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClass),
        additionalInterfaces,
        target,
        options,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
      _ = Assert.IsInstanceOfType<IDisposable>(result, "Proxy should implement IDisposable");
    }

    #endregion

    #region Integration Tests with Real Interception

    /// <summary>
    /// Tests that proxy with ServiceProvider correctly intercepts virtual method calls.
    /// </summary>
    [TestMethod]
    public void When_virtual_method_called_CreateClassProxyWithTarget_with_serviceProvider_intercepts_correctly()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      var target = new TestClassWithVirtualMethod();
      var asyncInterceptor = new TestAsyncInterceptor();

      // Act
      TestClassWithVirtualMethod proxy = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        asyncInterceptors: asyncInterceptor);
      string result = proxy.GetValue();

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("test-value", result);
      Assert.IsTrue(asyncInterceptor.WasInvoked, "Async interceptor should have been invoked");
      Assert.AreEqual(nameof(TestClassWithVirtualMethod.GetValue), asyncInterceptor.LastMethodName);
    }

    /// <summary>
    /// Tests that proxy with multiple AsyncInterceptors invokes all interceptors in order.
    /// </summary>
    [TestMethod]
    public void When_multiple_asyncInterceptors_CreateClassProxyWithTarget_with_serviceProvider_invokes_all()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      var target = new TestClassWithVirtualMethod();
      var asyncInterceptor1 = new TestAsyncInterceptor();
      var asyncInterceptor2 = new TestAsyncInterceptor();

      // Act
      TestClassWithVirtualMethod proxy = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        new IAsyncInterceptor[] { asyncInterceptor1, asyncInterceptor2 });
      string result = proxy.GetValue();

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(asyncInterceptor1.WasInvoked, "First interceptor should have been invoked");
      Assert.IsTrue(asyncInterceptor2.WasInvoked, "Second interceptor should have been invoked");
    }

    /// <summary>
    /// Tests that proxy with ServiceProvider and additional interfaces can be cast to those interfaces.
    /// </summary>
    [TestMethod]
    public void When_additionalInterfaces_specified_CreateClassProxyWithTarget_with_serviceProvider_implements_interfaces()
    {
      // Arrange
      var proxyGenerator = new ProxyGenerator();
      var serviceCollection = new ServiceCollection();
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
      object target = CreateTestClassInstance();
      var asyncInterceptor = new TestAsyncInterceptor();
      Type[] additionalInterfaces = [typeof(IDisposable), typeof(ICloneable)];

      // Act
      object result = proxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClass),
        additionalInterfaces,
        target,
        asyncInterceptors: asyncInterceptor);

      // Assert
      Assert.IsNotNull(result);
      _ = Assert.IsInstanceOfType<TestClass>(result);
      _ = Assert.IsInstanceOfType<IDisposable>(result, "Proxy should implement IDisposable");
      _ = Assert.IsInstanceOfType<ICloneable>(result, "Proxy should implement ICloneable");
    }

    #endregion

    #region Helper Classes for Integration Tests

    /// <summary>
    /// Test class with a virtual method for interception testing.
    /// </summary>
    public class TestClassWithVirtualMethod
    {
      public virtual string GetValue() => "test-value";
    }

    /// <summary>
    /// Test class with a constructor for constructor argument testing.
    /// </summary>
    public class TestClassWithConstructor
    {
      private readonly int _value;

      public TestClassWithConstructor(int value) => _value = value;

      public virtual int GetValue() => _value;
    }

    /// <summary>
    /// Test AsyncInterceptor that tracks if it was invoked.
    /// </summary>
    public class TestAsyncInterceptor : AsyncInterceptorBase
    {
      public bool WasInvoked { get; private set; }

      public string? LastMethodName { get; private set; }

      public override void Intercept(IInvocation invocation)
      {
        ArgumentNullException.ThrowIfNull(invocation);
        WasInvoked = true;
        LastMethodName = invocation.Method.Name;
        invocation.Proceed();
      }

      public override async System.Threading.Tasks.ValueTask InterceptAsync(IInvocation invocation)
      {
        ArgumentNullException.ThrowIfNull(invocation);
        WasInvoked = true;
        LastMethodName = invocation.Method.Name;
        await invocation.ProceedAsync();
      }

      public override async System.Threading.Tasks.ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation) where TResult : default
      {
        ArgumentNullException.ThrowIfNull(invocation);
        WasInvoked = true;
        LastMethodName = invocation.Method.Name;
        return await invocation.ProceedAsync<TResult>();
      }
    }

    #endregion
  }
}
