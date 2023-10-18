using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.MEF
{
    public static class MEFServiceCollectionExtension
    {
        private static readonly Func<Type,bool> IncludeAllFilter = (type) => true;
        private static readonly MethodInfo GetMefServiceMethodInfo = typeof(VS).GetMethod(nameof(VS.GetMefService), BindingFlags.Public | BindingFlags.Static);
        public static void AddMEF(this IServiceCollection serviceCollection,Func<Type,bool> filter = null,params Assembly[] assemblies)
        {
            if (!(assemblies?.Any() ?? false))
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            filter ??= IncludeAllFilter;
            var exportingTypes = assemblies.SelectMany(assembly => assembly.GetTypes().Where(t => t.CustomAttributes.Any(ca => ca.AttributeType == typeof(ExportAttribute))));
            exportingTypes = exportingTypes.Where(filter);
            foreach (var item in exportingTypes)
            {
                var exportAttributes = item.GetCustomAttributes<ExportAttribute>();
                foreach (var exportAttribute in exportAttributes)
                {
                    serviceCollection.Add(
                        new ServiceDescriptor(exportAttribute.ContractType, (_) => GetMefService(exportAttribute.ContractType), ServiceLifetime.Singleton)
                    );
                }
            }
        }

        private static object GetMefService(Type t)
        {
            var closedMethod = GetMefServiceMethodInfo.MakeGenericMethod(t);
            return closedMethod.Invoke(null, new object[] { });
        }
    }

    
}