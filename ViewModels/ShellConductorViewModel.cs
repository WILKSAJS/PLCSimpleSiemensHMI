﻿using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.ExternalComponents.Services;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ShellConductorViewModel: Conductor<Screen>.Collection.OneActive, IHandle<NavigateMessage>
    {
        private readonly CreatePlcViewModel _createPlcViewModel;
        private readonly IBasePlcRepository _plcRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        public ShellConductorViewModel(
            PlcListViewModel plcListViewModel,
            CreatePlcViewModel createPlcViewModel,
            IBasePlcRepository plcRepository,
            IEventAggregator eventAggregator,
            IWindowManager windowManager)
        {
            // TopMenu = topMenuViewModel;
            PlcList = plcListViewModel;
            _createPlcViewModel = createPlcViewModel;
            _plcRepository = plcRepository;
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
        }
        //public TopMenuViewModel TopMenu { get; }
        public PlcListViewModel PlcList { get; }
        
        public void Handle(NavigateMessage message)
        {
            switch (message.CurrentPage)
            {
                case CurrentPage.ControlPage:
                    ActivateItem(new ControlsHolderViewModel(_plcRepository, new Sharp7PlcService(), (PlcViewModel)message.Sender, _eventAggregator, _windowManager));
                    break;
                //case CurrentPage.SettingsPage:
                //    ActivateItem(_settingsViewModel);
                //    break;
                case CurrentPage.CreatePlcPage:
                    ActivateItem(_createPlcViewModel);
                    break;
                case CurrentPage.EditPlcPage:
                    ActivateItem(new EditPlcViewModel(message.Plc, PlcList, (PlcViewModel)message.Sender, _eventAggregator));
                    break;
                default:
                    break;
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
        }


    }
}
