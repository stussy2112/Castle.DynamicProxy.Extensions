// -----------------------------------------------------------------------
// <copyright file="AsyncInterceptorTests.cs" company="Karma, LLC">
//   Copyright (c) Karma, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.DynamicProxy.Extensions.Tests
{
  [TestClass]
  [ExcludeFromCodeCoverage]
  public class AsyncInterceptorTests
  {
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task When_async_function_intercepted_interceptor_is_callled_Async()
    {
      ITestInterceptedService interceptedService = new TestInterceptedService();
      var generator = new ProxyGenerator();
      var interceptor = new CountingAsyncInterceptor(TestContext);
      ITestInterceptedService proxy = generator.CreateInterfaceProxyWithTargetInterface<ITestInterceptedService>(interceptedService, interceptor);

      Assert.IsNotNull(proxy);

      string action = "Test Action";
      string actual = await proxy.DoFunctionAsync(action);
      Assert.IsNotNull(actual);
      Assert.IsTrue(actual.Contains(action, StringComparison.Ordinal));

      Assert.AreEqual(1, interceptor.InvocationCount);
    }

    public class CountingAsyncInterceptor : AsyncInterceptorBase
    {
      public CountingAsyncInterceptor(TestContext testContext) => TestContext = testContext;

      public int InvocationCount { get; private set; }

      public TestContext TestContext { get; }

      public override void Intercept(IInvocation invocation)
      {
        // Example implementation: Log the method call and proceed with the invocation.
        TestContext.WriteLine($"Intercepting synchronous call to method: {invocation?.Method.Name}");

        InvocationCount++;
        invocation?.Proceed(); // Proceed with the original method call
        // Log the return value or perform any post-processing here
        TestContext.WriteLine($"Completed synchronous  call to {invocation?.Method.Name}");
      }

      public override async ValueTask InterceptAsync(IInvocation invocation)
      {
        if (invocation is null)
        {
          return;
        }

        // Log the method call or perform any pre-processing here
        TestContext.WriteLine($"Intercepting asynchronous call to {invocation.Method.Name}");

        InvocationCount++;
        // Proceed with the original method call
        await invocation.ProceedAsync();

        // Log the return value or perform any post-processing here
        TestContext.WriteLine($"Completed asynchronous call to {invocation.Method.Name}");
      }

      public override async ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation) where TResult : default
      {
        if (invocation is null)
        {
          return default;
        }

        // Log the method call or perform any pre-processing here
        TestContext.WriteLine($"Intercepting asynchronous with result call to {invocation.Method.Name}");

        // Proceed with the original method call
        InvocationCount++;
        TResult? result = await invocation.ProceedAsync<TResult>();

        // Log the return value or perform any post-processing here
        TestContext.WriteLine($"Completed asynchronous with result call to {invocation.Method.Name} - {invocation.ReturnValue} : {result}");

        return result;
      }
    }

    public interface ITestInterceptedService
    {
      Task DoActionAsync(string action);
      Task<string> DoFunctionAsync(string action);
      ValueTask TryDoActionAsync(string action);
      ValueTask<string> TryDoFunctionAsync(string action);
    }

    public class TestInterceptedService : ITestInterceptedService
    {
      public TestInterceptedService() { }

      public async Task DoActionAsync(string action)
      {
        Console.WriteLine($"Executing {action}");

        await Task.Delay(1000);

        Console.WriteLine($"Executed {action}");
      }

      public async Task<string> DoFunctionAsync(string action)
      {
        Console.WriteLine($"Executing Function: {action}");
        await Task.Delay(1000);
        Console.WriteLine($"Executed Function: {action}");

        return $"Completed {action}";
      }

      public async ValueTask TryDoActionAsync(string action)
      {
        Console.WriteLine($"Trying {action}");
        await DoActionAsync(action);
      }

      public async ValueTask<string> TryDoFunctionAsync(string action)
      {
        Console.WriteLine($"Trying Function: {action}");
        return await DoFunctionAsync(action);
      }
    }

    [ExcludeFromCodeCoverage]
    public class ThrowingTestInterceptedService : ITestInterceptedService
    {
      public Task DoActionAsync(string action) => throw new InvalidOperationException("DoActionAsync intentionally throws for testing");

      public Task<string> DoFunctionAsync(string action) => throw new InvalidOperationException("DoFunctionAsync intentionally throws for testing");

      public ValueTask TryDoActionAsync(string action) => throw new InvalidOperationException("TryDoActionAsync intentionally throws for testing");

      public ValueTask<string> TryDoFunctionAsync(string action) => throw new InvalidOperationException("TryDoFunctionAsync intentionally throws for testing");
    }

    [ExcludeFromCodeCoverage]
    public class ReturnValueCapturingInterceptor : AsyncInterceptorBase
    {
      public ReturnValueCapturingInterceptor(TestContext testContext) => TestContext = testContext;

      public object? CapturedReturnValue { get; private set; }
      public TestContext TestContext { get; }

      public override void Intercept(IInvocation invocation)
      {
        if (invocation is null)
        {
          return;
        }

        TestContext.WriteLine($"Intercepting call to {invocation.Method.Name}");

        try
        {
          // Attempt to invoke - this will throw
          invocation.CaptureProceedInfo().Invoke();
        }
        catch (Exception ex)
        {
          TestContext.WriteLine($"Invoke() threw: {ex.Message}");

          // Capture the ReturnValue even though an exception was thrown
          CapturedReturnValue = invocation.ReturnValue;

          TestContext.WriteLine($"ReturnValue after exception: {CapturedReturnValue?.GetType().Name ?? "null"}");

          // Re-throw to propagate the exception
          throw;
        }
      }

      public override ValueTask InterceptAsync(IInvocation invocation)
      {
        if (invocation is null)
        {
          return ValueTask.CompletedTask;
        }

        TestContext.WriteLine($"Intercepting call to {invocation.Method.Name}");

        try
        {
          // Attempt to invoke - this will throw
          return invocation.ProceedAsync();
        }
        catch (Exception ex)
        {
          TestContext.WriteLine($"Invoke() threw: {ex.Message}");

          // Capture the ReturnValue even though an exception was thrown
          CapturedReturnValue = invocation.ReturnValue;

          TestContext.WriteLine($"ReturnValue after exception: {CapturedReturnValue?.GetType().Name ?? "null"}");

          // Re-throw to propagate the exception
          throw;
        }
      }

      public override ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation) where TResult : default
      {
        if (invocation is null)
        {
          return default;
        }

        TestContext.WriteLine($"Intercepting call to {invocation.Method.Name}");

        try
        {
          // Attempt to invoke - this will throw
          return invocation.ProceedAsync<TResult>();
        }
        catch (Exception ex)
        {
          TestContext.WriteLine($"Invoke() threw: {ex.Message}");

          // Capture the ReturnValue even though an exception was thrown
          CapturedReturnValue = invocation.ReturnValue;

          TestContext.WriteLine($"ReturnValue after exception: {CapturedReturnValue?.GetType().Name ?? "null"}");

          // Re-throw to propagate the exception
          throw;
        }
      }
    }
  }
}