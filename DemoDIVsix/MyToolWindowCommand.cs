using Community.VisualStudio.Toolkit.DependencyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection.Core;
using Microsoft.VisualStudio.Shell;

namespace DemoDIVsix
{
    [Command(PackageGuids.ControlsGuidString, PackageIds.cmdIdOpenToolWindow)]
    public sealed class MyToolWindowCommand : BaseDICommand
    {
        public MyToolWindowCommand(DIToolkitPackage package) : base(package) { }
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e){
            return DemoDIVsixPackage.toolWindowTypeSpecificInitialization.ShowAsync();
        }
    }
}