# Minimal Dependency Injection

This is a minimal implementation of the `Microsoft.Extensions.DependencyInjection` package, including the `ActivatorUtilities` class.

Allows for Singleton and Transient services to be registered with an IOC container. Scoped services have not been implemented.

This is designed as a very lightweight, and minimalistic, solution, but this may come at a cost. This package does not include many of the safety measures and fallbacks that the full package contains. It was written from the ground up, other than the `ActivatorUtilities` class. Therefore, it might not play well with third-party IOC wrappers like AutoFac or CastleWindsor. It should not be seen as a replacement for `Microsoft.Extensions.DependencyInjection`.

The original purpose of this package was to act as an IOC container for game mods, where a full enterprise level package simply comes with too much bloat. This is ideal for those kinds of scenarios, when you just need a quick and simple IOC container, without all the bloat.

## Acknowledgement

Inspired by Nick Chapsas' video tutorial on bespoke dependency injection solutions.

https://www.youtube.com/watch?v=NSVZa4JuTl8

---

## Overview

This repository provides a compact, focused implementation of dependency injection primitives suitable for small applications, tools and game mods where the official Microsoft dependency injection package would be overly heavy.

Key components include:

- `IServiceCollection` - a lightweight service collection interface.
- `ServiceCollection` - a simple list-backed implementation of `IServiceCollection`.
- `ServiceDescriptor` - describes a service registration (service type, implementation, factory and lifetime).
- `ActivatorUtilities` - helpers to instantiate types using constructor injection from an `IServiceProvider`.
- Attribute-based registration helpers in the `Annotation` namespace: `SingletonAttribute`, `TransientAttribute`, and scanning helpers to register annotated types from assemblies.

This project is intentionally small and dependency-light. It aims to provide the core features most commonly needed for composition and injection without the additional complexity of the full Microsoft implementation.

## Features

- Register transient and singleton services using descriptors or extension methods.
- Register instances and factories for custom construction logic.
- Mix explicit constructor arguments with container-resolved services using `ActivatorUtilities`.
- Scan assemblies and register types annotated with `SingletonAttribute` or `TransientAttribute`.
- Lightweight, zero-dependency implementation intended for constrained environments.

## Important Notes and Limitations

- Scoped services are not implemented.
- This implementation does not include all the diagnostics, fallback behaviour or extension points present in the official Microsoft DI.
- Behaviour may differ from the official DI in edge cases - test carefully before replacing the official DI in production systems.
- There is no guarantee of compatibility with third-party IOC containers or wrappers.

## Quick Start

The library exposes a simple API surface for registering services and creating instances. The code below demonstrates common tasks.

1. Add Services to a `ServiceCollection`:

```csharp
var services = new ApacheTech.Common.DependencyInjection.ServiceCollection();

// Register concrete type as singleton
services.TryAddSingleton<MyService>();

// Register service with implementation
services.TryAddTransient(typeof(IMyService), typeof(MyService));

// Register using factory
services.TryAddSingleton(typeof(IMyService), sp => new MyService(sp.GetService(typeof(IOther))));
```

2. Resolve Services via an `IServiceProvider`:

This repository provides a minimal set of `IServiceProvider` extension helpers. How you obtain a runtime service provider instance depends on your composition root or hosting environment. The examples below use the extension helpers for resolution where a provider is available.

```csharp
IServiceProvider provider = /* obtain provider from your composition root */;
var svc = provider.GetRequiredService<MyService>();
```

3. Create Instances with `ActivatorUtilities`:

```csharp
// Create an instance of a type and let the container satisfy constructor dependencies
var instance = ActivatorUtilities.CreateInstance(provider, typeof(MyType), someExplicitArg);

// Generic helper
var typed = ActivatorUtilities.CreateInstance<MyType>(provider, someExplicitArg);

// Get service or create instance if not registered
var maybe = ActivatorUtilities.GetServiceOrCreateInstance(provider, typeof(MyOtherType));
```

## API Details

This section documents the public API surface in more detail and includes short examples showing common usages.

