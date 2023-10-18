using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using DemoDIVsix;
using Microsoft.VisualStudio.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.ProxyInner;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows
{
    [Export(typeof(WrapperMeffed))]
    public class WrapperMeffed
    {
        public string GetTitle() => "Wrapper";
    }

    public class WrapperToolWindow : BaseDiToolWindow {
        private readonly WrapperMeffed meffed;

        public WrapperToolWindow(DIToolkitPackage diToolkitPackage, WrapperMeffed meffed) : base(diToolkitPackage)
        {
            this.meffed = meffed;
        }
        

        protected override Type PaneType => typeof(Pane);

        protected override async Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return new ToolWindowControl();
        }

        protected override string GetTitle(int toolWindowId)
        {
            return meffed.GetTitle();
        }

        [Guid("0571938F-383D-4031-B81E-5BBC81911CC9")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                // Set an image icon for the tool window
                BitmapImageMoniker = KnownMonikers.StatusInformation;
            }
        }
    }

}