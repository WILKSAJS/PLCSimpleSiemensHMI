using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ShellConductorViewModel: Conductor<Screen>.Collection.OneActive, IHandle<NavigateMessage>
    {
        private readonly ControlsViewModel _controlsViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly CreatePlcViewModel _createPlcViewModel;
        private readonly HmiStatusBarViewModel _hmiStatusBarViewModel;
        private readonly Sharp7PlcService _plcService;
        private readonly IEventAggregator _eventAggregator;

        public ShellConductorViewModel(TopMenuViewModel topMenuViewModel,
            ControlsViewModel controlsViewModel, 
            SettingsViewModel settingsViewModel,
            PlcListViewModel plcListViewModel,
            CreatePlcViewModel createPlcViewModel,
            HmiStatusBarViewModel hmiStatusBarViewModel,
            Sharp7PlcService plcService,
            IEventAggregator eventAggregator)
        {
            TopMenu = topMenuViewModel;
            PlcList = plcListViewModel;
            _createPlcViewModel = createPlcViewModel;
            _hmiStatusBarViewModel = hmiStatusBarViewModel;
            _plcService = plcService;
            _controlsViewModel = controlsViewModel;
            _settingsViewModel = settingsViewModel;
            _eventAggregator = eventAggregator;
        }
        public TopMenuViewModel TopMenu { get; }
        public PlcListViewModel PlcList { get; }

        public void Handle(NavigateMessage message)
        {
            switch (message.CurrentPage)
            {
                case CurrentPage.ControlPage:
                    ActivateItem(new ControlsViewModel(_plcService,_hmiStatusBarViewModel, (PlcViewModel)message.Sender));
                    break;
                case CurrentPage.SettingsPage:
                    ActivateItem(_settingsViewModel);
                    break;
                case CurrentPage.CreatePlcPage:
                    ActivateItem(_createPlcViewModel);
                    break;
                case CurrentPage.EditPlcPage:
                    ActivateItem(new EditPlcViewModel(message.Plc, PlcList, (PlcViewModel)message.Sender));
                    break;
                default:
                    break;
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
            // ActivateItem(_controlsViewModel);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
        }


    }
}
