using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows
{

    internal static class ServiceRegistrationExtensions
    {
        private static readonly Type registrationType = typeof(BaseDIToolWindowRegistration<,>);
        private static Type GetToolWindowProviderType(Type derivedType)
        {
            if (derivedType == null) return null;

            var baseType = derivedType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType)
                {
                    var genericTypeDefinition = baseType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == registrationType)
                    {
                        return baseType.GenericTypeArguments[1];
                    }
                }
                baseType = baseType.BaseType;
            }
            return null;
        }

        public static IServiceCollection RegisterToolWindows(this IServiceCollection services, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
        {
            if (!(assemblies?.Any() ?? false))
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            foreach (var assembly in assemblies)
            {
                var toolWindowProviderTypes = assembly.GetTypes().Select(t => GetToolWindowProviderType(t)).Where(t => t != null);


                foreach (var toolWindowProviderType in toolWindowProviderTypes)
                    services.Add(new ServiceDescriptor(toolWindowProviderType, toolWindowProviderType, serviceLifetime));
            }
            return services;

        }
    }

}
