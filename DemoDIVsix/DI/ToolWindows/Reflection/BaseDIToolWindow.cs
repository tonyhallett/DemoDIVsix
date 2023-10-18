using Microsoft.VisualStudio.Shell;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection
{
    // this mirrors what the Toolkit does - is constructed by the service provider
    // uses reflection to fullfill the base class and to add the
    // THIS DOES NOT REQUIRE RegisterToolWindows(this AsyncPackage package, params Assembly[] assemblies)
    // Uses own method
    public abstract class BaseDIToolWindow<T> : BaseToolWindow<T> where T : BaseToolWindow<T>, new()
    {
        private static readonly Type BaseToolWindowType = typeof(BaseToolWindow<T>);

        public BaseDIToolWindow() { }
        public BaseDIToolWindow(AsyncPackage package)
        {
            EnsureProvidesToolWindow(package);
            SetPackageProperties(package);
            SetStaticImplementationField();
            AddToolWindow();
        }

        private void EnsureProvidesToolWindow(AsyncPackage package)
        {
            // Verify that the package has a ProvideToolWindow attribute for this tool window.
            ProvideToolWindowAttribute[] toolWindowAttributes = (ProvideToolWindowAttribute[])package.GetType().GetCustomAttributes(typeof(ProvideToolWindowAttribute), true);
            ProvideToolWindowAttribute foundToolWindowAttr = toolWindowAttributes.FirstOrDefault(a => a.ToolType == this.PaneType);
            if (foundToolWindowAttr == null)
            {
                Debug.Fail($"The tool window '{this.GetType().Name}' requires a ProvideToolWindow attribute on the package.");  // For testing debug build of the toolkit (not for users of the release-built nuget package).
                throw new InvalidOperationException($"The tool window '{this.GetType().Name}' requires a ProvideToolWindow attribute on the package.");
            }
        }

        private void SetPackageProperties(AsyncPackage package)
        {
            var packageProperty = BaseToolWindowType.GetProperty("Package", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            packageProperty.SetValue(this, package as ToolkitPackage);
            var staticPackageField = BaseToolWindowType.GetField("_package", BindingFlags.Static | BindingFlags.NonPublic);
            staticPackageField.SetValue(null, package as ToolkitPackage);
        }

        private void SetStaticImplementationField()
        {
            var staticImplementationField = BaseToolWindowType.GetField("_implementation", BindingFlags.Static | BindingFlags.NonPublic);
            staticImplementationField.SetValue(null, this);
        }

        private void AddToolWindow()
        {
            PackageReflectionMethods.AddToolWindow.Invoke(this.Package, new object[] { this });
        }
    }

}
