using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.ProxyInner
{
    public abstract class BaseDiToolWindow
    {
        private static BaseDiToolWindow implementation;
        public BaseDiToolWindow(DIToolkitPackage diToolkitPackage)
        {
            implementation = this;
            DiBaseToolWindowProxy.Initialize(diToolkitPackage);
        }

        public static Task ShowAsync(int id = 0, bool create = true)
        {
            return DiBaseToolWindowProxy.ShowAsync(id, create);
        }
        
        public static Task HideAsync(int id = 0)
        {
            return DiBaseToolWindowProxy.HideAsync(id);
        }

        /// <summary>
        /// Gets the title to show in the tool window.
        /// </summary>
        /// <param name="toolWindowId">The ID of the tool window for a multi-instance tool window.</param>
        protected abstract string GetTitle(int toolWindowId);

        /// <summary>
        /// Gets the type of <see cref="ToolWindowPane"/> that will be created for this tool window.
        /// </summary>
        protected abstract Type PaneType { get; }

        /// <summary>
        /// Creates the UI element that will be shown in the tool window. 
        /// Use this method to create the user control or any other UI element that you want to show in the tool window.
        /// </summary>
        /// <param name="toolWindowId">The ID of the tool window instance being created for a multi-instance tool window.</param>
        /// <param name="cancellationToken">The cancellation token to use when performing asynchronous operations.</param>
        /// <returns>The UI element to show in the tool window.</returns>
        protected abstract Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken);

        /// <summary>
        /// Called when the <see cref="ToolWindowPane"/> has been initialized and "sited". 
        /// The pane's service provider can be used from this point onwards.
        /// </summary>
        /// <param name="pane">The tool window pane that was created.</param>
        /// <param name="toolWindowId">The ID of the tool window that the pane belongs to.</param>
        protected virtual void SetPane(ToolWindowPane pane, int toolWindowId)
        {
            // Consumers can override this if they need access to the pane.
        }

        private class DiBaseToolWindowProxy : BaseToolWindow<DiBaseToolWindowProxy>
        {
            internal BaseDiToolWindow implementation;
            public DiBaseToolWindowProxy()
            {
                implementation = BaseDiToolWindow.implementation;

            }

            public override string GetTitle(int toolWindowId) => implementation.GetTitle(toolWindowId);

            public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
            {
                return implementation.CreateAsync(toolWindowId, cancellationToken);
            }

            public override Type PaneType => implementation.PaneType;

            public override void SetPane(ToolWindowPane pane, int toolWindowId)
            {
                implementation.SetPane(pane, toolWindowId);
            }

        }

    }

}