using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Messages
{
    public enum CurrentPage
    {
        MainPage,
        SettingsPage,
        CreatePlcPage,
        EditPlcPage
    }
    public class NavigateMessage
    {
        public CurrentPage CurrentPage { get; set; }
        public Plc Plc { get; set; }
        public object Sender { get; set; }
    }
}
