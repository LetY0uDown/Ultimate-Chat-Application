using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WPFLibrary.DI.Attributes;

namespace WPFLibrary.DI.Extensions;

public static class ServiceCollectionExtensions
{
    private static Type[] GetTypes ()
    {
        return Assembly.GetExecutingAssembly()
                       .GetTypes();
    }

    private static Type GetBaseType (Type type)
    {
        var interfaces = type.GetInterfaces();

        foreach (var i in interfaces) {
            var attributes = i.GetCustomAttributes();

            if (attributes.Any(a => a is BaseTypeAttribute)) {
                return i;
            }
        }

        throw new InvalidOperationException($"Can not get base type for {type.FullName}");
    }

    private static Lifetime DetermineLifetime (Type service, bool inheritAttributes)
    {
        if (service.GetCustomAttributes(inheritAttributes)
                                       .FirstOrDefault(s => s is LifetimeAttribute)
                                       is not LifetimeAttribute lifetimeAttribute)
            throw new InvalidOperationException($"Cannot determine lifetime of the service {service.FullName}");

        return lifetimeAttribute.Lifetime;
    }

    private static void AddServiceTypesAsBaseTypes (IServiceCollection serviceCollection, IEnumerable<Type> services, bool inheritAttributes)
    {
        foreach (var service in services) {
            var lifetime = DetermineLifetime(service, inheritAttributes);

            var baseType = GetBaseType(service);

            if (lifetime == Lifetime.Singleton) {
                serviceCollection.AddSingleton(baseType, service);
                continue;
            }

            if (lifetime == Lifetime.Transient) {
                serviceCollection.AddTransient(baseType, service);
            }
        }
    }

    private static void AddServiceTypesDirectly (IServiceCollection serviceCollection, IEnumerable<Type> services, bool inheritAttributes)
    {
        foreach (var service in services) {
            var lifetime = DetermineLifetime(service, inheritAttributes);

            if (lifetime == Lifetime.Singleton) {
                serviceCollection.AddSingleton(service, service);
                continue;
            }

            if (lifetime == Lifetime.Transient) {
                serviceCollection.AddTransient(service, service);
            }
        }
    }

    private static void AddServiceTypes (IServiceCollection serviceCollection, IEnumerable<Type> services, bool useBaseType, bool inheritAttributes = false)
    {
        if (useBaseType)
            AddServiceTypesAsBaseTypes(serviceCollection, services, inheritAttributes);
        else
            AddServiceTypesDirectly(serviceCollection, services, inheritAttributes);
    }

    public static void RegisterViewModels<TViewModel> (this IServiceCollection services)
    {
        List<Type> viewModels = GetTypes().Where(t => t is { IsClass: true, IsAbstract: false } &&
                                                 t.IsAssignableTo(typeof(TViewModel))).ToList();

        AddServiceTypes(services, viewModels, useBaseType: false, inheritAttributes: true);
    }
}