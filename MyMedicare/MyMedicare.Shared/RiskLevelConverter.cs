using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MyMedicare
{
    public class RiskLevelConverter : IValueConverter
    {
        EnumRiskLevel level;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is Record))
                throw new ArgumentException("value to convert must be a Record");
            level = ((Record)value).RiskLevel;
            if (level == EnumRiskLevel.LOW)
                return new SolidColorBrush(Colors.Green);
            else if (level == EnumRiskLevel.MEDIUM)
                return new SolidColorBrush(Colors.Yellow);
            else if (level == EnumRiskLevel.HIGH)
                return new SolidColorBrush(Colors.Red);
            else
                return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
