// -----------------------------------------------------------------------
// <copyright file="AsyncInterceptorProcessorTests.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  [ExcludeFromCodeCoverage]
  [TestClass]
  public class AsyncInterceptorProcessorTests
  {
    private TestAsyncInterceptor _testInterceptor = null!;
    private AsyncInterceptorProcessor _sut = null!;
    private ProxyGenerator _proxyGenerator = null!;

    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void TestInitialize()
    {
      _testInterceptor = new TestAsyncInterceptor(TestContext);
      _sut = new AsyncInterceptorProcessor(_testInterceptor);
      _proxyGenerator = new ProxyGenerator();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      _testInterceptor = null!;
      _sut = null!;
      _proxyGenerator = null!;
    }

    [TestMethod]
    public void When_interceptor_is_null_Constructor_throws_ArgumentNullException() =>
      // Arrange & Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => new AsyncInterceptorProcessor(null!));

    [TestMethod]
    public void When_invocation_is_null_Intercept_throws_ArgumentNullException() =>
      // Arrange & Act & Assert
      _ = Assert.ThrowsExactly<ArgumentNullException>(() => _sut.Intercept(null!));

    [TestMethod]
    public void When_Task_method_intercepted_InterceptAsync_is_called()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      Task result = proxy.DoActionAsync("test");
      result.GetAwaiter().GetResult();

      // Assert
      Assert.AreEqual(1, _testInterceptor.InterceptAsyncCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptAsyncGenericCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptCallCount);
    }

    [TestMethod]
    public void When_Task_of_T_method_intercepted_InterceptAsync_generic_is_called()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      Task<string> result = proxy.GetValueAsync("test");
      string value = result.GetAwaiter().GetResult();

      // Assert
      Assert.AreEqual(0, _testInterceptor.InterceptAsyncCallCount);
      Assert.AreEqual(1, _testInterceptor.InterceptAsyncGenericCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptCallCount);
      Assert.IsNotNull(value);
      Assert.AreEqual("test-result", value);
    }

    [TestMethod]
    public void When_ValueTask_method_intercepted_InterceptAsync_is_called()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      ValueTask result = proxy.DoValueTaskActionAsync("test");
      result.AsTask().GetAwaiter().GetResult();

      // Assert
      Assert.AreEqual(1, _testInterceptor.InterceptAsyncCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptAsyncGenericCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptCallCount);
    }

    [TestMethod]
    public void When_ValueTask_of_T_method_intercepted_InterceptAsync_generic_is_called()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      ValueTask<int> result = proxy.GetValueTaskValueAsync(42);
      int value = result.AsTask().GetAwaiter().GetResult();

      // Assert
      Assert.AreEqual(0, _testInterceptor.InterceptAsyncCallCount);
      Assert.AreEqual(1, _testInterceptor.InterceptAsyncGenericCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptCallCount);
      Assert.AreEqual(84, value);
    }

    [TestMethod]
    public void When_synchronous_method_intercepted_Intercept_is_called()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      proxy.DoSyncAction();

      // Assert
      Assert.AreEqual(0, _testInterceptor.InterceptAsyncCallCount);
      Assert.AreEqual(0, _testInterceptor.InterceptAsyncGenericCallCount);
      Assert.AreEqual(1, _testInterceptor.InterceptCallCount);
    }

    [TestMethod]
    public void When_Task_method_intercepted_ReturnValue_is_Task()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      Task result = proxy.DoActionAsync("test");
      result.GetAwaiter().GetResult();

      // Assert
      Assert.IsNotNull(_testInterceptor.LastInvocation);
      _ = Assert.IsInstanceOfType<Task>(_testInterceptor.LastInvocation.ReturnValue);
    }

    [TestMethod]
    public void When_Task_of_T_method_intercepted_ReturnValue_is_Task_of_T()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      Task<string> result = proxy.GetValueAsync("test");
      _ = result.GetAwaiter().GetResult();

      // Assert
      Assert.IsNotNull(_testInterceptor.LastInvocation);
      _ = Assert.IsInstanceOfType<Task<string>>(_testInterceptor.LastInvocation.ReturnValue);
    }

    [TestMethod]
    public void When_ValueTask_method_intercepted_ReturnValue_is_ValueTask()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      ValueTask result = proxy.DoValueTaskActionAsync("test");
      result.AsTask().GetAwaiter().GetResult();

      // Assert
      Assert.IsNotNull(_testInterceptor.LastInvocation);
      _ = Assert.IsInstanceOfType<ValueTask>(_testInterceptor.LastInvocation.ReturnValue);
    }

    [TestMethod]
    public void When_ValueTask_of_T_method_intercepted_ReturnValue_is_ValueTask_of_T()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      ValueTask<int> result = proxy.GetValueTaskValueAsync(42);
      _ = result.AsTask().GetAwaiter().GetResult();

      // Assert
      Assert.IsNotNull(_testInterceptor.LastInvocation);
      _ = Assert.IsInstanceOfType<ValueTask<int>>(_testInterceptor.LastInvocation.ReturnValue);
    }

    [TestMethod]
    public void When_exception_thrown_in_InterceptAsync_ReturnValue_is_faulted_Task()
    {
      // Arrange
      var throwingInterceptor = new ThrowingAsyncInterceptor(new InvalidOperationException("Test exception"));
      var processor = new AsyncInterceptorProcessor(throwingInterceptor);
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, processor);

      // Act
      Task result = proxy.DoActionAsync("test");

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.IsFaulted);
    }

    [TestMethod]
    public void When_exception_thrown_in_InterceptAsync_generic_ReturnValue_is_faulted_Task_of_T()
    {
      // Arrange
      var throwingInterceptor = new ThrowingAsyncInterceptor(new InvalidOperationException("Test exception"));
      var processor = new AsyncInterceptorProcessor(throwingInterceptor);
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, processor);

      // Act
      Task<string> result = proxy.GetValueAsync("test");

      // Assert
      Assert.IsNotNull(result);
      Assert.IsTrue(result.IsFaulted);
    }

    [TestMethod]
    public void When_same_return_type_intercepted_multiple_times_handler_is_cached()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act - Call multiple methods with Task<string> return type
      Task<string> result1 = proxy.GetValueAsync("test1");
      Task<string> result2 = proxy.GetValueAsync("test2");
      Task<string> result3 = proxy.GetAnotherValueAsync("test3");

      _ = result1.GetAwaiter().GetResult();
      _ = result2.GetAwaiter().GetResult();
      _ = result3.GetAwaiter().GetResult();

      // Assert - All three calls should use InterceptAsync<TResult>
      Assert.AreEqual(3, _testInterceptor.InterceptAsyncGenericCallCount);
    }

    [TestMethod]
    public void When_different_return_types_intercepted_separate_handlers_are_created()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);

      // Act
      Task<string> stringResult = proxy.GetValueAsync("test");
      Task<int> intResult = proxy.GetIntValueAsync(42);
      ValueTask<bool> boolResult = proxy.GetBoolValueAsync(true);

      _ = stringResult.GetAwaiter().GetResult();
      _ = intResult.GetAwaiter().GetResult();
      _ = boolResult.AsTask().GetAwaiter().GetResult();

      // Assert - All should use generic interceptor
      Assert.AreEqual(3, _testInterceptor.InterceptAsyncGenericCallCount);
    }

    [TestMethod]
    public void When_Task_method_throws_exception_is_captured_in_ReturnValue()
    {
      // Arrange
      var throwingService = new ThrowingTestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(throwingService, _sut);

      // Act
      Task result = proxy.DoActionAsync("test");

      // Assert
      Assert.IsNotNull(result);
      InvalidOperationException exception = Assert.ThrowsExactly<InvalidOperationException>(() => result.GetAwaiter().GetResult());
      Assert.AreEqual("DoActionAsync throws", exception.Message);
    }

    [TestMethod]
    public void When_ValueTask_method_throws_exception_is_captured_in_ReturnValue()
    {
      // Arrange
      var throwingService = new ThrowingTestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(throwingService, _sut);

      // Act
      ValueTask result = proxy.DoValueTaskActionAsync("test");

      // Assert
      InvalidOperationException exception = Assert.ThrowsExactly<InvalidOperationException>(() => result.AsTask().GetAwaiter().GetResult());
      Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void When_null_result_returned_from_InterceptAsync_generic_ReturnValue_is_null()
    {
      // Arrange
      var nullReturningInterceptor = new NullReturningAsyncInterceptor();
      var processor = new AsyncInterceptorProcessor(nullReturningInterceptor);
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, processor);

      // Act
      Task<string> result = proxy.GetValueAsync("test");
      string value = result.GetAwaiter().GetResult();

      // Assert
      Assert.IsNull(value);
    }

    [TestMethod]
    public void When_multiple_threads_call_same_method_type_handler_is_thread_safe()
    {
      // Arrange
      ITestService service = new TestService();
      ITestService proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface<ITestService>(service, _sut);
      System.Collections.Generic.List<Task> tasks = [];

      // Act - Create multiple concurrent calls
      for (int i = 0; i < 10; i++)
      {
        var task = Task.Run(() =>
        {
          Task<string> result = proxy.GetValueAsync($"test-{i}");
          _ = result.GetAwaiter().GetResult();
        }, TestContext.CancellationToken);
        tasks.Add(task);
      }

      Task.WaitAll([.. tasks]);

      // Assert - All calls should have been handled successfully
      Assert.AreEqual(10, _testInterceptor.InterceptAsyncGenericCallCount);
    }

    public interface ITestService
    {
      Task DoActionAsync(string value);
      Task<string> GetValueAsync(string value);
      Task<string> GetAnotherValueAsync(string value);
      Task<int> GetIntValueAsync(int value);
      ValueTask DoValueTaskActionAsync(string value);
      ValueTask<int> GetValueTaskValueAsync(int value);
      ValueTask<bool> GetBoolValueAsync(bool value);
      void DoSyncAction();
    }

    [ExcludeFromCodeCoverage]
    public class TestService : ITestService
    {
      public Task DoActionAsync(string value) => Task.CompletedTask;

      public Task<string> GetValueAsync(string value) => Task.FromResult($"{value}-result");

      public Task<string> GetAnotherValueAsync(string value) => Task.FromResult($"{value}-another");

      public Task<int> GetIntValueAsync(int value) => Task.FromResult(value * 2);

      public ValueTask DoValueTaskActionAsync(string value) => ValueTask.CompletedTask;

      public ValueTask<int> GetValueTaskValueAsync(int value) => ValueTask.FromResult(value * 2);

      public ValueTask<bool> GetBoolValueAsync(bool value) => ValueTask.FromResult(!value);

      public void DoSyncAction() { }
    }

    [ExcludeFromCodeCoverage]
    public class ThrowingTestService : ITestService
    {
      public Task DoActionAsync(string value) => throw new InvalidOperationException("DoActionAsync throws");

      public Task<string> GetValueAsync(string value) => throw new InvalidOperationException("GetValueAsync throws");

      public Task<string> GetAnotherValueAsync(string value) => throw new InvalidOperationException("GetAnotherValueAsync throws");

      public Task<int> GetIntValueAsync(int value) => throw new InvalidOperationException("GetIntValueAsync throws");

      public ValueTask DoValueTaskActionAsync(string value) => throw new InvalidOperationException("DoValueTaskActionAsync throws");

      public ValueTask<int> GetValueTaskValueAsync(int value) => throw new InvalidOperationException("GetValueTaskValueAsync throws");

      public ValueTask<bool> GetBoolValueAsync(bool value) => throw new InvalidOperationException("GetBoolValueAsync throws");

      public void DoSyncAction() => throw new InvalidOperationException("DoSyncAction throws");
    }

    [ExcludeFromCodeCoverage]
    public class TestAsyncInterceptor : IAsyncInterceptor
    {
      private readonly TestContext _testContext;

      public TestAsyncInterceptor(TestContext testContext) => _testContext = testContext;

      public int InterceptCallCount { get; private set; }

      public int InterceptAsyncCallCount { get; private set; }

      public int InterceptAsyncGenericCallCount { get; private set; }

      public IInvocation LastInvocation { get; private set; } = null!;

      public void Intercept(IInvocation invocation)
      {
        ArgumentNullException.ThrowIfNull(invocation);

        InterceptCallCount++;
        LastInvocation = invocation;
        _testContext?.WriteLine($"Intercept called for {invocation!.Method.Name}");
        invocation.Proceed();
      }

      public ValueTask InterceptAsync(IInvocation invocation)
      {
        ArgumentNullException.ThrowIfNull(invocation);

        InterceptAsyncCallCount++;
        LastInvocation = invocation;
        _testContext?.WriteLine($"InterceptAsync called for {invocation.Method.Name}");
        return invocation.ProceedAsync();
      }

      public ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation)
      {
        ArgumentNullException.ThrowIfNull(invocation);

        InterceptAsyncGenericCallCount++;
        LastInvocation = invocation;
        _testContext?.WriteLine($"InterceptAsync<{typeof(TResult).Name}> called for {invocation.Method.Name}");
        return invocation.ProceedAsync<TResult>();
      }
    }

    [ExcludeFromCodeCoverage]
    public class ThrowingAsyncInterceptor : IAsyncInterceptor
    {
      private readonly Exception _exceptionToThrow;

      public ThrowingAsyncInterceptor(Exception exceptionToThrow) => _exceptionToThrow = exceptionToThrow;

      public void Intercept(IInvocation invocation) => throw _exceptionToThrow;

      public ValueTask InterceptAsync(IInvocation invocation) => throw _exceptionToThrow;

      public ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation) => throw _exceptionToThrow;
    }

    [ExcludeFromCodeCoverage]
    public class NullReturningAsyncInterceptor : IAsyncInterceptor
    {
      public void Intercept(IInvocation invocation) => invocation?.Proceed();

      public ValueTask InterceptAsync(IInvocation invocation) => ValueTask.CompletedTask;

      public ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation) => ValueTask.FromResult<TResult?>(default);
    }
  }
}