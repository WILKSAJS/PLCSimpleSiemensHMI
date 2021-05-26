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
        private readonly IEventAggregator _eventAggregator;

        public ShellConductorViewModel(TopMenuViewModel topMenuViewModel,
            ControlsViewModel controlsViewModel, 
            SettingsViewModel settingsViewModel,
            PlcListViewModel plcListViewModel,
            IEventAggregator eventAggregator)
        {
            TopMenu = topMenuViewModel;
            PlcList = plcListViewModel;

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
                case CurrentPage.MainPage:
                    ActivateItem(_controlsViewModel);
                    break;
                case CurrentPage.SettingsPage:
                    ActivateItem(_settingsViewModel);
                    break;
                default:
                    break;
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
            ActivateItem(_controlsViewModel);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
        }


    }
}
