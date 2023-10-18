using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Returns the private FieldInfo matching 'name' from either type 't' itself,
        /// or its most-derived base type (unlike 'System.Type.GetField').
        /// </summary>
        /// <returns> Returns 'null' if no match is found. </returns>
        public static FieldInfo GetPrivateField(this Type t, string name)
        {
            const BindingFlags bf = BindingFlags.Instance |
                                    BindingFlags.NonPublic |
                                    BindingFlags.DeclaredOnly;

            FieldInfo fi;
            while ((fi = t.GetField(name, bf)) == null && (t = t.BaseType) != null)
                ;
            return fi;
        }
    }
}