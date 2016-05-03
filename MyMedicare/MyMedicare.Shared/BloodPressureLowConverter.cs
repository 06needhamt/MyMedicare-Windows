using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MyMedicare
{
    class BloodPressureLowConverter : IValueConverter
    {
        double num;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is Record))
                throw new ArgumentException("value to convert must be a Record");
            num = ((Record)value).BloodPressureLow;
            if (num < 80)
                return new SolidColorBrush(Colors.Green);
            else if (num < 110)
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
