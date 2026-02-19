using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TaskManagerWPF.Models;  // если Priority находится в этом namespace

namespace TaskManagerWPF.Converters  // ДОЛЖНО БЫТЬ ИМЕННО ТАК
{
    public class PriorityToBrushConverter : IValueConverter  // public обязателен
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                switch (priority)
                {
                    case Priority.Low:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                    case Priority.Medium:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
                    case Priority.High:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}