using System;
using System.Linq;
using System.Windows;
using TaskManagerWPF.Models;

namespace TaskManagerWPF
{
    public partial class TaskWindow : Window
    {
        private DataService dataService;
        private TaskItem currentTask;

        // Свойство для привязки заголовка окна
        public string WindowTitle { get; set; }

        public TaskWindow(DataService service, TaskItem task = null)
        {
            InitializeComponent();
            dataService = service;
            currentTask = task;

            // Устанавливаем заголовок в зависимости от режима
            WindowTitle = currentTask == null ? "Новая задача" : "Редактирование задачи";
            this.DataContext = this; // Важно: устанавливаем DataContext на само окно для привязки

            // Заполнение категорий
            comboCategory.ItemsSource = dataService.Categories;
            comboCategory.SelectedIndex = 0;

            // Заполнение приоритетов
            comboPriority.ItemsSource = Enum.GetValues(typeof(Priority));
            comboPriority.SelectedIndex = 0;

            if (currentTask != null)
            {
                // Режим редактирования: заполняем поля
                txtTitle.Text = currentTask.Title;
                txtDescription.Text = currentTask.Description;
                datePickerDue.SelectedDate = currentTask.DueDate;
                comboCategory.SelectedItem = currentTask.Category;
                comboPriority.SelectedItem = currentTask.Priority;
                chkIsCompleted.IsChecked = currentTask.IsCompleted;
            }
            else
            {
                // Новая задача: устанавливаем дату по умолчанию
                datePickerDue.SelectedDate = DateTime.Today;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название задачи.");
                return;
            }

            if (currentTask == null)
            {
                // Создание новой задачи
                var newTask = new TaskItem
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    DueDate = datePickerDue.SelectedDate ?? DateTime.Today,
                    Category = comboCategory.SelectedItem as Category,
                    Priority = (Priority)comboPriority.SelectedItem,
                    IsCompleted = chkIsCompleted.IsChecked ?? false
                };
                dataService.AddTask(newTask);
            }
            else
            {
                // Обновление существующей задачи
                currentTask.Title = txtTitle.Text.Trim();
                currentTask.Description = txtDescription.Text.Trim();
                currentTask.DueDate = datePickerDue.SelectedDate ?? DateTime.Today;
                currentTask.Category = comboCategory.SelectedItem as Category;
                currentTask.Priority = (Priority)comboPriority.SelectedItem;
                currentTask.IsCompleted = chkIsCompleted.IsChecked ?? false;
                dataService.UpdateTask(currentTask);
            }

            DialogResult = true; // Закрываем окно с положительным результатом
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Закрываем окно с отрицательным результатом
            Close();
        }
    }
}