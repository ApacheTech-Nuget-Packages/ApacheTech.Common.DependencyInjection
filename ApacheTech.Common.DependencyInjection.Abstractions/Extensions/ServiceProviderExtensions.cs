using System;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions
{
    /// <summary>
    ///     Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <param name="serviceProvider">And object that provides access to the services collection.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="args">An optional list of arguments, sent the the constructor of the instantiated class.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if no object of type <paramref name="serviceType" /> can be instantiated from the service collection.</returns>
        public static object CreateInstance(this IServiceProvider serviceProvider, Type serviceType, params object[] args)
        {
            return ActivatorUtilities.CreateInstance(serviceProvider, serviceType, args);
        }

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="serviceProvider">And object that provides access to the services collection.</param>
        /// <param name="args">An optional list of arguments, sent the the constructor of the instantiated class.</param>
        /// <returns>An object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no object of type <typeparamref name="T" /> can be instantiated from the service collection.</returns>
        public static T CreateInstance<T>(this IServiceProvider serviceProvider, params object[] args) where T : class
        {
            return (T)serviceProvider.CreateInstance(typeof(T), args);
        }

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceProvider">And object that provides access to the services collection.</param>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no service object of type <typeparamref name="T" />.</returns>
        public static T Resolve<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}