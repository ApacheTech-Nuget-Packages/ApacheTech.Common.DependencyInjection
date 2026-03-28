#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace ApacheTech.Common.DependencyInjection;

internal sealed class ResolutionContext
{
    private readonly HashSet<Type> _callStack = [];

    public void Enter(Type type)
    {
        if (!_callStack.Add(type))
        {
            throw new InvalidOperationException(
                $"Circular dependency detected: {string.Join(" → ", _callStack.Select(t => t.Name))} → {type.Name}");
        }
    }

    public void Exit(Type type)
    {
        _callStack.Remove(type);
    }
}