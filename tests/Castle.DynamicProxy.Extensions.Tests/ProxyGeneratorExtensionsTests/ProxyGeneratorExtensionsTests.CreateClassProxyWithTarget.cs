// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateClassProxyWithTarget.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Castle.DynamicProxy.Extensions.Tests
{
  public partial class ProxyGeneratorExtensionsTests
  {
    [TestMethod]
    public void When_all_valid_CreateClassProxyWithTarget_comprehensive_delegates_correctly()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();
      var expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        target,
        _defaultOptions,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    [TestMethod]
    public void When_classToProxy_is_null_CreateClassProxyWithTarget_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      Type nullType = null!;
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          nullType!,
          Type.EmptyTypes,
          target,
          _defaultOptions,
          [],
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateClassProxyWithTarget_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();
      ProxyGenerationOptions options = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          target,
          options,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateClassProxyWithTarget_nongeneric_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          null!,
          [],
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxyWithTarget_generic_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      TestClass target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxyWithTarget(target, _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxyWithTarget_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxyWithTarget(typeof(TestClass), target, _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_target_provided_CreateClassProxyWithTarget_generic_delegates_correctly()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();
      var expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          target,
          ProxyGenerationOptions.Default,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
    }

    [TestMethod]
    public void When_valid_parameters_CreateClassProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_additionalInterfaces_and_options_CreateClassProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          additionalInterfaces,
          target,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        additionalInterfaces,
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_additionalInterfaces_CreateClassProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          additionalInterfaces,
          target,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        additionalInterfaces,
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_constructorArguments_CreateClassProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();
      object[] constructorArgs = [42, "test"];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          constructorArgs,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        target,
        constructorArgs,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_options_and_constructorArguments_CreateClassProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();
      object[] constructorArgs = [42];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          constructorArgs,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        target,
        _defaultOptions,
        constructorArgs,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_options_CreateClassProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    // ===== Tests for IServiceProvider overloads =====
    // Note: These methods use ActivatorUtilities.CreateInstance internally,
    // which makes them more integration-level rather than pure unit tests.
    // We focus on parameter validation here.

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxyWithTarget_with_serviceProvider_generic_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      TestClass target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxyWithTarget(_mockServiceProvider.Object, target, _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_serviceProvider_is_null_CreateClassProxyWithTarget_with_serviceProvider_generic_throws_ArgumentNullException()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget<TestClass>(null!, target, _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateClassProxyWithTarget_with_serviceProvider_generic_throws_ArgumentNullException()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          _mockServiceProvider.Object,
          target,
          (ProxyGenerationOptions)null!,
          _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxyWithTarget_with_serviceProvider_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxyWithTarget(_mockServiceProvider.Object, typeof(TestClass), target, _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_serviceProvider_is_null_CreateClassProxyWithTarget_with_serviceProvider_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(null!, typeof(TestClass), target, _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_classToProxy_is_null_CreateClassProxyWithTarget_with_serviceProvider_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(_mockServiceProvider.Object, null!, target, _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateClassProxyWithTarget_with_serviceProvider_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          _mockServiceProvider.Object,
          typeof(TestClass),
          target,
          (ProxyGenerationOptions)null!,
          _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateClassProxyWithTarget_with_serviceProvider_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          _mockServiceProvider.Object,
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          null!,
          [],
          _mockInterceptor.Object));
    }

    [TestMethod]
    public void When_classToProxy_is_null_CreateClassProxyWithTarget_with_serviceProvider_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestClassInstance();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxyWithTarget(
          _mockServiceProvider.Object,
          null!,
          Type.EmptyTypes,
          target,
          _defaultOptions,
          [],
          _mockInterceptor.Object));
    }

    // ===== Integration tests for IServiceProvider dependency resolution =====

    [TestMethod]
    public void When_dependencies_registered_in_serviceProvider_CreateClassProxyWithTarget_resolves_dependencies()
    {
      // Arrange
      var realProxyGenerator = new ProxyGenerator();
      var dependency = new TestDependency();
      IServiceCollection serviceCollection = new ServiceCollection().AddSingleton<ITestDependency>(dependency);
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

      var target = new TestClassWithDependency(dependency);

      // Use a pass-through interceptor that calls Proceed
      var mockInterceptor = new Mock<IInterceptor>();
      _ = mockInterceptor.Setup((x) => x.Intercept(It.IsAny<IInvocation>()))
        .Callback<IInvocation>((invocation) => invocation.Proceed());

      // Act
      TestClassWithDependency proxy = realProxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        mockInterceptor.Object);

      // Assert
      Assert.IsNotNull(proxy);
      _ = Assert.IsInstanceOfType<TestClassWithDependency>(proxy);
      TestClassWithDependency proxyInstance = proxy;
      Assert.IsNotNull(proxyInstance.Dependency);
      Assert.AreEqual("Dependency Value", proxyInstance.GetValue());
    }

    [TestMethod]
    public void When_dependencies_not_in_serviceProvider_CreateClassProxyWithTarget_uses_constructorArguments()
    {
      // Arrange
      var realProxyGenerator = new ProxyGenerator();
      IServiceCollection serviceCollection = new ServiceCollection();
      // Intentionally NOT registering ITestDependency
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

      var dependency = new TestDependency();
      var target = new TestClassWithDependency(dependency);

      // Use a pass-through interceptor
      var mockInterceptor = new Mock<IInterceptor>();
      _ = mockInterceptor.Setup((x) => x.Intercept(It.IsAny<IInvocation>()))
        .Callback<IInvocation>((invocation) => invocation.Proceed());

      // Act - Pass dependency as constructor argument
      object proxy = realProxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClassWithDependency),
        target,
        [dependency],
        mockInterceptor.Object);

      // Assert
      Assert.IsNotNull(proxy);
      _ = Assert.IsInstanceOfType<TestClassWithDependency>(proxy);
      var proxyInstance = (TestClassWithDependency)proxy;
      Assert.IsNotNull(proxyInstance.Dependency);
      Assert.AreEqual("Dependency Value", proxyInstance.GetValue());
    }

    [TestMethod]
    public void When_mixed_dependencies_CreateClassProxyWithTarget_resolves_from_both_sources()
    {
      // Arrange
      var realProxyGenerator = new ProxyGenerator();
      var dependency1 = new TestDependency();
      // Register first dependency in service provider
      IServiceCollection serviceCollection = new ServiceCollection().AddSingleton<ITestDependency>(dependency1);
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

      // Second dependency and string value passed as constructor arguments
      var dependency2 = new TestDependency();
      const string stringValue = "Test String";
      var target = new TestClassWithMultipleDependencies(dependency1, dependency2, stringValue);

      // Use a pass-through interceptor
      var mockInterceptor = new Mock<IInterceptor>();
      _ = mockInterceptor.Setup((x) => x.Intercept(It.IsAny<IInvocation>()))
        .Callback<IInvocation>((invocation) => invocation.Proceed());

      // Act - dependency1 from service provider, dependency2 and string from constructor args
      object proxy = realProxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        typeof(TestClassWithMultipleDependencies),
        target,
        [dependency2, stringValue],
        mockInterceptor.Object);

      // Assert
      Assert.IsNotNull(proxy);
      _ = Assert.IsInstanceOfType<TestClassWithMultipleDependencies>(proxy);
      var proxyInstance = (TestClassWithMultipleDependencies)proxy;
      Assert.IsNotNull(proxyInstance.Dependency1);
      Assert.IsNotNull(proxyInstance.Dependency2);
      Assert.AreEqual(stringValue, proxyInstance.StringValue);
      Assert.AreEqual("Dependency Value, Dependency Value, Test String", proxyInstance.GetCombinedValue());
    }

    [TestMethod]
    public void When_dependencies_registered_CreateClassProxyWithTarget_generic_resolves_dependencies()
    {
      // Arrange
      var realProxyGenerator = new ProxyGenerator();
      var dependency = new TestDependency();
      IServiceCollection serviceCollection = new ServiceCollection().AddSingleton<ITestDependency>(dependency);
      IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

      var target = new TestClassWithDependency(dependency);

      // Use a pass-through interceptor
      var mockInterceptor = new Mock<IInterceptor>();
      _ = mockInterceptor.Setup((x) => x.Intercept(It.IsAny<IInvocation>()))
        .Callback<IInvocation>((invocation) => invocation.Proceed());

      // Act
      TestClassWithDependency proxy = realProxyGenerator.CreateClassProxyWithTarget(
        serviceProvider,
        target,
        mockInterceptor.Object);

      // Assert
      Assert.IsNotNull(proxy);
      Assert.IsNotNull(proxy.Dependency);
      Assert.AreEqual("Dependency Value", proxy.GetValue());
    }
  }
}