using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Messages
{
    public class ChangeIntervalTimeArgs: EventArgs
    {
        public int NewIntervalTime { get; set; }
    }
}
