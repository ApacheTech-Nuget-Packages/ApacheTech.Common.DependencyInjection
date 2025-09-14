# Minimal Dependency Injection

This is a minimal implmentation of the `Microsoft.Extensions.DependencyInjection` package, including the `ActivatorUtilities` class.

Allows for Singleton and Transient services to be registered with an IOC container. Scoped services have not been implemented.

This is designed as a very lightweight, and minimalistic, but this may come at a cost. This package doesn't come with a lot of the safety measures, and fallbacks that the full package contains. It was written from the ground up, other than the `ActivatorUtilities` class. Therefore, it might not play well with thrid-party ICO wrappers like AutoFac, or CastleWindsor. It should not be seen as a replacement for Microsoft.Extensions.DependencyInjection.

The original purpose of this package was to act as an IOC container for game mods, where a full Enterprise level package simply comes with too much bloat. This is ideal for those kinds of scenarios, when you just need a quick and simple IOC container, without all the bloat.

## Acknowledgement:

Inspired by Nick Chapsas' video tutorial on bespoke dependency injection solutions.

https://www.youtube.com/watch?v=NSVZa4JuTl8

---

## Overview

This project provides a compact, focused implementation of dependency injection primitives. It includes:

- `IServiceCollection` - a lightweight service collection interface.
- `ServiceCollection` - a simple list-backed implementation of `IServiceCollection`.
- `ServiceDescriptor` - describes a service registration (service type, implementation, factory and lifetime).
- `ActivatorUtilities` - helpers to instantiate types using constructor injection from an `IServiceProvider`.
- Attribute-based registration helpers: `SingletonAttribute`, `TransientAttribute`, and scanning helpers to register annotated types from assemblies.

The goal is to offer a tiny, dependency-free alternative suitable for small projects, tools, or game modding where the full `Microsoft.Extensions.DependencyInjection` is too heavy.

## Features

- Register transient and singleton services via descriptors or convenience extension methods.
- Resolve services from an `IServiceProvider` instance.
- Create instances that mix explicit constructor arguments with services from the provider via `ActivatorUtilities`.
- Scan assemblies and register types annotated with `SingletonAttribute` or `TransientAttribute`.

## Important Notes / Limitations

- This implementation intentionally omits scoped services.
- It is minimal and does not implement all safeguards, diagnostic features or integrations that the official Microsoft DI provides.
- Behaviour may differ from the official DI in edge cases - test before replacing the official DI in critical systems.

## Quick Start

1. Add services to a `ServiceCollection`:

```csharp
var services = new ApacheTech.Common.DependencyInjection.ServiceCollection();

// Register concrete type as singleton
services.TryAddSingleton<MyService>();

// Register service with implementation
services.TryAddTransient(typeof(IMyService), typeof(MyService));

// Register using factory
services.TryAddSingleton(typeof(IMyService), sp => new MyService(sp.GetService(typeof(IOther))));
```

2. Resolve Services via an `IServiceProvider` (the project provides `IServiceProvider` extensions to help):

```csharp
IServiceProvider provider = /* obtain provider from your composition root */;
var svc = provider.GetRequiredService<MyService>();
```

3. Create Instances with `ActivatorUtilities`:

```csharp
// Create instance of a type and let the container satisfy constructor dependencies
var instance = ActivatorUtilities.CreateInstance(provider, typeof(MyType), someExplicitArg);

// Generic helper
var typed = ActivatorUtilities.CreateInstance<MyType>(provider, someExplicitArg);

// Get service or create instance if not registered
var maybe = ActivatorUtilities.GetServiceOrCreateInstance(provider, typeof(MyOtherType));
```

## Attribute Based Registration

Annotate classes with `SingletonAttribute` or `TransientAttribute` to make them discoverable by the scanning helpers.

#### Example:

```csharp
[Singleton(typeof(IMyService))]
public class MyService : IMyService { }

[Transient]
public class OtherService { }
```

Then register annotated types from an assembly:

```csharp
var services = new ApacheTech.Common.DependencyInjection.ServiceCollection();
services.AddAnnotatedServicesFromAssembly(typeof(MyService));
```

This will create `ServiceDescriptor`s based on the attributes and add them to the collection.

## API Highlights

- `ServiceDescriptor` - factory methods like `Transient`, `Singleton` and `Describe` to create registrations.
- `IServiceCollection` / `ServiceCollection` - storage for `ServiceDescriptor`s.
- `ServiceCollectionDescriptorExtensions` - extension helpers such as `Add`, `TryAdd`, `TryAddSingleton`, `TryAddTransient`, `Replace`, and `RemoveAll`.
- `ActivatorUtilities` - utilities for creating instances and compatible factories (`CreateFactory`, `CreateInstance`, `GetServiceOrCreateInstance`).
- Annotations in `Annotation` namespace - `SingletonAttribute`, `TransientAttribute` and `ActivatorUtilitiesConstructor`.

## Dependencies

- `ApacheTech.Common.Extensions` (used for some reflection helpers)