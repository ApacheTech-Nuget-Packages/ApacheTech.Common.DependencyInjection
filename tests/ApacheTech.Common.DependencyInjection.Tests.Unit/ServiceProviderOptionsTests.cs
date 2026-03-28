namespace ApacheTech.Common.DependencyInjection.Tests.Unit;

public class ServiceProviderOptionsTests
{
    [Fact]
    public void Default_DisposeImplementations_IsTrue()
    {
        var options = new ServiceProviderOptions();

        Assert.True(options.DisposeImplementations);
    }

    [Fact]
    public void DisposableAssemblies_CanBeConfigured()
    {
        var options = new ServiceProviderOptions();
        var assembly = typeof(ServiceProviderOptionsTests).Assembly;

        options.DisposableAssemblies = [assembly];

        Assert.NotNull(options.DisposableAssemblies);
        Assert.Single(options.DisposableAssemblies);
        Assert.Equal(assembly, options.DisposableAssemblies[0]);
    }

    [Fact]
    public void ValidateScopes_CanBeToggled()
    {
        var options = new ServiceProviderOptions
        {
            ValidateScopes = true
        };

        Assert.True(options.ValidateScopes);

        options.ValidateScopes = false;

        Assert.False(options.ValidateScopes);
    }
}
