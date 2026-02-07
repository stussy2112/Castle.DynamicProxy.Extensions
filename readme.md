# Castle.DynamicProxy.Extensions
[![Build Status](https://github.com/stussy2112/Castle.DynamicProxy.Extensions/workflows/CI/badge.svg)](https://github.com/stussy2112/Castle.DynamicProxy.Extensions/actions)
[![NuGet](https://img.shields.io/nuget/v/Castle.DynamicProxy.Extensions.svg)](https://www.nuget.org/packages/Castle.DynamicProxy.Extensions/)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-GNU-green.svg)](LICENSE)

A powerful extension library for Castle.DynamicProxy that provides seamless integration with Microsoft.Extensions.DependencyInjection and adds comprehensive support for asynchronous method interception.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
  - [Basic Interception with DI](#basic-interception-with-di)
  - [Async Interception](#async-interception)
  - [Keyed Services](#keyed-services)
- [API Reference](#api-reference)
- [Dependency Injection Extensions](#dependency-injection-extensions)
- [Async Interceptor Support](#async-interceptor-support)
  - [ProceedAsync Extension Methods](#proceedasync-extension-methods)
- [ProxyGenerator Extensions](#proxygenerator-extensions)
- [Advanced Scenarios](#advanced-scenarios)
- [Async Method Interception Best Practices](#async-method-interception-best-practices)
- [Multiple Interceptors](#multiple-interceptors)
- [Factory-Based Registration](#factory-based-registration)
- [Try-Add Pattern](#try-add-pattern)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Overview

Castle.DynamicProxy.Extensions simplifies the integration of aspect-oriented programming (AOP) patterns into .NET applications by providing:

- **Seamless DI Integration**: Extension methods for `IServiceCollection` that enable intercepted service registration
- **Async/Await Support**: Built-in support for intercepting asynchronous methods with `IAsyncInterceptor`
- **Flexible Registration**: Support for Transient, Scoped, and Singleton lifetimes with both regular and keyed services
- **Type-Safe API**: Strongly-typed extension methods with full IntelliSense support
- **.NET 10 Ready**: Built for the latest .NET platform with nullable reference types and C# 14 features

## Features

✅ **Dependency Injection Extensions**
- Register services with interceptors directly in `IServiceCollection`
- Support for all service lifetimes (Transient, Scoped, Singleton)
- Keyed service registration with interception
- Try-Add pattern support to prevent duplicate registrations

✅ **Asynchronous Interception**
- `IAsyncInterceptor` interface for async method interception
- `AsyncInterceptorBase` abstract class for easy implementation
- Support for `Task`, `ValueTask`, and generic return types

✅ **ProxyGenerator Extensions**
- Simplified proxy creation methods
- Support for class proxies, interface proxies, and target proxies
- Integration with `IServiceProvider` for dependency resolution

✅ **.NET 10 Features**
- Nullable reference types
- Latest C# language features
- `DynamicallyAccessedMembers` attributes for Native AOT compatibility

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package Castle.DynamicProxy.Extensions
```

Or via Package Manager Console:

```powershell
Install-Package Castle.DynamicProxy.Extensions
```

Or add directly to your `.csproj` file:

```xml
<PackageReference Include="Castle.DynamicProxy.Extensions" Version="1.0.0" />
```

## Quick Start

### Basic Interception with DI

Register a service with interceptors in your DI container:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;

// Create a logging interceptor
public class LoggingInterceptor : IInterceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        _logger.LogInformation("Calling method: {Method}", invocation.Method.Name);
        invocation.Proceed();
        _logger.LogInformation("Method completed: {Method}", invocation.Method.Name);
    }
}

// Register services with interception
services.AddSingleton<LoggingInterceptor>();
services.AddInterceptedTransient<IMyService, MyService>(typeof(LoggingInterceptor));

// Or with instance interceptors
var loggingInterceptor = new LoggingInterceptor(logger);
services.AddInterceptedSingleton<IMyService, MyService>(loggingInterceptor);
```

### Async Interception

Implement async method interception:

```csharp
using Castle.DynamicProxy.Extensions;

public class AsyncLoggingInterceptor : AsyncInterceptorBase
{
    private readonly ILogger<AsyncLoggingInterceptor> _logger;

    public AsyncLoggingInterceptor(ILogger<AsyncLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override void Intercept(IInvocation invocation)
    {
        _logger.LogInformation("Sync method: {Method}", invocation.Method.Name);
        invocation.Proceed();
    }

    public override async ValueTask InterceptAsync(IInvocation invocation)
    {
        _logger.LogInformation("Async method started: {Method}", invocation.Method.Name);
        
        await invocation.ProceedAsync();
        _logger.LogInformation("Async method completed: {Method}", invocation.Method.Name);
    }

    public override async ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation)
    {
        _logger.LogInformation("Async method with result started: {Method}", invocation.Method.Name);
        
        var result = await invocation.ProceedAsync();

        _logger.LogInformation("Async method completed with result: {Method}", invocation.Method.Name);
        return result;
    }
}

// Register with async interceptor
services.AddSingleton<AsyncLoggingInterceptor>();
services.AddInterceptedScoped<IMyAsyncService, MyAsyncService>(typeof(AsyncLoggingInterceptor));
```

### Keyed Services

Use keyed services with interception (.NET 8+):

```csharp
// Register multiple implementations with different keys
services.AddKeyedInterceptedSingleton<ICache, RedisCache>("redis", loggingInterceptor);
services.AddKeyedInterceptedSingleton<ICache, MemoryCache>("memory", loggingInterceptor);

// Resolve by key
var redisCache = serviceProvider.GetRequiredKeyedService<ICache>("redis");
var memoryCache = serviceProvider.GetRequiredKeyedService<ICache>("memory");
```

## API Reference

### Dependency Injection Extensions

All extension methods are available in the `Microsoft.Extensions.DependencyInjection` namespace.

#### Transient Service Registration

Register services with transient lifetime (new instance per request):

```csharp
// Basic registration
services.AddInterceptedTransient<TService>(params IInterceptor[] interceptors);
services.AddInterceptedTransient<TService>(params Type[] interceptorTypes);
services.AddInterceptedTransient<TService, TInterceptor>();

// With interface and implementation
services.AddInterceptedTransient<IService, Implementation>(params IInterceptor[] interceptors);
services.AddInterceptedTransient<IService, Implementation>(params Type[] interceptorTypes);
services.AddInterceptedTransient<IService, Implementation, TInterceptor>();

// With factory
services.AddInterceptedTransient<TService>(
    Func<IServiceProvider, TService> factory,
    params IInterceptor[] interceptors);

// Non-generic overloads
services.AddInterceptedTransient(Type serviceType, params IInterceptor[] interceptors);
services.AddInterceptedTransient(Type serviceType, Type implementationType, params IInterceptor[] interceptors);
```

**Try-Add Variants** (only register if not already registered):
```csharp
services.TryAddInterceptedTransient<TService>(params IInterceptor[] interceptors);
services.TryAddInterceptedTransient<IService, Implementation>(params IInterceptor[] interceptors);
```

**Keyed Service Variants**:
```csharp
services.AddKeyedInterceptedTransient<TService>(object? serviceKey, params IInterceptor[] interceptors);
services.TryAddKeyedInterceptedTransient<TService>(object? serviceKey, params IInterceptor[] interceptors);
```

#### Scoped Service Registration

Register services with scoped lifetime (one instance per request/scope):

```csharp
// All patterns available for transient are also available for scoped
services.AddInterceptedScoped<TService>(params IInterceptor[] interceptors);
services.AddInterceptedScoped<IService, Implementation>(params IInterceptor[] interceptors);
services.TryAddInterceptedScoped<TService>(params IInterceptor[] interceptors);
services.AddKeyedInterceptedScoped<TService>(object? serviceKey, params IInterceptor[] interceptors);
services.TryAddKeyedInterceptedScoped<TService>(object? serviceKey, params IInterceptor[] interceptors);
```

#### Singleton Service Registration

Register services with singleton lifetime (single instance for application lifetime):

```csharp
// All patterns available for transient/scoped are also available for singleton
services.AddInterceptedSingleton<TService>(params IInterceptor[] interceptors);
services.AddInterceptedSingleton<IService, Implementation>(params IInterceptor[] interceptors);
services.TryAddInterceptedSingleton<TService>(params IInterceptor[] interceptors);
services.AddKeyedInterceptedSingleton<TService>(object? serviceKey, params IInterceptor[] interceptors);
services.TryAddKeyedInterceptedSingleton<TService>(object? serviceKey, params IInterceptor[] interceptors);

// Singleton-specific: registration with existing instance
services.AddInterceptedSingleton<TService>(
    TService implementationInstance,
    params IInterceptor[] interceptors);
```

### Async Interceptor Support

#### IAsyncInterceptor Interface

```csharp
namespace Castle.DynamicProxy.Extensions
{
    public interface IAsyncInterceptor
    {
        // Synchronous interception
        void Intercept(IInvocation invocation);
        
        // Asynchronous interception (no return value)
        ValueTask InterceptAsync(IInvocation invocation);
        
        // Asynchronous interception (with return value)
        ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation);
    }
}
```

#### AsyncInterceptorBase Abstract Class

Base class for implementing async interceptors:

```csharp
public abstract class AsyncInterceptorBase : IAsyncInterceptor, IInterceptor
{
    // Implement these three abstract methods
    public abstract void Intercept(IInvocation invocation);
    public abstract ValueTask InterceptAsync(IInvocation invocation);
    public abstract ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation);
}
```

#### ProceedAsync Extension Methods

**⚠️ Important**: When implementing async interceptors, always use the `ProceedAsync()` extension methods instead of `Proceed()` in your `InterceptAsync` methods.

The `ProceedAsync()` extension methods provide proper async/await handling for intercepted async methods:

```csharp
namespace Castle.DynamicProxy
{
    public static class InvocationExtensions
    {
        // For async methods without return value (Task/ValueTask)
        public static ValueTask ProceedAsync(this IInvocation invocation);
        
        // For async methods with return value (Task<T>/ValueTask<T>)
        public static ValueTask<TResult?> ProceedAsync<TResult>(this IInvocation invocation);
    }
}
```

**Why use `ProceedAsync()`?**

✅ **Proper Async Flow**: Correctly captures and invokes the proceed info for async methods  
✅ **Exception Handling**: Ensures exceptions from async methods are properly propagated  
✅ **ConfigureAwait**: Uses `ConfigureAwait(false)` for better performance  
✅ **Type Safety**: Supports both `Task`/`Task<T>` and `ValueTask`/`ValueTask<T>`  

**Example - Correct Usage:**

```csharp
public override async ValueTask InterceptAsync(IInvocation invocation)
{
    // ✅ CORRECT: Use ProceedAsync()
    await invocation.ProceedAsync();
}

public override async ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation)
{
    // ✅ CORRECT: Use ProceedAsync<TResult>()
    return await invocation.ProceedAsync<TResult>();
}
```

**Example - Incorrect Usage (Do Not Use):**

```csharp
public override async ValueTask InterceptAsync(IInvocation invocation)
{
    // ❌ WRONG: Using Proceed() in async context
    invocation.Proceed();
    
    if (invocation.ReturnValue is Task task)
    {
        await task; // This may not work correctly
    }
}
```

### ProxyGenerator Extensions

Extension methods for `IProxyGenerator` to simplify proxy creation:

#### Class Proxy Creation

```csharp
// Create class proxy with async interceptors
var proxy = proxyGenerator.CreateClassProxy<TClass>(params IAsyncInterceptor[] asyncInterceptors);

// With options
var proxy = proxyGenerator.CreateClassProxy<TClass>(
    ProxyGenerationOptions options,
    params IAsyncInterceptor[] asyncInterceptors);

// With constructor arguments
var proxy = proxyGenerator.CreateClassProxy<TClass>(
    object?[]? constructorArguments,
    params IAsyncInterceptor[] asyncInterceptors);
```

#### Interface Proxy Creation

```csharp
// Create interface proxy without target
var proxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<TInterface>(
    params IAsyncInterceptor[] asyncInterceptors);

// Create interface proxy with target
var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(
    TInterface target,
    params IAsyncInterceptor[] asyncInterceptors);

// Create interface proxy with target interface
var proxy = proxyGenerator.CreateInterfaceProxyWithTargetInterface<TInterface>(
    TInterface target,
    params IAsyncInterceptor[] asyncInterceptors);
```

#### Class Proxy with Target

```csharp
var proxy = proxyGenerator.CreateClassProxyWithTarget<TClass>(
    TClass target,
    params IAsyncInterceptor[] asyncInterceptors);
```

## Advanced Scenarios

### Async Method Interception Best Practices

When implementing async interceptors, always use `ProceedAsync()` for proper async handling:

```csharp
public class AsyncCachingInterceptor : AsyncInterceptorBase
{
    private readonly IDistributedCache _cache;

    public override async ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation)
    {
        var cacheKey = GenerateCacheKey(invocation);
        
        // Check cache first
        var cachedData = await _cache.GetAsync(cacheKey);
        if (cachedData != null)
        {
            return JsonSerializer.Deserialize<TResult>(cachedData);
        }

        // ✅ Use ProceedAsync for proper async flow
        var result = await invocation.ProceedAsync<TResult>();
        
        // Cache the result
        if (result != null)
        {
            var serialized = JsonSerializer.SerializeToUtf8Bytes(result);
            await _cache.SetAsync(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }
        
        return result;
    }

    public override void Intercept(IInvocation invocation)
    {
        // Synchronous methods use Proceed()
        invocation.Proceed();
    }

    public override async ValueTask InterceptAsync(IInvocation invocation)
    {
        // ✅ Use ProceedAsync for Task methods without return value
        await invocation.ProceedAsync();
    }
}
```

### Multiple Interceptors

Interceptors are invoked in the order they are registered:

```csharp
services.AddSingleton<LoggingInterceptor>();
services.AddSingleton<CachingInterceptor>();
services.AddSingleton<AuthorizationInterceptor>();

services.AddInterceptedTransient<IMyService, MyService>(
    typeof(LoggingInterceptor),
    typeof(CachingInterceptor),
    typeof(AuthorizationInterceptor)
);

// Execution order: Logging -> Caching -> Authorization -> MyService -> Authorization -> Caching -> Logging
```

### Factory-Based Registration

Use factories for complex initialization:

```csharp
services.AddInterceptedTransient<IDataService>(
    serviceProvider =>
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = config.GetConnectionString("Default");
        return new DataService(connectionString);
    },
    loggingInterceptor,
    cachingInterceptor
);
```

### Try-Add Pattern

Prevent duplicate registrations:

```csharp
// First registration wins
services.TryAddInterceptedSingleton<ILogger, ConsoleLogger>(loggingInterceptor);
services.TryAddInterceptedSingleton<ILogger, FileLogger>(loggingInterceptor); // This won't be registered

var logger = serviceProvider.GetService<ILogger>(); // Returns ConsoleLogger
```

### Interceptor Type Resolution

Interceptors can be resolved from the DI container:

```csharp
// Register interceptor in DI
services.AddSingleton<MyInterceptor>();

// Reference by type - will be resolved from DI
services.AddInterceptedTransient<IMyService, MyService>(typeof(MyInterceptor));

// Or use generic constraint
services.AddInterceptedTransient<IMyService, MyService, MyInterceptor>();
```

## Examples

### Example 1: Logging Interceptor

```csharp
public class LoggingInterceptor : IInterceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var methodName = $"{invocation.TargetType?.Name}.{invocation.Method.Name}";
        
        _logger.LogInformation(
            "Entering {MethodName} with arguments: {Arguments}",
            methodName,
            string.Join(", ", invocation.Arguments));

        try
        {
            invocation.Proceed();
            
            _logger.LogInformation(
                "Exiting {MethodName} with result: {Result}",
                methodName,
                invocation.ReturnValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in {MethodName}",
                methodName);
            throw;
        }
    }
}

// Registration
services.AddSingleton<LoggingInterceptor>();
services.AddInterceptedTransient<IUserService, UserService>(typeof(LoggingInterceptor));
```

### Example 2: Caching Interceptor

```csharp
public class CachingInterceptor : AsyncInterceptorBase
{
    private readonly IMemoryCache _cache;

    public CachingInterceptor(IMemoryCache cache)
    {
        _cache = cache;
    }

    public override void Intercept(IInvocation invocation)
    {
        // Handle synchronous methods
        var cacheKey = GenerateCacheKey(invocation);
        
        if (_cache.TryGetValue(cacheKey, out object? cachedValue))
        {
            invocation.ReturnValue = cachedValue;
            return;
        }

        invocation.Proceed();
        _cache.Set(cacheKey, invocation.ReturnValue, TimeSpan.FromMinutes(5));
    }

    public override async ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation)
    {
        var cacheKey = GenerateCacheKey(invocation);
        
        if (_cache.TryGetValue(cacheKey, out TResult? cachedValue))
        {
            return cachedValue;
        }

        invocation.Proceed();
        
        if (invocation.ReturnValue is Task<TResult> task)
        {
            var result = await task;
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            return result;
        }

        return default;
    }

    public override ValueTask InterceptAsync(IInvocation invocation)
    {
        // For Task methods without return value
        invocation.Proceed();
        return invocation.ReturnValue is Task task
            ? new ValueTask(task)
            : ValueTask.CompletedTask;
    }

    private static string GenerateCacheKey(IInvocation invocation)
    {
        var methodName = $"{invocation.TargetType?.Name}.{invocation.Method.Name}";
        var args = string.Join(",", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
        return $"{methodName}({args})";
    }
}

// Registration
services.AddMemoryCache();
services.AddSingleton<CachingInterceptor>();
services.AddInterceptedScoped<IProductService, ProductService>(typeof(CachingInterceptor));
```

### Example 3: Authorization Interceptor

```csharp
public class AuthorizationInterceptor : IInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationInterceptor(
        IHttpContextAccessor httpContextAccessor,
        IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }

    public void Intercept(IInvocation invocation)
    {
        var authorizeAttribute = invocation.Method
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .FirstOrDefault() as AuthorizeAttribute;

        if (authorizeAttribute != null)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                throw new UnauthorizedAccessException(
                    $"User must be authenticated to call {invocation.Method.Name}");
            }

            if (!string.IsNullOrEmpty(authorizeAttribute.Roles))
            {
                var roles = authorizeAttribute.Roles.Split(',');
                if (!roles.Any(role => user.IsInRole(role.Trim())))
                {
                    throw new UnauthorizedAccessException(
                        $"User must be in one of these roles: {authorizeAttribute.Roles}");
                }
            }
        }

        invocation.Proceed();
    }
}

// Usage
public interface IAdminService
{
    [Authorize(Roles = "Admin")]
    void DeleteUser(int userId);
}

// Registration
services.AddSingleton<AuthorizationInterceptor>();
services.AddInterceptedScoped<IAdminService, AdminService>(typeof(AuthorizationInterceptor));
```

### Example 4: Performance Monitoring

```csharp
public class PerformanceMonitoringInterceptor : AsyncInterceptorBase
{
    private readonly ILogger<PerformanceMonitoringInterceptor> _logger;

    public PerformanceMonitoringInterceptor(ILogger<PerformanceMonitoringInterceptor> logger)
    {
        _logger = logger;
    }

    public override void Intercept(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            invocation.Proceed();
        }
        finally
        {
            stopwatch.Stop();
            LogPerformance(invocation, stopwatch.ElapsedMilliseconds);
        }
    }

    public override async ValueTask InterceptAsync(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await invocation.ProceedAsync();
        }
        finally
        {
            stopwatch.Stop();
            LogPerformance(invocation, stopwatch.ElapsedMilliseconds);
        }
    }

    public override async ValueTask<TResult?> InterceptAsync<TResult>(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            return await invocation.ProceedAsync();
        }
        finally
        {
            stopwatch.Stop();
            LogPerformance(invocation, stopwatch.ElapsedMilliseconds);
        }
    }

    private void LogPerformance(IInvocation invocation, long elapsedMilliseconds)
    {
        var methodName = $"{invocation.TargetType?.Name}.{invocation.Method.Name}";
        
        if (elapsedMilliseconds > 1000)
        {
            _logger.LogWarning(
                "SLOW: {MethodName} took {ElapsedMs}ms",
                methodName,
                elapsedMilliseconds);
        }
        else
        {
            _logger.LogInformation(
                "{MethodName} took {ElapsedMs}ms",
                methodName,
                elapsedMilliseconds);
        }
    }
}

// Registration
services.AddSingleton<PerformanceMonitoringInterceptor>();
services.AddInterceptedTransient<IOrderService, OrderService>(
    typeof(PerformanceMonitoringInterceptor));
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Built with ❤️ using Castle.DynamicProxy and .NET 10**