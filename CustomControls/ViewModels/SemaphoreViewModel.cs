using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class SemaphoreViewModel: BaseControlViewModel
    {

        public SemaphoreViewModel(BrushConverterColours brushConverterColours, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel, IWindowManager windowManager)
            :base(plcRepository,defaultControl,plcViewModel, windowManager)
        {
            SemaphoreColour = brushConverterColours;
        }

        private bool _semaphoreState;
        public bool SemaphoreState
        {
            get { return _semaphoreState; }
            set => Set(ref _semaphoreState, value);
        }

        private BrushConverterColours _semaphoreColour;
        public BrushConverterColours SemaphoreColour
        {
            get { return _semaphoreColour; }
            set => Set(ref _semaphoreColour, value);
        }

        // Read bit that is converted to Semaphore Brush.Colour
        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            SemaphoreState = await plcService.ReadBit(DbBlockAdress);
        }

    }
}
