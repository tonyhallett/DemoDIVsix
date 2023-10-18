using Community.VisualStudio.Toolkit.DependencyInjection.Core;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace DemoDIVsix
{
    [Export(typeof(ProxyMeffed))]
    public class ProxyMeffed
    {
        public string GetTitle() => "Proxy";
        
    }
    public class ProxyToolWindowProvider : Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.IToolWindowProvider
    {
        private readonly ProxyMeffed meffed;
        public ProxyToolWindowProvider(ProxyMeffed meffed)
        {
            this.meffed = meffed;
        }

        public Type PaneType => typeof(Pane);
        
        public async System.Threading.Tasks.Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return new ToolWindowControl();
        }

        public string GetTitle(int toolWindowId)
        {
            return meffed.GetTitle();
        }

        [Guid("0571938F-383D-4031-B81E-5BBC81911CC3")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                // Set an image icon for the tool window
                BitmapImageMoniker = KnownMonikers.StatusInformation;
            }
        }


    }

    public class ProxyToolWindow : BaseDIToolWindowRegistration<ProxyToolWindow, ProxyToolWindowProvider> { }
}