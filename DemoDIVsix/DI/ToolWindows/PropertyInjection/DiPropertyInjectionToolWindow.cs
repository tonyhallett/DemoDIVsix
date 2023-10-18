using Community.VisualStudio.Toolkit.DependencyInjection.PropertyInjection;

namespace Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.PropertyInjection
{
    public abstract class DiPropertyInjectionToolWindow<T> : BaseToolWindow<T> where T : BaseToolWindow<T>, new()
    {
#pragma warning disable IDE0052 // Remove unread private members
        private IPropertyInjector propertyDescriptor = InjectAttributeInjector.Instance;
#pragma warning restore IDE0052 // Remove unread private members
        protected IPropertyInjector PropertyInjector 
        { 
            set 
            {
                propertyDescriptor = value;
            }
        } 
        public DiPropertyInjectionToolWindow()
        {
            InjectableToolWindows.Add(this);
        }
    }
}