### ServiceDescriptor

`ServiceDescriptor` represents a registration. It records the service type, the implementation type or instance, an optional factory, and the service lifetime.

Common factory methods and constructors are shown in the table below.

| Method | Description | Example |
|---|---|---|
| `Transient<TService, TImplementation>()` | Register `TImplementation` as a transient implementation for `TService`. | `var d = ServiceDescriptor.Transient<IMyService, MyService>();` |
| `Transient<TService>(Func<IServiceProvider, TService> factory)` | Register a transient using a generic factory. | `var d = ServiceDescriptor.Transient(sp => new MyService());` |
| `Transient(Type service, Type implementationType)` | Non-generic transient overload. | `var d = ServiceDescriptor.Transient(typeof(IMyService), typeof(MyService));` |
| `Transient(Type service, Func<IServiceProvider, object> implementationFactory)` | Non-generic transient using a factory. | `var d = ServiceDescriptor.Transient(typeof(IMyService), sp => new MyService());` |
| `Singleton<TService, TImplementation>()` | Register `TImplementation` as a singleton implementation for `TService`. | `var d = ServiceDescriptor.Singleton<IMyService, MyService>();` |
| `Singleton(Type service, Type implementationType)` | Non-generic singleton overload. | `var d = ServiceDescriptor.Singleton(typeof(IMyService), typeof(MyService));` |
| `Singleton(Type service, object instance)` | Register an existing instance as a singleton. | `var d = ServiceDescriptor.Singleton(typeof(IMyService), instance);` |
| `Singleton<TService>(Func<IServiceProvider, TService> factory)` | Register a singleton with a factory. | `var d = ServiceDescriptor.Singleton(sp => new MyService());` |
| `Describe(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)` | Create a descriptor with a factory and explicit lifetime. | `var d = ServiceDescriptor.Describe(typeof(IMyService), sp => new MyService(), ServiceLifetime.Singleton);` |

`ServiceDescriptor` also exposes `GetImplementationType()` which is useful to determine the concrete implementation type for a descriptor when inspecting or composing descriptors.

---

### IServiceCollection and ServiceCollection

`IServiceCollection` is a minimal contract that extends `IList<ServiceDescriptor>`. `ServiceCollection` is the list-backed default implementation.

Common collection operations and helpers are shown in the table below.

| Operation | Description | Example |
|---|---|---|
| `Add(ServiceDescriptor)` | Append a descriptor unconditionally. | `services.Add(ServiceDescriptor.Transient<IMyService, MyService>());` |
| `Add(IEnumerable<ServiceDescriptor>)` | Append multiple descriptors unconditionally. | `services.Add(new[] { d1, d2 });` |
| `TryAdd(ServiceDescriptor)` | Add a descriptor only if the service type is not already present. | `services.TryAdd(ServiceDescriptor.Singleton<IMyService, MyService>());` |
| `TryAdd(IEnumerable<ServiceDescriptor>)` | Try to add multiple descriptors, skipping those whose service type already exists. | `services.TryAdd(new[] { d1, d2 });` |
| `TryAddTransient(Type service, Type impl)` | Add a transient registration only if the service type is not already registered. | `services.TryAddTransient(typeof(IMyService), typeof(MyService));` |
| `Replace(ServiceDescriptor descriptor)` | Remove the first descriptor with a matching `ServiceType` and add the supplied descriptor. | `services.Replace(ServiceDescriptor.Singleton(typeof(IMyService), new MyService()));` |
| `RemoveAll(Type serviceType)` | Remove every descriptor for the specified service type. | `services.RemoveAll(typeof(IMyService));` |

Note: `Add` is unconditional, while `TryAdd` is conditional on the absence of a registration for the same `ServiceType`.

Use `TryAdd` or `TryAddEnumerable` when adding descriptors from scanning to ensure idempotent behaviour.

---

### ServiceCollectionDescriptorExtensions

This static class contains convenience helpers for common registration patterns. Key helpers are shown below.

