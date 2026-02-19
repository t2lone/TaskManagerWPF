using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TaskManagerWPF.Models;
using Formatting = Newtonsoft.Json.Formatting;

namespace TaskManagerWPF
{
    public class DataService
    {
        public List<TaskItem> Tasks { get; private set; }
        public List<Category> Categories { get; private set; }
        private readonly string filePath = "tasks.json";

        public DataService()
        {
            // Инициализация списков
            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Работа" },
                new Category { Id = 2, Name = "Личное" },
                new Category { Id = 3, Name = "Учёба" }
            };
            Tasks = new List<TaskItem>();
            LoadData();
        }

        public void LoadData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            else
            {
                Tasks = new List<TaskItem>();
            }
        }

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(Tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // CRUD
        public void AddTask(TaskItem task)
        {
            task.Id = Tasks.Count > 0 ? Tasks.Max(t => t.Id) + 1 : 1;
            Tasks.Add(task);
            SaveData();
        }

        public void UpdateTask(TaskItem updatedTask)
        {
            var existing = Tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existing != null)
            {
                existing.Title = updatedTask.Title;
                existing.Description = updatedTask.Description;
                existing.DueDate = updatedTask.DueDate;
                existing.Category = updatedTask.Category;
                existing.Priority = updatedTask.Priority;
                existing.IsCompleted = updatedTask.IsCompleted;
                SaveData();
            }
        }

        public void DeleteTask(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                Tasks.Remove(task);
                SaveData();
            }
        }

        // Фильтрация
        public List<TaskItem> FilterTasks(Priority? priority = null, Category category = null, bool? isCompleted = null)
        {
            var query = Tasks.AsEnumerable();
            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);
            if (category != null)
                query = query.Where(t => t.Category?.Id == category.Id);
            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            return query.ToList();
        }
    }
}