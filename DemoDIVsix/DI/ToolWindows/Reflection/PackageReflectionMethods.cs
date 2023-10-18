using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection
{
    internal static class PackageReflectionMethods
    {
        public static readonly MethodInfo AddToolWindow = typeof(ToolkitPackage).GetMethod("AddToolWindow", BindingFlags.Instance | BindingFlags.NonPublic);
    }
    
}