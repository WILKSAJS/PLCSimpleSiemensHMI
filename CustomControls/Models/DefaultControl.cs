using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class DefaultControl : Screen
    {
        public int Id { get; set; }
        public string ControlName { get; set; }
        public string DataBlock { get; set; }
        public string Index { get; set; }
        public string Offset { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string DbBlockAdress
        {
            get { return $"{this.DataBlock}.{this.Index}.{this.Offset}"; }
        }
        public ControlType ControlType { get; set; }

    }
}
