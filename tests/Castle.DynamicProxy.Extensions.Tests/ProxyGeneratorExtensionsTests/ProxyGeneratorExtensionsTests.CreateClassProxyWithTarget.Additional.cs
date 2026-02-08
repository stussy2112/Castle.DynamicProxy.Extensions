// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateClassProxyWithTarget.Additional.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Additional tests for CreateClassProxyWithTarget extension methods to ensure 100% coverage.
  /// </summary>
  public partial class ProxyGeneratorExtensionsTests
  {
    #region Generic CreateClassProxyWithTarget<TClass> Tests

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with target and no options uses default options.
    /// </summary>
    [TestMethod]
    public void When_target_provided_CreateClassProxyWithTarget_generic_simple_overload_uses_default_options()
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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        target,
        ProxyGenerationOptions.Default,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget converts async interceptors to regular interceptors.
    /// </summary>
    [TestMethod]
    public void When_async_interceptors_provided_CreateClassProxyWithTarget_generic_converts_to_interceptors()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();
      var expectedProxy = new TestClass();
      IInterceptor? capturedInterceptor = null;

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Callback<TestClass, ProxyGenerationOptions, IInterceptor[]>((t, o, interceptors) => capturedInterceptor = interceptors.Length > 0 ? interceptors[0] : null)
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.IsNotNull(capturedInterceptor);
      _ = Assert.IsInstanceOfType<IInterceptor>(capturedInterceptor);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with null target still creates proxy.
    /// </summary>
    [TestMethod]
    public void When_null_target_CreateClassProxyWithTarget_generic_creates_proxy()
    {
      // Arrange
      TestClass? nullTarget = null;
      var expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          nullTarget,
          ProxyGenerationOptions.Default,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        nullTarget,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
    }

    #endregion

    #region Non-Generic CreateClassProxyWithTarget(Type) Tests

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with type and additional interfaces delegates correctly.
    /// </summary>
    [TestMethod]
    public void When_type_and_additionalInterfaces_provided_CreateClassProxyWithTarget_delegates_with_default_options()
    {
      // Arrange
      object target = CreateTestClassInstance();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      object expectedProxy = new TestClass();

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          additionalInterfaces,
          target,
          ProxyGenerationOptions.Default,
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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        additionalInterfaces,
        target,
        ProxyGenerationOptions.Default,
        It.Is<object[]>((a) => a.Length == 0),
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with type and options and constructorArgs passes empty interfaces array.
    /// </summary>
    [TestMethod]
    public void When_type_options_constructorArgs_provided_CreateClassProxyWithTarget_passes_empty_interfaces()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object[] constructorArgs = [42];
      object expectedProxy = new TestClass();

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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        Type.EmptyTypes,
        target,
        _defaultOptions,
        constructorArgs,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with type and constructorArgs uses default options.
    /// </summary>
    [TestMethod]
    public void When_type_target_constructorArgs_provided_CreateClassProxyWithTarget_uses_default_options()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object[] constructorArgs = [42, "test"];
      object expectedProxy = new TestClass();

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          ProxyGenerationOptions.Default,
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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        Type.EmptyTypes,
        target,
        ProxyGenerationOptions.Default,
        constructorArgs,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with type only uses empty types and default options.
    /// </summary>
    [TestMethod]
    public void When_type_target_only_provided_CreateClassProxyWithTarget_uses_empty_types_and_default_options()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object expectedProxy = new TestClass();

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          ProxyGenerationOptions.Default,
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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        Type.EmptyTypes,
        target,
        ProxyGenerationOptions.Default,
        It.Is<object[]>((a) => a.Length == 0),
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with type and options passes empty types and args.
    /// </summary>
    [TestMethod]
    public void When_type_target_options_provided_CreateClassProxyWithTarget_passes_empty_types_and_args()
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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        Type.EmptyTypes,
        target,
        _defaultOptions,
        It.Is<object[]>((a) => a.Length == 0),
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that CreateClassProxyWithTarget with additionalInterfaces and options passes empty args.
    /// </summary>
    [TestMethod]
    public void When_additionalInterfaces_options_provided_CreateClassProxyWithTarget_passes_empty_args()
    {
      // Arrange
      object target = CreateTestClassInstance();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      object expectedProxy = new TestClass();

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
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        additionalInterfaces,
        target,
        _defaultOptions,
        It.Is<object[]>((a) => a.Length == 0),
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests the most comprehensive overload with all parameters.
    /// </summary>
    [TestMethod]
    public void When_all_parameters_provided_CreateClassProxyWithTarget_comprehensive_passes_all_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      Type[] additionalInterfaces = [typeof(IDisposable), typeof(ICloneable)];
      object[] constructorArgs = [42, "test", true];
      object expectedProxy = new TestClass();

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          additionalInterfaces,
          target,
          _defaultOptions,
          constructorArgs,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        additionalInterfaces,
        target,
        _defaultOptions,
        constructorArgs,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      _mockProxyGenerator.Verify((x) => x.CreateClassProxyWithTarget(
        typeof(TestClass),
        additionalInterfaces,
        target,
        _defaultOptions,
        constructorArgs,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    /// <summary>
    /// Tests that null additionalInterfaces is handled correctly.
    /// </summary>
    [TestMethod]
    public void When_null_additionalInterfaces_CreateClassProxyWithTarget_handles_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      Type[]? nullInterfaces = null;
      object expectedProxy = new TestClass();

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          nullInterfaces,
          target,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        nullInterfaces,
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    /// <summary>
    /// Tests that null constructorArguments is handled correctly.
    /// </summary>
    [TestMethod]
    public void When_null_constructorArguments_CreateClassProxyWithTarget_handles_correctly()
    {
      // Arrange
      object target = CreateTestClassInstance();
      object[]? nullArgs = null;
      object expectedProxy = new TestClass();

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          typeof(TestClass),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          nullArgs,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        typeof(TestClass),
        target,
        _defaultOptions,
        nullArgs,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    #endregion

    #region Multiple Async Interceptors Tests

    /// <summary>
    /// Tests that multiple async interceptors are correctly converted.
    /// </summary>
    [TestMethod]
    public void When_multiple_async_interceptors_CreateClassProxyWithTarget_converts_all()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();
      var expectedProxy = new TestClass();
      var mockAsyncInterceptor2 = new Mock<IAsyncInterceptor>();
      var mockAsyncInterceptor3 = new Mock<IAsyncInterceptor>();
      IInterceptor[]? capturedInterceptors = null;

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Callback<TestClass, ProxyGenerationOptions, IInterceptor[]>((t, o, interceptors) => capturedInterceptors = interceptors)
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object,
        mockAsyncInterceptor2.Object,
        mockAsyncInterceptor3.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.IsNotNull(capturedInterceptors);
      Assert.HasCount(3, capturedInterceptors);
    }

    /// <summary>
    /// Tests that empty async interceptors array is handled correctly.
    /// </summary>
    [TestMethod]
    public void When_empty_async_interceptors_CreateClassProxyWithTarget_handles_correctly()
    {
      // Arrange
      TestClass target = CreateTestClassInstance();
      var expectedProxy = new TestClass();
      IInterceptor[]? capturedInterceptors = null;

      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxyWithTarget(
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Callback<TestClass, ProxyGenerationOptions, IInterceptor[]>((t, o, interceptors) => capturedInterceptors = interceptors)
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxyWithTarget(
        target,
        _defaultOptions);

      // Assert
      Assert.IsNotNull(result);
      Assert.IsNotNull(capturedInterceptors);
      Assert.IsEmpty(capturedInterceptors);
    }

    #endregion
  }
}
