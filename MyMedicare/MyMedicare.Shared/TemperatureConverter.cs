using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MyMedicare
{
    public class TemperatureConverter : IValueConverter
    {
        double num;
        Color col;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(!(value is Record))
                throw new ArgumentException("value to convert must be a Record");
                num = ((Record) value).Temperature;
            if (num >= 37 && num < 38)
                return new SolidColorBrush(Colors.Green);
            else if (num >= 38 && num < 39)
                return new SolidColorBrush(Colors.Yellow);
            else
                return new SolidColorBrush(Colors.Red); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
