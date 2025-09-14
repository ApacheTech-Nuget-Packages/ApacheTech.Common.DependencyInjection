using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation;

/// <summary>
///     Marks the constructor to be used when activating type using <see cref="ActivatorUtilities" />.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor)]
public class ActivatorUtilitiesConstructor : Attribute;