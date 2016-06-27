using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ToDoList.Core
{
    public static class TaskManager
    {

        static TaskManager()
        {
        }

        public static Task GetTask(int id)
        {
            return TaskRepositoryADO.GetTask(id);
        }

        public static IList<Task> GetTasks()
        {
            return new List<Task>(TaskRepositoryADO.GetTasks());
        }

        public static int SaveTask(Task item)
        {
            return TaskRepositoryADO.SaveTask(item);
        }

        public static int DeleteTask(int id)
        {
            return TaskRepositoryADO.DeleteTask(id);
        }
    }
}