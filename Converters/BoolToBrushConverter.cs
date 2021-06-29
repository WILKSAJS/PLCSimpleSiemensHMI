using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PLCSiemensSymulatorHMI.Converters
{
    public enum BrushConverterColours
    {
        Green,
        Red,
        Orange
    }

    [ValueConversion(typeof(bool), typeof(Brushes), ParameterType = typeof(string))]
    public class BoolToBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BrushConverterColours colour = (BrushConverterColours)Enum.Parse(typeof(BrushConverterColours), parameter.ToString());
            var boolValue = (bool)value;

            switch (colour)
            {
                case BrushConverterColours.Green:
                    return boolValue ? Brushes.Green : Brushes.Gray;
                case BrushConverterColours.Red:
                    return boolValue ? Brushes.Red : Brushes.Gray;
                case BrushConverterColours.Orange:
                    return boolValue ? Brushes.Orange : Brushes.Gray;
                default:
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
