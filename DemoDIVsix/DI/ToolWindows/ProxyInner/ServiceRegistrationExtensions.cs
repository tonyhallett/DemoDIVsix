using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.ProxyInner
{
    internal static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterToolWindowsWrapper(this IServiceCollection services, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
        {
            if (!(assemblies?.Any() ?? false))
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            foreach (var assembly in assemblies)
            {
                var toolWindowTypes = assembly.GetTypes().Where(t => typeof(BaseDiToolWindow).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var toolWindowType in toolWindowTypes)
                {
                    services.Add(new ServiceDescriptor(toolWindowType, toolWindowType, serviceLifetime));
                }
            }
            return services;

        }
    }

}