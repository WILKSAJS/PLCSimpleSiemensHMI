﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Messages
{
    public enum ControlType
    {
        GreenSemaphore,
        OrangeSemaphore,
        RedSemaphore,
        BistableButton,
        MonostableButton,
        RealTextBox,
        IntegerTextbox,
        EmergencyButton
    }

    public class CreateControlMessage
    {
        public string ControlName { get; set; }
        public string DataBlock { get; set; }
        public string Index { get; set; }
        public string Offset { get; set; }
        public ControlType ControlType { get; set; }
    }
}
