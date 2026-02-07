// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateClassProxy.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Castle.DynamicProxy.Extensions.Tests
{
  public partial class ProxyGeneratorExtensionsTests
  {
    [TestMethod]
    public void When_all_parameters_valid_CreateClassProxy_comprehensive_delegates_correctly()
    {
      // Arrange
      var expectedProxy = new TestClass();
      object[] constructorArgs = [42];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy<TestClass>(
          _defaultOptions,
          constructorArgs,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxy<TestClass>(
        _defaultOptions,
        constructorArgs,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
      _mockProxyGenerator.Verify((x) => x.CreateClassProxy<TestClass>(
        _defaultOptions,
        constructorArgs,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    [TestMethod]
    public void When_asyncInterceptors_is_provided_CreateClassProxy_generic_delegates_correctly()
    {
      // Arrange
      var expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy<TestClass>(
          It.IsAny<ProxyGenerationOptions>(),
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxy<TestClass>(_mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
      _mockProxyGenerator.Verify((x) => x.CreateClassProxy<TestClass>(
        ProxyGenerationOptions.Default,
        It.Is<object[]>((arr) => arr.Length == 0),
        It.Is<IInterceptor[]>((arr) => arr.Length == 1)), Times.Once);
    }

    [TestMethod]
    public void When_classToProxy_is_null_CreateClassProxy_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      Type nullType = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxy(
          nullType,
          Type.EmptyTypes,
          _defaultOptions,
          [],
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_constructorArguments_provided_CreateClassProxy_generic_delegates_correctly()
    {
      // Arrange
      var expectedProxy = new TestClass();
      object[] constructorArgs = [42, "test"];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy<TestClass>(
          It.IsAny<ProxyGenerationOptions>(),
          constructorArgs,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxy<TestClass>(
        constructorArgs,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      _mockProxyGenerator.Verify((x) => x.CreateClassProxy<TestClass>(
        ProxyGenerationOptions.Default,
        constructorArgs,
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    [TestMethod]
    public void When_options_is_null_CreateClassProxy_comprehensive_throws_ArgumentNullException() =>
      // Arrange & Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxy<TestClass>(
          null!,
          [],
          _mockAsyncInterceptor.Object));

    [TestMethod]
    public void When_options_is_null_CreateClassProxy_nongeneric_comprehensive_throws_ArgumentNullException() =>
      // Arrange & Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateClassProxy(
          typeof(TestClass),
          Type.EmptyTypes,
          null!,
          [],
          _mockAsyncInterceptor.Object));

    [TestMethod]
    public void When_options_provided_CreateClassProxy_generic_with_options_delegates_correctly()
    {
      // Arrange
      var expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy<TestClass>(
          It.IsAny<ProxyGenerationOptions>(),
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      TestClass result = _mockProxyGenerator.Object.CreateClassProxy<TestClass>(
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      _mockProxyGenerator.Verify((x) => x.CreateClassProxy<TestClass>(
        _defaultOptions,
        It.Is<object[]>((arr) => arr.Length == 0),
        It.IsAny<IInterceptor[]>()), Times.Once);
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxy_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxy<TestClass>(
          _defaultOptions,
          [],
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxy_generic_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxy<TestClass>(_mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateClassProxy_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateClassProxy(typeof(TestClass), _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_valid_parameters_CreateClassProxy_nongeneric_delegates_correctly()
    {
      // Arrange
      object expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy(
          typeof(TestClass),
          Type.EmptyTypes,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxy(
        typeof(TestClass),
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
    }

    [TestMethod]
    public void When_with_additionalInterfaces_CreateClassProxy_nongeneric_delegates_correctly()
    {
      // Arrange
      object expectedProxy = new TestClass();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy(
          typeof(TestClass),
          additionalInterfaces,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxy(
        typeof(TestClass),
        additionalInterfaces,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_options_CreateClassProxy_nongeneric_delegates_correctly()
    {
      // Arrange
      object expectedProxy = new TestClass();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateClassProxy(
          typeof(TestClass),
          Type.EmptyTypes,
          _defaultOptions,
          It.IsAny<object[]>(),
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateClassProxy(
        typeof(TestClass),
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }
  }
}