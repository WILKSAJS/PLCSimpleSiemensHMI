using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.Models
{
    public class SemaphoreControl: DefaultControl
    {
        public bool SemaphoreState { get; set; }
        public BrushConverterColours SemaphoreColour { get; set; }
        
    }
}
