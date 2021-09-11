using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PLCSiemensSymulatorHMI.CustomControls.Models
{
    public class TankProgressBar:DefaultControl
    {
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public Color ProcessColor { get; set; }
    }
}
