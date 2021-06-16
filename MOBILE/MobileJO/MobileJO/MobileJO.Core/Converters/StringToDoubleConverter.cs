using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileJO.Core.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double returnVal = 0;

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                return double.TryParse(value.ToString(), out returnVal) ? returnVal : 0;
            }

            return returnVal;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}