using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.PropertyInjection
{
    internal class InjectAttributeInjector : IPropertyInjector
    {
        private InjectAttributeInjector()
        {

        }
        public static InjectAttributeInjector Instance { get; } = new InjectAttributeInjector();
        private object _injectTo;
        private Type _injectToType;
        private IServiceProvider _serviceProvider;
        public void Inject(object injectTo, IServiceProvider serviceProvider)
        {
            _injectTo = injectTo;
            _injectToType = injectTo.GetType();
            _serviceProvider = serviceProvider;

            InjectX(_injectToType.GetFields, fi => fi.FieldType, (fi, o) => fi.SetValue(_injectTo, o));
            InjectX(_injectToType.GetProperties, pi => pi.PropertyType, (pi, o) => pi.SetValue(_injectTo, o));
        }

        private void InjectX<TFieldOrProperty>(Func<BindingFlags, TFieldOrProperty[]> getMembers,Func<TFieldOrProperty,Type> getMemberType,Action<TFieldOrProperty,object> setValue) where TFieldOrProperty:MemberInfo
        {
            var members = getMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var member in members)
            {
                var injectAttribute = member.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute != null)
                {
                    var dependency = _serviceProvider.GetRequiredService(getMemberType(member));
                    setValue(member, dependency);
                }
            }
        }
    }
}
