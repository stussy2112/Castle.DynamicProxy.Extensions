// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateInterfaceProxyWithTargetInterface.cs" company="Karma, LLC">
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
    public void When_options_is_null_CreateInterfaceProxyWithTargetInterface_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      ITestInterface target = CreateTestInterfaceImplementation();
      ProxyGenerationOptions options = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
          target,
          options,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateInterfaceProxyWithTargetInterface_nongeneric_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      ProxyGenerationOptions options = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
          typeof(ITestInterface),
          Type.EmptyTypes,
          target,
          options,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateInterfaceProxyWithTargetInterface_generic_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      ITestInterface target = CreateTestInterfaceImplementation();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateInterfaceProxyWithTargetInterface(target, _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateInterfaceProxyWithTargetInterface_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      object target = CreateTestInterfaceImplementation();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateInterfaceProxyWithTargetInterface(
          typeof(ITestInterface),
          target,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_target_is_null_CreateInterfaceProxyWithTargetInterface_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      object nullTarget = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
          typeof(ITestInterface),
          Type.EmptyTypes,
          nullTarget,
          _defaultOptions,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_target_is_null_CreateInterfaceProxyWithTargetInterface_generic_throws_ArgumentNullException()
    {
      // Arrange
      ITestInterface? nullTarget = null;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(nullTarget, _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_valid_parameters_CreateInterfaceProxyWithTargetInterface_generic_delegates_correctly()
    {
      // Arrange
      ITestInterface target = CreateTestInterfaceImplementation();
      ITestInterface expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTargetInterface(
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      ITestInterface result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_valid_parameters_CreateInterfaceProxyWithTargetInterface_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTargetInterface(
          typeof(ITestInterface),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
        typeof(ITestInterface),
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_additionalInterfaces_CreateInterfaceProxyWithTargetInterface_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      object expectedProxy = new TestInterfaceImplementation();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTargetInterface(
          typeof(ITestInterface),
          additionalInterfaces,
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
        typeof(ITestInterface),
        additionalInterfaces,
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_options_CreateInterfaceProxyWithTargetInterface_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTargetInterface(
          typeof(ITestInterface),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTargetInterface(
        typeof(ITestInterface),
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }
  }
}