using System.Collections.Generic;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.PropertyInjection
{
    internal static class InjectableToolWindows
    {
        private static readonly List<object> list = new ();
        
        public static void Add(object injectable) => list.Add(injectable);
        
        public static IReadOnlyCollection<object> Get {get;} = list.AsReadOnly();
    }
}