using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SP = Microsoft.VisualStudio.Shell.ServiceProvider;
using Microsoft.VisualStudio.Shell;
using System.Reflection;
using System.Linq;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows
{
    public abstract class BaseDIToolWindowRegistration<T, TToolWindowProvider> : BaseToolWindow<T> where T : BaseToolWindow<T>, new() where TToolWindowProvider : IToolWindowProvider
    {
        private readonly TToolWindowProvider toolWindowProvider;

        public BaseDIToolWindowRegistration()
        {
            var toolkitPackageType = Assembly.GetCallingAssembly().GetTypes().First(t => MicrosoftDIToolkitReflectionHelpers.IsMicrosoftDIToolkitPackage(t));

#pragma warning disable VSTHRD104 // Offer async methods
            var serviceProvider = ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                /*
                    Due to https://github.com/VsixCommunity/Community.VisualStudio.Toolkit.DependencyInjection/issues/13
                    When multiple packages are loaded, the first package loaded will be the one that is used to get the service provider.
                */
                var serviceType = MicrosoftDIToolkitReflectionHelpers.GetSToolkitServiceProviderType(toolkitPackageType);
                return SP.GlobalProvider.GetService(serviceType) as IServiceProvider;
            });
#pragma warning restore VSTHRD104 // Offer async methods
            toolWindowProvider = (TToolWindowProvider)serviceProvider.GetRequiredService(typeof(TToolWindowProvider));
        }


        public override Type PaneType => toolWindowProvider.PaneType;

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return toolWindowProvider.CreateAsync(toolWindowId, cancellationToken);
        }

        public override string GetTitle(int toolWindowId)
        {
            return toolWindowProvider.GetTitle(toolWindowId);
        }
    }

}
