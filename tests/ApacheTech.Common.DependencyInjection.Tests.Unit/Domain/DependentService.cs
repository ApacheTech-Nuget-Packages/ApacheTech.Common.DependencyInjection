namespace ApacheTech.Common.DependencyInjection.Tests.Unit.Domain;

public sealed class DependentService(ScopedService scoped)
{
    public ScopedService Scoped { get; } = scoped;
}