| Helper | Description | Example |
|---|---|---|
| `AddTransient(this IServiceCollection, Type service, Type implementationType)` | Add a transient registration with specified implementation type. | `services.AddTransient(typeof(IMyService), typeof(MyService));` |
| `AddTransient(this IServiceCollection, Type service, Func<IServiceProvider, object> implementationFactory)` | Add a transient registration using a factory. | `services.AddTransient(typeof(IMyService), sp => new MyService());` |
| `AddTransient<TService, TImplementation>()` | Generic form to add a transient mapping. | `services.AddTransient<IMyService, MyService>();` |
| `AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)` | Generic transient using a factory. | `services.AddTransient(sp => new MyService());` |
| `TryAddTransient<TService, TImplementation>()` | Register a transient mapping with generic types if not already registered. | `services.TryAddTransient<IMyService, MyService>();` |
| `TryAddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)` | Register a transient with a factory. | `services.TryAddTransient(sp => new MyService());` |
| `TryAddTransient(this IServiceCollection, Type service, Func<IServiceProvider, object> implementationFactory)` | Non-generic transient factory overload. | `services.TryAddTransient(typeof(IMyService), sp => new MyService());` |
| `AddSingleton(this IServiceCollection, Type serviceType, Type implementationType)` | Add a singleton registration with specified implementation type. | `services.AddSingleton(typeof(IMyService), typeof(MyService));` |
| `AddSingleton(this IServiceCollection, Type serviceType, Func<IServiceProvider, object> implementationFactory)` | Add a singleton registration using a factory. | `services.AddSingleton(typeof(IMyService), sp => new MyService());` |
| `AddSingleton<TService, TImplementation>()` | Generic form to add a singleton mapping. | `services.AddSingleton<IMyService, MyService>();` |
| `AddSingleton(this IServiceCollection, Type serviceType, object implementationInstance)` | Register an existing instance as a singleton. | `services.AddSingleton(typeof(IMyService), instance);` |
| `AddSingleton<TService>(TService implementationInstance)` | Generic instance singleton registration. | `services.AddSingleton<IMyService>(instance);` |
| `TryAddSingleton<TService, TImplementation>()` | Register a singleton mapping with generic types if not already registered. | `services.TryAddSingleton<IMyService, MyService>();` |
| `TryAddSingleton<TService>(TService instance)` | Register an existing instance as a singleton if not already registered. | `services.TryAddSingleton<IMyService>(instance);` |
| `TryAddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)` | Register a singleton using a factory. | `services.TryAddSingleton(sp => new MyService());` |
| `TryAdd(this IServiceCollection, ServiceDescriptor)` | Add a descriptor only if a registration for the same `ServiceType` does not exist. | `services.TryAdd(ServiceDescriptor.Singleton(...));` |
| `TryAddEnumerable(ServiceDescriptor)` | Add a descriptor supporting multiple implementations while avoiding duplicate implementation types. | `services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IMyService), typeof(MyImpl)));` |
| `TryAddEnumerable(IEnumerable<ServiceDescriptor>)` | Add multiple enumerable descriptors with duplicate implementation checking. | `services.TryAddEnumerable(new[] { d1, d2 });` |
| `Replace(ServiceDescriptor)` | Replace the first matching registration with the provided descriptor. | `services.Replace(ServiceDescriptor.Singleton(...));` |
| `RemoveAll(Type serviceType)` | Remove all registrations for the given service type. | `services.RemoveAll(typeof(IMyService));` |

---

### ActivatorUtilities

`ActivatorUtilities` provides helpers for creating instances where some constructor arguments are supplied explicitly and others are resolved from an `IServiceProvider`.

