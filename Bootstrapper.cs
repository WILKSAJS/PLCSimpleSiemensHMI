﻿using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using PLCSiemensSymulatorHMI.ExternalComponents.Services;
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

            // repository
            _container.Singleton<IBasePlcRepository, XmlPlcRepository>();

            // conductoris
            _container.Singleton<ShellConductorViewModel>();

            // viewModels
            _container.PerRequest<HmiStatusBarViewModel>();

            _container.Singleton<PlcListViewModel>();
            _container.Singleton<CreatePlcViewModel>();
            _container.Singleton<EditPlcViewModel>();
            _container.Singleton<SettingsViewModel>();

            _container.PerRequest<SemaphoreViewModel>();
            _container.PerRequest<MonostableButtonViewModel>();
            _container.Singleton<CreateControlViewModel>();
            _container.Singleton<ControlsHolderViewModel>();
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
