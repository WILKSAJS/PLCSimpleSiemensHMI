using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PLCSiemensSymulatorHMI.CustomControls.Models
{
    [XmlInclude(typeof(SemaphoreControl))]
    [XmlInclude(typeof(BistableButton))]
    [XmlInclude(typeof(IntTextBox))]
    [XmlInclude(typeof(MonostableButton))]
    [XmlInclude(typeof(RealTextBox))]
    [XmlInclude(typeof(EmergencyButton))]
    [XmlInclude(typeof(TankProgressBar))]
    [Serializable]
    public class DefaultControl
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
