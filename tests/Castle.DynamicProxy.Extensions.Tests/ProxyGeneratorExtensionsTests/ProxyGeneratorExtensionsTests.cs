// -----------------------------------------------------------------------
// <copyright file="ProxyGeneratorExtensionsTests.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Castle.DynamicProxy.Extensions.Tests
{
  public interface ITestInterface
  {
    string GetValue();
  }

  /// <summary>
  /// Unit tests for ProxyGeneratorExtensions class.
  /// </summary>
  [ExcludeFromCodeCoverage]
  [TestClass]
  public partial class ProxyGeneratorExtensionsTests
  {
    private ProxyGenerationOptions _defaultOptions = null!;
    private Mock<IAsyncInterceptor> _mockAsyncInterceptor = null!;
    private Mock<IInterceptor> _mockInterceptor = null!;
    private Mock<IProxyGenerator> _mockProxyGenerator = null!;
    private Mock<IServiceProvider> _mockServiceProvider = null!;

    [TestCleanup]
    public void TestCleanup()
    {
      _mockProxyGenerator = null!;
      _mockAsyncInterceptor = null!;
      _mockInterceptor = null!;
      _mockServiceProvider = null!;
      _defaultOptions = null!;
    }

    [TestInitialize]
    public void TestInitialize()
    {
      _mockProxyGenerator = new Mock<IProxyGenerator>();
      _mockAsyncInterceptor = new Mock<IAsyncInterceptor>();
      _mockInterceptor = new Mock<IInterceptor>();
      _mockServiceProvider = new Mock<IServiceProvider>();
      _defaultOptions = ProxyGenerationOptions.Default;
    }

    // Helper methods
    private TestClass CreateTestClassInstance() => new TestClass();

    private ITestInterface CreateTestInterfaceImplementation() => new TestInterfaceImplementation();
  }

  // Test supporting classes
  [ExcludeFromCodeCoverage]
  public class TestClass
  {
    public TestClass()
    {
      // Parameterless constructor for proxy creation
    }

    public virtual string Name { get; set; } = "TestClass";

    public virtual void DoSomething()
    {
      // Test method
    }
  }

  [ExcludeFromCodeCoverage]
  public class TestInterfaceImplementation : ITestInterface
  {
    public string GetValue() => "TestValue";
  }

  // Dependency injection test classes
  public interface ITestDependency
  {
    string GetDependencyValue();
  }

  [ExcludeFromCodeCoverage]
  public class TestDependency : ITestDependency
  {
    /// <summary>
    /// Gets or sets a value indicating whether the dependency was used.
    /// </summary>
    public bool WasUsed { get; set; }

    public string GetDependencyValue() => "Dependency Value";
  }

  [ExcludeFromCodeCoverage]
  public class TestClassWithDependency
  {
    public ITestDependency Dependency { get; }

    public TestClassWithDependency(ITestDependency dependency) => Dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));

    public virtual string GetValue() => Dependency?.GetDependencyValue() ?? string.Empty;
  }

  [ExcludeFromCodeCoverage]
  public class TestClassWithMultipleDependencies
  {
    public ITestDependency Dependency1 { get; }
    public ITestDependency Dependency2 { get; }
    public string StringValue { get; }

    public TestClassWithMultipleDependencies(ITestDependency dependency1, ITestDependency dependency2, string stringValue)
    {
      Dependency1 = dependency1 ?? throw new ArgumentNullException(nameof(dependency1));
      Dependency2 = dependency2 ?? throw new ArgumentNullException(nameof(dependency2));
      StringValue = stringValue ?? throw new ArgumentNullException(nameof(stringValue));
    }

    public virtual string GetCombinedValue() => $"{Dependency1?.GetDependencyValue() ?? string.Empty}, {Dependency2?.GetDependencyValue() ?? string.Empty}, {StringValue}";
  }
}