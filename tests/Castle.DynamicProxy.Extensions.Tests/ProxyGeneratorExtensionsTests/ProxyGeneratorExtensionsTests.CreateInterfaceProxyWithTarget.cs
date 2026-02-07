// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.CreateInterfaceProxyWithTarget.cs" company="Karma, LLC">
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
    public void When_interfaceToProxy_is_null_CreateInterfaceProxyWithTarget_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      Type nullType = null!;
      object target = CreateTestInterfaceImplementation();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
          nullType,
          Type.EmptyTypes,
          target,
          _defaultOptions,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_options_is_null_CreateInterfaceProxyWithTarget_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      ITestInterface target = CreateTestInterfaceImplementation();
      ProxyGenerationOptions options = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
          target,
          options,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateInterfaceProxyWithTarget_generic_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      ITestInterface target = CreateTestInterfaceImplementation();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateInterfaceProxyWithTarget(target, _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_proxyGenerator_is_null_CreateInterfaceProxyWithTarget_nongeneric_throws_ArgumentNullException()
    {
      // Arrange
      IProxyGenerator nullGenerator = null!;
      object target = CreateTestInterfaceImplementation();

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        nullGenerator.CreateInterfaceProxyWithTarget(
          typeof(ITestInterface),
          target,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_target_is_null_CreateInterfaceProxyWithTarget_generic_throws_ArgumentNullException()
    {
      // Arrange
      ITestInterface nullTarget = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(nullTarget, _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_target_is_null_CreateInterfaceProxyWithTarget_nongeneric_comprehensive_throws_ArgumentNullException()
    {
      // Arrange
      object nullTarget = null!;

      // Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() =>
        _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
          typeof(ITestInterface),
          Type.EmptyTypes,
          nullTarget,
          _defaultOptions,
          _mockAsyncInterceptor.Object));
    }

    [TestMethod]
    public void When_valid_parameters_CreateInterfaceProxyWithTarget_generic_delegates_correctly()
    {
      // Arrange
      ITestInterface target = CreateTestInterfaceImplementation();
      ITestInterface expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTarget(
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      ITestInterface result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_valid_parameters_CreateInterfaceProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTarget(
          typeof(ITestInterface),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
        typeof(ITestInterface),
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_additionalInterfaces_CreateInterfaceProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      object expectedProxy = new TestInterfaceImplementation();
      Type[] additionalInterfaces = [typeof(IDisposable)];
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTarget(
          typeof(ITestInterface),
          additionalInterfaces,
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
        typeof(ITestInterface),
        additionalInterfaces,
        target,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void When_with_options_CreateInterfaceProxyWithTarget_nongeneric_delegates_correctly()
    {
      // Arrange
      object target = CreateTestInterfaceImplementation();
      object expectedProxy = new TestInterfaceImplementation();
      _ = _mockProxyGenerator
        .Setup((x) => x.CreateInterfaceProxyWithTarget(
          typeof(ITestInterface),
          Type.EmptyTypes,
          target,
          _defaultOptions,
          It.IsAny<IInterceptor[]>()))
        .Returns(expectedProxy);

      // Act
      object result = _mockProxyGenerator.Object.CreateInterfaceProxyWithTarget(
        typeof(ITestInterface),
        target,
        _defaultOptions,
        _mockAsyncInterceptor.Object);

      // Assert
      Assert.IsNotNull(result);
    }
  }
}