# Minimal Dependency Injection

This is a minimal implmentation of the `Microsoft.Extensions.DependencyInjection` package, including the `ActivatorUtilities` class.

Allows for Singleton and Transient services to be registered with an IOC container. Scoped services have not been implemented.

This is designed as a very lightweight, and minimalistic, but this may come at a cost. This package doesn't come with a lot of the safety measures, and fallbacks that the full package contains. It was written from the ground up, other than the `ActivatorUtilities` class. Therefore, it might not play well with thrid-party ICO wrappers like AutoFac, or CastleWindsor. It should not be seen as a replacement for Microsoft.Extensions.DependencyInjection.

The original purpose of this package was to act as an IOC container for game mods, where a full Enterprise level package simply comes with too much bloat. This is ideal for those kinds of scenarios, when you just need a quick and simple IOC container, without all the bloat.

## Acknowledgement:

Inspired by Nick Chapsas' video tutorial on bespoke dependency injection solutions.