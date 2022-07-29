using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation
{
    /// <summary>
    ///     Denotes that this class should be registered as a service within the IOC container.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class AnnotatedServiceAttribute : Attribute
    {
        private readonly ServiceLifetime _serviceLifetime;
        private readonly Type _serviceType;

        /// <summary>
        ///     Initialises a new instance of the <see cref="AnnotatedServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceLifetime">The service lifetime.</param>
        protected AnnotatedServiceAttribute(ServiceLifetime serviceLifetime)
        {
            _serviceLifetime = serviceLifetime;
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SingletonServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <param name="serviceType">The type of the representation of the registered class within the IOC Container.</param>
        protected AnnotatedServiceAttribute(ServiceLifetime serviceLifetime, Type serviceType) : this(serviceLifetime)
        {
            _serviceType = serviceType;
        }

        /// <summary>
        ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified <paramref name="implementationType"/>.
        /// </summary>
        /// <param name="implementationType">The type of the implementation.</param>
        public ServiceDescriptor Describe(Type implementationType)
        {
           return ServiceDescriptor.Describe(_serviceType ?? implementationType, implementationType, _serviceLifetime);
        }
    }
}