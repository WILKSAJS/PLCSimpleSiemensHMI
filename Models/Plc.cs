using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Models
{
    public class Plc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAdress { get; set; }
        public string Rack { get; set; }
        public string Slot { get; set; }
    }
}
