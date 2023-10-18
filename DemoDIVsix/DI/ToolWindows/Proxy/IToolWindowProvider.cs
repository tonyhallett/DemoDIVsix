using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows
{
    public interface IToolWindowProvider
    {
        Type PaneType { get; }

        string GetTitle(int toolWindowId);

        Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken);
    }

}