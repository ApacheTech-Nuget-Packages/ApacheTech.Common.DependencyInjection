using ApacheTech.Common.DependencyInjection.Abstractions;

namespace ApacheTech.Common.DependencyInjection.Tests.Unit;

public class ServiceCollectionTests
{
    [Fact]
    public void AddAndIndex_ShouldStoreDescriptorsInOrder()
    {
        var collection = new ServiceCollection();
        var descriptor1 = new ServiceDescriptor(typeof(string), typeof(string), ServiceLifetime.Transient);
        var descriptor2 = new ServiceDescriptor(typeof(int), typeof(int), ServiceLifetime.Singleton);

        ((ICollection<ServiceDescriptor>)collection).Add(descriptor1);
        ((ICollection<ServiceDescriptor>)collection).Add(descriptor2);

        Assert.Equal(2, collection.Count);
        Assert.Same(descriptor1, collection[0]);
        Assert.Same(descriptor2, collection[1]);
    }

    [Fact]
    public void Remove_ShouldRemoveDescriptor()
    {
        var collection = new ServiceCollection();
        var descriptor = new ServiceDescriptor(typeof(string), typeof(string), ServiceLifetime.Transient);
        ((ICollection<ServiceDescriptor>)collection).Add(descriptor);

        var removed = collection.Remove(descriptor);

        Assert.True(removed);
        Assert.Empty(collection);
    }

    [Fact]
    public void Clear_ShouldRemoveAllDescriptors()
    {
        var collection = new ServiceCollection();
        ((ICollection<ServiceDescriptor>)collection).Add(new ServiceDescriptor(typeof(string), typeof(string), ServiceLifetime.Transient));
        ((ICollection<ServiceDescriptor>)collection).Add(new ServiceDescriptor(typeof(int), typeof(int), ServiceLifetime.Singleton));

        collection.Clear();

        Assert.Empty(collection);
    }

    [Fact]
    public void Indexer_Set_ShouldReplaceDescriptor()
    {
        var collection = new ServiceCollection();
        var original = new ServiceDescriptor(typeof(string), typeof(string), ServiceLifetime.Transient);
        var replacement = new ServiceDescriptor(typeof(int), typeof(int), ServiceLifetime.Singleton);
        ((ICollection<ServiceDescriptor>)collection).Add(original);

        collection[0] = replacement;

        Assert.Same(replacement, collection[0]);
        Assert.Single(collection);
    }
}
