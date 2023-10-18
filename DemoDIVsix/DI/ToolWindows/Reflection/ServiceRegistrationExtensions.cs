using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection.Reflection
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterToolWindows(this IServiceCollection services, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
        {
            if (!(assemblies?.Any() ?? false))
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            foreach (var assembly in assemblies)
            {
                var diToolWindowTypes = assembly.GetTypes()
                    .Where(x => BaseDIToolWindowType.IsBaseDIToolWindowType(x));

                foreach (var diToolWindowType in diToolWindowTypes)
                    services.Add(new ServiceDescriptor(diToolWindowType, diToolWindowType, serviceLifetime));
            }
            return services;

        }
    }
}
