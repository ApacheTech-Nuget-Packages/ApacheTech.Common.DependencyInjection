namespace ApacheTech.Common.DependencyInjection.Tests.Unit.Domain;

public sealed class AsyncDisposableService : IAsyncDisposable
{
    public bool Disposed { get; private set; }

    public ValueTask DisposeAsync()
    {
        Disposed = true;
        return ValueTask.CompletedTask;
    }
}