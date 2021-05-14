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
        SettingsPage
    }
    public class NavigateMessage
    {
        public CurrentPage CurrentPage { get; set; }
    }
}
