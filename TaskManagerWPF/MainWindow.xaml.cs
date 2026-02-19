using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManagerWPF.Models;

namespace TaskManagerWPF
{
    public partial class MainWindow : Window
    {
        private DataService dataService;

        public MainWindow()
        {
            InitializeComponent();
            dataService = new DataService();
            LoadFilters();
            RefreshTasksList(dataService.Tasks);
        }

        private void LoadFilters()
        {
            comboCategoryFilter.ItemsSource = dataService.Categories;
            comboCategoryFilter.DisplayMemberPath = "Name";
            comboCategoryFilter.SelectedIndex = 0;

            comboPriorityFilter.ItemsSource = Enum.GetValues(typeof(Priority));
            comboPriorityFilter.SelectedIndex = -1;
        }

        private void RefreshTasksList(System.Collections.Generic.List<TaskItem> tasks)
        {
            listViewTasks.ItemsSource = null;
            listViewTasks.ItemsSource = tasks;
        }

        private void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            Priority? priority = comboPriorityFilter.SelectedItem as Priority?;
            Category category = comboCategoryFilter.SelectedItem as Category;
            bool? isCompleted = chkShowCompleted.IsChecked == true ? true :
                                (chkShowCompleted.IsChecked == false ? false : (bool?)null);

            var filtered = dataService.FilterTasks(priority, category, isCompleted);
            RefreshTasksList(filtered);
        }

        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            comboPriorityFilter.SelectedIndex = -1;
            comboCategoryFilter.SelectedIndex = 0;
            chkShowCompleted.IsChecked = false;
            RefreshTasksList(dataService.Tasks);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var taskWindow = new TaskWindow(dataService);
            if (taskWindow.ShowDialog() == true)
            {
                RefreshTasksList(dataService.Tasks);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTasks.SelectedItem is TaskItem selectedTask)
            {
                var taskWindow = new TaskWindow(dataService, selectedTask);
                if (taskWindow.ShowDialog() == true)
                {
                    RefreshTasksList(dataService.Tasks);
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTasks.SelectedItem is TaskItem selectedTask)
            {
                var result = MessageBox.Show($"Удалить задачу \"{selectedTask.Title}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    dataService.DeleteTask(selectedTask.Id);
                    RefreshTasksList(dataService.Tasks);
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // НОВЫЙ ОБРАБОТЧИК для клика по чекбоксу
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var task = checkBox.DataContext as TaskItem;
                if (task != null)
                {
                    // Сохраняем изменения (свойство IsCompleted уже обновлено через binding)
                    dataService.UpdateTask(task);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            dataService.SaveData();
            base.OnClosed(e);
        }
    }
}