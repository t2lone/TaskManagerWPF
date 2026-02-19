using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TaskManagerWPF.Converters
{
    public class DueDateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dueDate && dueDate.Date < DateTime.Today)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")); // красный
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")); // обычный цвет
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}