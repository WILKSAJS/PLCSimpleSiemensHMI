using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class TopMenuViewModel: PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;
        
        public TopMenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;           
        }

        //public void NaviToMainPage()
        //{
        //    _eventAggregator.PublishOnUIThread(new NavigateMessage() { CurrentPage = CurrentPage.ControlPage });
        //}

        //public void NaviToMainSettings()
        //{
        //    _eventAggregator.PublishOnUIThread(new NavigateMessage() { CurrentPage = CurrentPage.SettingsPage });
        //}
    }
}
