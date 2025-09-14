using System;
using System.Collections.Generic;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

/// <summary>
///     Provides extension methods for hosting services.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    ///     Attempts to add an element, with the provided key and value, to the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c>, if the element was successfully added to the collection; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">dictionary</exception>
    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
        if (dictionary.ContainsKey(key)) return false;
        dictionary.Add(key, value);
        return true;

    }

    /// <summary>
    ///     Attempts to add an element, with the provided key and value, to the <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c>, if the element was successfully added to the collection; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">dictionary</exception>
    public static bool TryAdd<TValue>(this IList<TValue> collection, TValue value)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        if (collection.Contains(value)) return false;
        collection.Add(value);
        return true;
    }
}