using System;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;
using ApacheTech.Common.Extensions.Reflection;

// ReSharper disable UnusedType.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation
{
    /// <summary>
    ///     Extensions methods to aid in the addition of annotated services.
    /// </summary>
    public static class AnnotationExtensions
    {
        /// <summary>
        ///     Registers classes, annotate with <see cref="SingletonServiceAttribute"/>,
        ///     or <see cref="TransientServiceAttribute"/> within the given assembly.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="assembly">The assembly to scan for annotated service classes.</param>
        public static void AddAnnotatedServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var descriptors = assembly
                .GetTypesWithAttribute<AnnotatedServiceAttribute>()
                .Select(x => x.Attribute.Describe(x.Type));

            services.Add(descriptors);
        }

        /// <summary>
        ///     Registers classes, annotate with <see cref="SingletonServiceAttribute"/>,
        ///     or <see cref="TransientServiceAttribute"/> within the given assembly.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        public static void AddAnnotatedServicesFromAssembly(this IServiceCollection services)
        {
            var descriptors = Assembly
                .GetCallingAssembly()
                .GetTypesWithAttribute<AnnotatedServiceAttribute>()
                .Select(x => x.Attribute.Describe(x.Type));

            services.Add(descriptors);
        }

        /// <summary>
        ///     Registers classes, annotate with <see cref="SingletonServiceAttribute"/>,
        ///     or <see cref="TransientServiceAttribute"/> within the given assembly.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="assemblyMarkers">The assemblies to scan for annotated service classes.</param>
        public static void AddAnnotatedServicesFromAssembly(this IServiceCollection services, params Type[] assemblyMarkers)
        {
            var descriptors = assemblyMarkers
                .Select(marker => marker.Assembly)
                .SelectMany(assembly => assembly
                    .GetTypesWithAttribute<AnnotatedServiceAttribute>()
                    .Select(t => t.Attribute.Describe(t.Type)));

            services.Add(descriptors);
        }
    }
}
