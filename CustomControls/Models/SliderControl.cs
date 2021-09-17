using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PLCSiemensSymulatorHMI.CustomControls.Models
{
    public class SliderControl: DefaultControl
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public Color ProcessColor { get; set; }
    }
}
