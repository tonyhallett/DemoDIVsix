using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace DemoDIVsix
{
    [Export(typeof(ReflectionMeffed))]
    public class ReflectionMeffed
    {
        public string GetTitle() => "Reflection";
    }
    public class ReflectionToolWindow : BaseDIToolWindow<ReflectionToolWindow>
    {
        public ReflectionToolWindow() {}
        public ReflectionToolWindow(AsyncPackage package, ReflectionMeffed meffed) : base(package) { 
            this.meffed = meffed;
        }
        public override Type PaneType => typeof(Pane);

        private readonly ReflectionMeffed meffed;

        public override async System.Threading.Tasks.Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return new ToolWindowControl();
        }

        public override string GetTitle(int toolWindowId)
        {
            return meffed.GetTitle();
        }

        [Guid("0571938F-383D-4031-B81E-5BBC81911CC4")]
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