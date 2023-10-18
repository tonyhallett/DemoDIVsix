using System;

namespace Community.VisualStudio.Toolkit.DependencyInjection.PropertyInjection
{
    public interface IPropertyInjector
    {
        void Inject(object injectTo, IServiceProvider serviceProvider);
    }
}
