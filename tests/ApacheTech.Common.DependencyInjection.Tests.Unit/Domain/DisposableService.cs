namespace ApacheTech.Common.DependencyInjection.Tests.Unit.Domain;

public sealed class DisposableService : IDisposable
{
    public bool Disposed { get; private set; }

    public void Dispose()
    {
        Disposed = true;
    }
}