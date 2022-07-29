using System;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation
{
    /// <summary>
    ///     Denotes that this class should be registered as a transient service within the IOC container.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class TransientServiceAttribute : AnnotatedServiceAttribute
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="TransientServiceAttribute"/> class.
        /// </summary>
        public TransientServiceAttribute()
        : base(ServiceLifetime.Singleton)
        {
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="TransientServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public TransientServiceAttribute(Type serviceType)
        : base(ServiceLifetime.Singleton, serviceType)
        {
        }
    }
}