| Method | Description | Example |
|---|---|---|
| `CreateInstance(IServiceProvider provider, Type instanceType, params object[] parameters)` | Create and initialise an instance of `instanceType`, resolving remaining constructor parameters from the provider. | `var obj = ActivatorUtilities.CreateInstance(provider, typeof(MyController), arg1);` |
| `CreateInstance<T>(IServiceProvider provider, params object[] parameters)` | Generic variant of `CreateInstance`. | `var obj = ActivatorUtilities.CreateInstance<MyController>(provider, arg1);` |
| `CreateFactory(Type instanceType, Type[] argumentTypes)` | Produce an `ObjectFactory` delegate that can be reused to instantiate many instances efficiently. | `var factory = ActivatorUtilities.CreateFactory(typeof(MyWorker), new[] { typeof(string), typeof(int) });` |
| `GetServiceOrCreateInstance(IServiceProvider provider, Type type)` | Resolve a registered service or create an instance if not registered. | `var svc = ActivatorUtilities.GetServiceOrCreateInstance(provider, typeof(MyService));` |

If a class has a constructor marked with `ActivatorUtilitiesConstructor` then that constructor is preferred when selecting which constructor to use.

---

### Annotations and Assembly Scanning

The library provides a small set of attributes to support declarative registration. The table below summarises the attributes and how to discover annotated types.

| Attribute | Purpose | Example |
|---|---|---|
| `SingletonAttribute(Type? serviceType = null)` | Mark a class to be registered as a singleton. Optionally specify the service interface type. | `[Singleton(typeof(IMyService))] public class MyService : IMyService { }` |
| `TransientAttribute(Type? serviceType = null)` | Mark a class to be registered as transient. Optionally specify the service interface type. | `[Transient] public class OtherService { }` |
| `ActivatorUtilitiesConstructor` | Mark the constructor to prefer when creating instances with `ActivatorUtilities`. | `public MyType([ActivatorUtilitiesConstructor] MyType(...)) { }` |

To register annotated types, use the `AddAnnotatedServicesFromAssembly` extension methods on `IServiceCollection`.

| Method | Description | Example |
|---|---|---|
| `AddAnnotatedServicesFromAssembly(this IServiceCollection services, Assembly assembly)` | Scan the supplied assembly for types annotated with `ServiceAttribute` derivatives and add descriptors. | `services.AddAnnotatedServicesFromAssembly(typeof(MyService).Assembly);` |
| `AddAnnotatedServicesFromAssembly(this IServiceCollection services)` | Scan the calling assembly. | `services.AddAnnotatedServicesFromAssembly();` |
| `AddAnnotatedServicesFromAssembly(this IServiceCollection services, params Type[] assemblyMarkers)` | Scan the assemblies that contain the supplied marker types. | `services.AddAnnotatedServicesFromAssembly(typeof(MyService), typeof(OtherMarker));` |

#### Example:

```csharp
[Singleton(typeof(IMyService))]
public class MyService : IMyService { }

[Transient]
public class OtherService { }
```

Register annotated types from an assembly:

```csharp
var services = new ApacheTech.Common.DependencyInjection.ServiceCollection();
services.AddAnnotatedServicesFromAssembly(typeof(MyService));
```

This will create `ServiceDescriptor` instances based on the attributes and add them to the collection.

---

### Service Provider Extension Helpers

The project includes a small set of `IServiceProvider` extension helpers in `ServiceProviderExtensions` to make consuming services easier.

| Helper | Description | Example |
|---|---|---|
| `CreateInstance(this IServiceProvider provider, Type serviceType, params object[] args)` | Wrapper around `ActivatorUtilities.CreateInstance`. | `var obj = provider.CreateInstance(typeof(MyType), someArg);` |
| `CreateInstance<T>(this IServiceProvider provider, params object[] args)` | Generic wrapper around `CreateInstance`. | `var obj = provider.CreateInstance<MyType>(someArg);` |
| `Resolve<T>(this IServiceProvider provider)` | Alias for `GetRequiredService<T>()`. | `var s = provider.Resolve<MyService>();` |
| `GetRequiredService<T>(this IServiceProvider provider)` | Attempt to get a service and throw `KeyNotFoundException` if not present. | `var s = provider.GetRequiredService<MyService>();` |
| `GetServices<T>(this IServiceProvider provider)` | Attempt to resolve `IEnumerable<T>` and return an empty collection when none are registered. | `var all = provider.GetServices<IMyService>();` |