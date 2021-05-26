using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
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
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            _container.Singleton<ShellConductorViewModel>();

            _container.Singleton<HmiStatusBarViewModel>();
            _container.Singleton<TopMenuViewModel>();
            _container.Singleton<ControlsViewModel>();
            _container.Singleton<SettingsViewModel>();
            _container.Singleton<PlcListViewModel>();


            _container.Singleton<Sharp7PlcService>();
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
