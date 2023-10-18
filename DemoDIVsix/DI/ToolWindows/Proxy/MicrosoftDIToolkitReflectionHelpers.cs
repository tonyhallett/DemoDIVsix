using Community.VisualStudio.Toolkit.DependencyInjection.Core;
using Community.VisualStudio.Toolkit.DependencyInjection.Microsoft;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows
{
    public static class MicrosoftDIToolkitReflectionHelpers
    {
        private static readonly Type OpenSToolkitServiceProviderType = typeof(SToolkitServiceProvider<>);
        public static bool IsMicrosoftDIToolkitPackage(Type type)
        {
            var baseType = type.BaseType;
            if (baseType == null)
            {
                return false;
            }
            if (baseType.IsGenericType)
            {
                var genericTypeDefinition = baseType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(MicrosoftDIToolkitPackage<>))
                {
                    return true;
                }
            }
            return IsMicrosoftDIToolkitPackage(baseType);
        }

        public static Type GetSToolkitServiceProviderType(Type toolkitPackageType)
        {
            return OpenSToolkitServiceProviderType.MakeGenericType(toolkitPackageType);
        }

    }

}
