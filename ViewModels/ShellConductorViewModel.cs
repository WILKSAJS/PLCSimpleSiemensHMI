using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ShellConductorViewModel: Conductor<Screen>.Collection.OneActive
    {
        private readonly ControlsViewModel _controlsViewModel;
        private readonly SettingsViewModel _settingsViewModel;

        public ShellConductorViewModel(HmiStatusBarViewModel hmiStatusBarViewModel, ControlsViewModel controlsViewModel, SettingsViewModel settingsViewModel)
        {
            HmiStatusBar = hmiStatusBarViewModel;
            _controlsViewModel = controlsViewModel;
            _settingsViewModel = settingsViewModel;
            
        }
        public HmiStatusBarViewModel HmiStatusBar { get; }

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(_controlsViewModel);
        }


    }
}
