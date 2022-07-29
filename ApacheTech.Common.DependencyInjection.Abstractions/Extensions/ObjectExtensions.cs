using System;
using System.Runtime.CompilerServices;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions
{
    /// <summary>
    ///     Extension methods for objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Throws an <exception cref="ArgumentNullException"></exception>, if the object is null.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="this">The instance to check.</param>
        /// <param name="paramName">The name of the object passed into this extension method.</param>
        public static void ThrowIfNull<T>(this T @this, [CallerMemberName] string paramName = "")
        {
            if (@this == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}