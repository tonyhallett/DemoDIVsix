global using Community.VisualStudio.Toolkit;
global using System;
global using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell;
using Community.VisualStudio.Toolkit.DependencyInjection.MEF;
using Community.VisualStudio.Toolkit.DependencyInjection.Microsoft;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Threading;
using ReflectionRegistration = Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection.Reflection.ServiceRegistrationExtensions;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.Reflection;
using Community.VisualStudio.Toolkit.DependencyInjection;
using System.Collections.Generic;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.PropertyInjection;
using Community.VisualStudio.Toolkit.DependencyInjection.ToolWindows.ProxyInner;

namespace DemoDIVsix
{
    interface IToolWindowTypeSpecificInitialization
    {
        void InitializeServices(IServiceCollection services);
        void Initialize(DIToolkitPackage package);
        Task ShowAsync();
    }

    class ProxyInitialization : IToolWindowTypeSpecificInitialization
    {
        public void InitializeServices(IServiceCollection services)
        {
            // registers each of the TToolWindowProvider in the service collection
            services.RegisterToolWindows(ServiceLifetime.Singleton); //Proxy
        }
        public void Initialize(DIToolkitPackage package)
        {
            // normally called on this
            // normal registration - static InitializeAsync
            package.RegisterToolWindows();
        }

        public Task ShowAsync()
        {
            return ProxyToolWindow.ShowAsync();
        }
    }

    class ReflectionInitialization : IToolWindowTypeSpecificInitialization
    {
        private IServiceCollection services;
        public void InitializeServices(IServiceCollection services)
        {
            this.services = services;
            // would use as extension if used alone - extension clashes with proxy
            ReflectionRegistration.RegisterToolWindows(services, ServiceLifetime.Singleton);
        }

        public void Initialize(DIToolkitPackage package)
        {
            // would normally get ServiceProvider from  "this"
            services.RegisterToolWindows(package.ServiceProvider);
        }

        public Task ShowAsync()
        {
            return ReflectionToolWindow.ShowAsync();
        }
    }

    class PropertyInjectionInitialization : IToolWindowTypeSpecificInitialization
    {
        public void InitializeServices(IServiceCollection services)
        {
        }

        public void Initialize(DIToolkitPackage package)
        {
            // which is RegisterToolWindows followed by Injecting
            package.PropertyInjectToolWindows();
        }

        public Task ShowAsync()
        {
            return PropertyInjectionToolWindow.ShowAsync();
        }
    }

    class WrapperInitialization : IToolWindowTypeSpecificInitialization
    {
        public void Initialize(DIToolkitPackage package)
        {
            services.InitializeToolWindows(package.ServiceProvider);
        }
        private IServiceCollection services;
        public void InitializeServices(IServiceCollection services)
        {
            this.services = services;
            services.RegisterToolWindowsWrapper(ServiceLifetime.Singleton);
        }

        public Task ShowAsync()
        {
            return WrapperToolWindow.ShowAsync();
        }
    }

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.DemoDIVsixString)]
    [ProvideToolWindow(typeof(ReflectionToolWindow.Pane))]
    [ProvideToolWindow(typeof(ProxyToolWindowProvider.Pane))]
    [ProvideToolWindow(typeof(WrapperToolWindow.Pane))]
    [ProvideToolWindow(typeof(PropertyInjectionToolWindow.Pane))]
    public sealed class DemoDIVsixPackage : MicrosoftDIToolkitPackage<DemoDIVsixPackage>
    {
        private static Type ToolWindowType = typeof(WrapperToolWindow);// change this to demo
        private readonly Dictionary<Type, IToolWindowTypeSpecificInitialization> toolWindowTypeSpecificInitializations = new(){
            { typeof(ProxyToolWindow), new ProxyInitialization()},
            { typeof(ReflectionToolWindow), new ReflectionInitialization() },
            { typeof(PropertyInjectionToolWindow), new PropertyInjectionInitialization() },
            { typeof(WrapperToolWindow), new WrapperInitialization() },
        };
        internal static IToolWindowTypeSpecificInitialization toolWindowTypeSpecificInitialization;
        
        protected override void InitializeServices(IServiceCollection services)
        {
            services.RegisterCommands(ServiceLifetime.Singleton);
            services.AddMEF();
            if (ToolWindowType == typeof(PropertyInjectionToolWindow))
            {
                // All 3 examples are incompatible !
                // because property use normal RegisterToolWindows 
                // the Proxy is constructed but have not registered its TToolWindowProvider !
                new ProxyInitialization().InitializeServices(services);
            }
            toolWindowTypeSpecificInitialization.InitializeServices(services);
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            toolWindowTypeSpecificInitialization = toolWindowTypeSpecificInitializations[DemoDIVsixPackage.ToolWindowType];

            await base.InitializeAsync(cancellationToken, progress);
            toolWindowTypeSpecificInitialization.Initialize(this);

        }
    }
}