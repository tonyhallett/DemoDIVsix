using Community.VisualStudio.Toolkit.DependencyInjection.PropertyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.PropertyInjection;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace DemoDIVsix
{
    [Export(typeof(PropertyInjectionMeffed))]
    public class PropertyInjectionMeffed
    {
        public string GetTitle() => "Property";
        
    }

    public class PropertyInjectionToolWindow : DiPropertyInjectionToolWindow<PropertyInjectionToolWindow>
    {
        [Inject]
        private PropertyInjectionMeffed meffed;
        public PropertyInjectionToolWindow()
        {
        }
        public override Type PaneType => typeof(Pane);


        public override async System.Threading.Tasks.Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return new ToolWindowControl();
        }

        public override string GetTitle(int toolWindowId)
        {
            return meffed.GetTitle();
        }

        [Guid("0571938F-383D-4031-B81E-5BBC81911CC5")]
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