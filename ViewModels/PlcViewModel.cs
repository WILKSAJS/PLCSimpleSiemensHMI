using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class PlcViewModel
    {
        private readonly Plc _plc;

        public PlcViewModel(Plc plc)
        {
            _plc = plc;
        }

        public string Name => _plc.Name;
        public string IpAdress => _plc.IpAdress;
        public string Rack => _plc.Rack;
        public string Slot => _plc.Slot;
    }
}
