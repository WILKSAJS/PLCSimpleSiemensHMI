using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public enum ProgrssBarColor
    {
        Red,
        Green,
        Blue,
        Lime,
        Orange,
        Brown
    }

    public class CreateControlDetailViewModel: Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly DefaultControl _defaultControl;
        public CreateControlDetailViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, DefaultControl defaultControl)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _defaultControl = defaultControl;
            PorgressBarColor = Enum.GetValues(typeof(ProgrssBarColor)).Cast<ProgrssBarColor>().ToList();

            //initialize
            MinimumValue = "";
            MaximumValue = "";
        }

        #region Control Properties
        private string _minimumValue;

        public string MinimumValue
        {
            get { return _minimumValue; }
            set => Set(ref _minimumValue, value);
        }
        private string _maximumValue;

        public string MaximumValue
        {
            get { return _maximumValue; }
            set => Set(ref _maximumValue, value);
        }

        public IReadOnlyList<ProgrssBarColor> PorgressBarColor { get; }

        private ProgrssBarColor _selectedPorgressBarColor;

        public ProgrssBarColor SelectedPorgressBarColor
        {
            get { return _selectedPorgressBarColor; }
            set => Set(ref _selectedPorgressBarColor, value);
        }

        #endregion

        #region Sumbit processing
        public void Submit()
        {
            _eventAggregator.PublishOnUIThread(new CreateControlMessage()
            {
                ControlName = _defaultControl.ControlName,
                DataBlock = _defaultControl.DataBlock,
                Index = _defaultControl.Index,
                Offset = _defaultControl.Offset,
                ControlType = _defaultControl.ControlType,
                AdditionalControlInfo = new string[3] {MinimumValue, MaximumValue, SelectedPorgressBarColor.ToString()}
            });
            
            TryClose();
        }

        public bool CanSubmit
        {
            get { return AreInputsValid(); }
        }

        // regex for int or real number only
        Regex MinMaxRegex = new Regex(@"^-?[0-9][0-9,\.]*$", RegexOptions.IgnoreCase);
        private bool AreInputsValid()
        {
            return (String.IsNullOrEmpty(MinimumValue) ? false : MinMaxRegex.IsMatch(MinimumValue))
                && (String.IsNullOrEmpty(MaximumValue) ? false : MinMaxRegex.IsMatch(MaximumValue))
                && IsMaxHigherThanMin(MinimumValue, MaximumValue);
        }

        private bool IsMaxHigherThanMin(string minValue, string maxValue)
        {
            double doubleMin;
            double doubleMax;
            double.TryParse(minValue,out doubleMin);
            double.TryParse(maxValue, out doubleMax);
            return doubleMax > doubleMin;
        }

        public void TextChanged()
        {
            this.NotifyOfPropertyChange(nameof(CanSubmit));
        }
        #endregion
    }
}
