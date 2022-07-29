using System;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation
{
    /// <summary>
    ///     Denotes that this class should be registered as a singleton service within the IOC container.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonServiceAttribute : AnnotatedServiceAttribute
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="SingletonServiceAttribute"/> class.
        /// </summary>
        public SingletonServiceAttribute() 
            : base(ServiceLifetime.Singleton)
        {
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="SingletonServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public SingletonServiceAttribute(Type serviceType) 
            : base(ServiceLifetime.Singleton, serviceType)
        {
        }
    }
}