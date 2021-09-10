using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.Models
{
    public class TankProgressBar:DefaultControl
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
    }
}
