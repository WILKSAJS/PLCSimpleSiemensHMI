using Caliburn.Micro;
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
        private readonly SettingsViewModel _settingsViewModel;

        public ShellConductorViewModel(
            PlcListViewModel plcListViewModel,
            CreatePlcViewModel createPlcViewModel,
            SettingsViewModel settingsViewModel,
            IBasePlcRepository plcRepository,
            IEventAggregator eventAggregator,
            IWindowManager windowManager)
        {
            PlcList = plcListViewModel;
            _createPlcViewModel = createPlcViewModel;
            _plcRepository = plcRepository;
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
        }
        public PlcListViewModel PlcList { get; }
        
        public void Handle(NavigateMessage message)
        {
            switch (message.CurrentPage)
            {
                case CurrentPage.ControlPage:
                    ActiveItem?.TryClose();
                    ActivateItem(new ControlsHolderViewModel(_plcRepository, new Sharp7PlcService(_settingsViewModel), (PlcViewModel)message.Sender, _eventAggregator, _windowManager));
                    break;
                case CurrentPage.SettingsPage:
                    ActiveItem?.TryClose();
                    ActivateItem(_settingsViewModel);
                    break;
                case CurrentPage.CreatePlcPage:
                    ActiveItem?.TryClose();
                    ActivateItem(_createPlcViewModel);
                    break;
                case CurrentPage.EditPlcPage:
                    ActiveItem?.TryClose();
                    ActivateItem(new EditPlcViewModel(message.Plc, PlcList, (PlcViewModel)message.Sender, _eventAggregator));
                    break;
                default:
                    break;
            }
        }

        public void NavigateToSettings()
        {
            Handle(new NavigateMessage() { CurrentPage = CurrentPage.SettingsPage, Sender = this });
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
