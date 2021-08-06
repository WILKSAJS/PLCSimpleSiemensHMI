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
    class BoolToBistableButtonColourConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)values[0];

            return boolValue == true ? Brushes.Gray : Brushes.WhiteSmoke;
            // POPRAWIC ZMIANE KOLORU!
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
