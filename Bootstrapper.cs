using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLCSiemensSymulatorHMI
{
    class Bootstrapper: BootstrapperBase
    {
        SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellConductorViewModel>();
        }

        protected override void Configure()
        {
            // internals
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // conductoris
            _container.Singleton<ShellConductorViewModel>();

            // viewModels
            _container.Singleton<HmiStatusBarViewModel>();
            _container.Singleton<TopMenuViewModel>();
            _container.Singleton<ControlsViewModel>();
            _container.Singleton<SettingsViewModel>();
            _container.Singleton<PlcListViewModel>();

            // service
            _container.Singleton<Sharp7PlcService>();

            // repository
            _container.Singleton<PlcRepository>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
