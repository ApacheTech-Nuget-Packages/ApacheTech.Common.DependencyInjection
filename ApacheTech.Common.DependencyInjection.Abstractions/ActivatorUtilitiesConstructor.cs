using System;

// ReSharper disable EmptyConstructor
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions
{
    /// <summary>
    ///     Marks the constructor to be used when activating type using <see cref="ActivatorUtilities" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class ActivatorUtilitiesConstructor : Attribute
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="ActivatorUtilitiesConstructor"/> class.
        /// </summary>
        public ActivatorUtilitiesConstructor()
        {
        }
    }
}