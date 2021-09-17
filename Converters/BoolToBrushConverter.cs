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
        Orange,
        Blue
    }

    [ValueConversion(typeof(bool), typeof(Brushes), ParameterType = typeof(string))]
    public class BoolToBrushConverter : IMultiValueConverter //IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values array is set in SemaphoreView and contains: [0] actual bool value - semaphore state, [1] - SemaphoreColour
            BrushConverterColours colour = (BrushConverterColours)Enum.Parse(typeof(BrushConverterColours), values[1].ToString());
            var boolValue = (bool)values[0];

            switch (colour)
            {
                case BrushConverterColours.Green:
                    return boolValue ? Brushes.Green : Brushes.Gray;
                case BrushConverterColours.Red:
                    return boolValue ? Brushes.Red : Brushes.Gray;
                case BrushConverterColours.Orange:
                    return boolValue ? Brushes.Orange : Brushes.Gray;
                case BrushConverterColours.Blue:
                    return boolValue ? Brushes.Blue : Brushes.Gray;
                default:
                    return Brushes.Gray;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
