using PLCSiemensSymulatorHMI.CustomControls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Messages
{
    public class ControlEditEventArgs:EventArgs
    {
        public string NewDefaultControlName { get; set; }
        public string NewDefaultControlOffset { get; set; }
        public string NewDefaultControlIndex { get; set; }
        public string NewDefaultControlDatablock { get; set; }
    }
}
