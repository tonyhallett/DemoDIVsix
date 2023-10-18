using Community.VisualStudio.Toolkit.DependencyInjection.PropertyInjection;
using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.PropertyInjection
{
    public static class PackageExtensions
    {
        public static void PropertyInjectToolWindows(this DIToolkitPackage package, params Assembly[] assemblies)
        {
            package.RegisterToolWindows(assemblies);
            foreach (var toolWindow in InjectableToolWindows.Get)
            {
                var propertyInjectorField = toolWindow.GetType().GetPrivateField("propertyDescriptor");
                var propertyInjector = propertyInjectorField.GetValue(toolWindow) as IPropertyInjector;
                propertyInjector.Inject(toolWindow, package.ServiceProvider);
            }
        }
    }
}
