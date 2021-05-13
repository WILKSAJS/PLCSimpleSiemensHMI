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
        public ShellConductorViewModel(HmiStatusBarViewModel hmiStatusBarViewModel, ControlsViewModel controlsViewModel)
        {
            HmiStatusBar = hmiStatusBarViewModel;
            ControlsView = controlsViewModel;
        }

        public HmiStatusBarViewModel HmiStatusBar { get; }
        public ControlsViewModel ControlsView { get; }

    }
}
