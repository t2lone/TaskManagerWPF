using System;
using System.Windows.Media;  // Обязательно добавьте эту строку

namespace TaskManagerWPF.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Category Category { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }

        // НОВЫЕ СВОЙСТВА (добавлены внутрь класса)
        public Brush PriorityColor
        {
            get
            {
                switch (Priority)
                {
                    case Priority.Low:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                    case Priority.Medium:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
                    case Priority.High:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }
        }

        public Brush DueDateColor
        {
            get
            {
                if (DueDate.Date < DateTime.Today)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")); // красный
                else
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")); // тёмно-серый
            }
        }
    }
}