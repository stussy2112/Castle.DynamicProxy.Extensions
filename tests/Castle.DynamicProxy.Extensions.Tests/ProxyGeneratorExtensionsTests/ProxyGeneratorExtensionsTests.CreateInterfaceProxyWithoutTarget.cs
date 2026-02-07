// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateInterfaceProxyWithoutTarget.cs" company="Karma, LLC">
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
    public void When_interfaceToProxy_is_null_CreateInterfaceProxyWithoutTarget_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      Type nullType = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget(
          nullType,
          Type.EmptyTypes,
          _defaultOptions,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateInterfaceProxyWithoutTarget_comprehensive_throws_ArgumentNullException() =>
      // Arrange & Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget<ITestInterface>(
          (ProxyGenerationOptions?)null!,
          _mockAsyncInterceptor.Object));

    [TestMethod]
    public void When_options_is_null_CreateInterfaceProxyWithoutTarget_nongeneric_comprehensive_throws_ArgumentNullException() =>
      // Arrange & Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget(
          typeof(ITestInterface),
          Type.EmptyTypes,
          (ProxyGenerationOptions)null!,
          _mockAsyncInterceptor.Object));

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateInterfaceProxyWithoutTarget_generic_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateInterfaceProxyWithoutTarget<ITestInterface>(_mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateInterfaceProxyWithoutTarget_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      Type interfaceToProxy = typeof(ITestInterface);

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateInterfaceProxyWithoutTarget(
          interfaceToProxy,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_single_interceptor_CreateInterfaceProxyWithoutTarget_generic_delegates_correctly()
    {
      // Arrange
      ITestInterface expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithoutTarget<ITestInterface>(
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      ITestInterface result = _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget<ITestInterface>(
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_single_interceptor_CreateInterfaceProxyWithoutTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      Type interfaceToProxy = typeof(ITestInterface);

      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithoutTarget(
          interfaceToProxy,
          Type.EmptyTypes,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget(
        interfaceToProxy,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      _mockProxyGenerator.Verify((x) => x.CreateInterfaceProxyWithoutTarget(
        interfaceToProxy,
        Type.EmptyTypes,
        _defaultOptions,
        It.Is<IInterceptor[]>((arr) => arr.Length == 1)), Times.Once);
    }

    [TestMethod]
    public void When_valid_parameters_CreateInterfaceProxyWithoutTarget_generic_delegates_correctly()
    {
      // Arrange
      ITestInterface expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithoutTarget<ITestInterface>(
          ProxyGenerationOptions.Default,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      ITestInterface result = _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget<ITestInterface>(
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(expectedProxy, result);
    }

    [TestMethod]
    public void When_valid_parameters_CreateInterfaceProxyWithoutTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      Type interfaceToProxy = typeof(ITestInterface);

      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithoutTarget(
          interfaceToProxy,
          Type.EmptyTypes,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget(
        interfaceToProxy,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_additionalInterfaces_CreateInterfaceProxyWithoutTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object expectedProxy = new TestInterfaceImplementation();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithoutTarget(
          typeof(ITestInterface),
          additionalInterfaces,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget(
        typeof(ITestInterface),
        additionalInterfaces,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_options_CreateInterfaceProxyWithoutTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithoutTarget(
          typeof(ITestInterface),
          Type.EmptyTypes,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithoutTarget(
        typeof(ITestInterface),
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }
  }
}