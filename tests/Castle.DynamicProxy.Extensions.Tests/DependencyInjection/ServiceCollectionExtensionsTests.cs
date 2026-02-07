// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensionsTests.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  /// <summary>
  /// Tests for ServiceCollectionExtensions AddIntercepted methods.
  /// </summary>
  [TestClass]
  [ExcludeFromCodeCoverage]
  public partial class ServiceCollectionExtensionsTests
  {
    private ServiceCollection _services = null!;
    private TestInterceptor _interceptor = null!;

    /// <summary>
    /// Initializes test dependencies before each test.
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
      _services = new ServiceCollection();
      _interceptor = new TestInterceptor();
    }

    /// <summary>
    /// Cleans up test dependencies after each test.
    /// </summary>
    [TestCleanup]
    public void TestCleanup()
    {
      _services = null!;
      _interceptor = null!;
    }
  }

  /// <summary>
  /// Test service interface for testing interception.
  /// </summary>
  public interface ITestService
  {
    /// <summary>
    /// Gets or sets the tag associated with the object.
    /// </summary>
    string? Tag { get; }

    /// <summary>
    /// Gets the name of the test service.
    /// </summary>
    /// <returns>The name.</returns>
    string GetName();

    /// <summary>
    /// Gets a value indicating whether the method was called.
    /// </summary>
    bool WasCalled { get; }
  }

  /// <summary>
  /// Test service implementation for testing interception.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class TestService : ITestService
  {
    public TestService()
    {
    }

    public TestService(string tag) => Tag = tag;

    /// <summary>
    /// Gets a value indicating whether the method was called.
    /// </summary>
    public bool WasCalled { get; private set; }

    public string? Tag
    {
      get;
    }

    /// <summary>
    /// Gets the name of the test service.
    /// </summary>
    /// <returns>The name.</returns>
    public string GetName()
    {
      WasCalled = true;
      return "TestService";
    }
  }

  /// <summary>
  /// Test service class with virtual methods for class proxy testing.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class TestClassService
  {
    /// <summary>
    /// Gets a value indicating whether the method was called.
    /// </summary>
    public bool WasCalled { get; private set; }

    /// <summary>
    /// Gets the name of the test service.
    /// </summary>
    /// <returns>The name.</returns>
    public virtual string GetName()
    {
      WasCalled = true;
      return "TestClassService";
    }
  }

  /// <summary>
  /// Test interceptor that tracks interception calls.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class TestInterceptor : IInterceptor
  {
    /// <summary>
    /// Gets a value indicating whether the interceptor was invoked.
    /// </summary>
    public bool WasInvoked { get; private set; }

    /// <summary>
    /// Gets the name of the intercepted method.
    /// </summary>
    public string? InterceptedMethodName { get; private set; }

    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      WasInvoked = true;
      InterceptedMethodName = invocation.Method.Name;
      invocation.Proceed();
    }
  }

  /// <summary>
  /// Second test interceptor for testing interceptor ordering.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public class SecondTestInterceptor : IInterceptor
  {
    /// <summary>
    /// Gets a value indicating whether the interceptor was invoked.
    /// </summary>
    public bool WasInvoked { get; private set; }

    /// <summary>
    /// Intercepts the method invocation.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void Intercept(IInvocation invocation)
    {
      ArgumentNullException.ThrowIfNull(invocation);
      WasInvoked = true;
      invocation.Proceed();
    }
  }
}