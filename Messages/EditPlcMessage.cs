using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Messages
{
    public class EditPlcMessage
    {
        public PlcViewModel PlcViewModel { get; set; }
        public Plc EditedPlc { get; set; }
    }
}
