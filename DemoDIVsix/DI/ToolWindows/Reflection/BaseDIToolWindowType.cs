using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection
{
    public static class BaseDIToolWindowType
    {
        public static bool IsBaseDIToolWindowType(Type derivedType)
        {
            if (derivedType == null) return false;

            var baseType = derivedType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType)
                {
                    var genericTypeDefinition = baseType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(BaseDIToolWindow<>))
                    {
                        return true;
                    }
                }
                baseType = baseType.BaseType;
            }
            return false;
        }

        public static void RegisterToolWindows(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var diToolWindowServices = services.Where(service => IsBaseDIToolWindowType(service.ImplementationType));
            foreach (var diToolWindowService in diToolWindowServices)
            {
                _ = serviceProvider.GetRequiredService(diToolWindowService.ImplementationType);
            }
        }
    }

}