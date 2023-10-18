using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.ProxyInner
{
    internal static class PackageRegistrationExtension
    {
        // this would go inside the toolkit
        public static void InitializeToolWindows(this IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {
            var baseDiToolWindowsSds = serviceCollection.Where(sd => typeof(BaseDiToolWindow).IsAssignableFrom(sd.ServiceType));
            foreach(var baseDiToolWindowSd in baseDiToolWindowsSds)
            {
                serviceProvider.GetRequiredService(baseDiToolWindowSd.ServiceType);
            }
        }

    }

}