using System;
using ApacheTech.Common.DependencyInjection.Abstractions;

// ReSharper disable EmptyConstructor
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Annotation
{
    /// <summary>
    ///     Denotes that this class should be registered within the IOC container, when the mod is launched.
    /// </summary>
    /// <remarks>
    ///     If no specific type is supplied, the class will be registered via convention:<br/><br/>
    ///
    ///      • If the class implements an interface, the interface will be be used as the representation.<br/>
    ///      • If the class implements more than one interface, the first interface will be be used as the representation.<br/>
    ///      • If the class does not implement an interface, it will be registered as itself.
    /// </remarks>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisteredServiceAttribute : Attribute
    {
        /// <summary>
        ///     Gets the scope of the service, be it Singleton, Scoped, or Transient.
        /// </summary>
        /// <value>The service scope.</value>
        internal ServiceLifetime ServiceScope { get; }

        /// <summary>
        ///     Gets the type of the registered.
        /// </summary>
        /// <value>The type of the registered.</value>
        internal Type ServiceType { get; }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="RegisteredServiceAttribute"/> class.
        /// </summary>
        /// <param name="serviceScope">The service scope.</param>
        /// <param name="serviceType">The type of the representation of the registered class within the IOC Container.</param>
        public RegisteredServiceAttribute(ServiceLifetime serviceScope, Type serviceType)
        {
            ServiceScope = serviceScope;
            ServiceType = serviceType;
        }
    }
}