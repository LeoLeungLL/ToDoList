using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ToDoList.Core;

namespace ToDoList
{
    //[Activity(Label = "ToDoList", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "NewTaskScreen")]
    public class NewTaskScreen : Activity
    {

        Task newTask = new Task();
        EditText taskName;
        EditText taskDescription;
        EditText taskDueDate;
        EditText taskDueDateTime;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "H" layout resource
            SetContentView(Resource.Layout.NewTaskScreen);

            //set layout objects
            taskName = FindViewById<EditText>(Resource.Id.txtTaskName);
            taskDescription = FindViewById<EditText>(Resource.Id.txtTaskDescription);
            taskDueDate = FindViewById<EditText>(Resource.Id.txtDueDate);
            taskDueDate.Text = DateTime.Now.ToString("MMM dd, yyyy");
            taskDueDateTime = FindViewById<EditText>(Resource.Id.txtDueDateTime);
            taskDueDateTime.Text = DateTime.Now.ToString("H:mm");

            //cancel button returns Home
            ImageButton cancelNewTask= FindViewById<ImageButton>(Resource.Id.imbCancel);
            cancelNewTask.Click += (sender, e) =>
            {
                StartActivity(typeof(HomeScreen));
            };
            ImageButton createNewTask = FindViewById<ImageButton>(Resource.Id.imbAdd);
            createNewTask.Click += (sender, e) =>
            {
                CreateNewTask();
                StartActivity(typeof(HomeScreen));
                //Task.CreateNewTask
            };

        }

        private void CreateNewTask()
        {

            newTask.Name = taskName.Text;
            newTask.Description = taskDescription.Text;
            newTask.DueDate = DateTime.Parse(taskDueDate.Text + " " + taskDueDateTime.Text);
            TaskManager.SaveTask(newTask);
            
        }

        
    }
}

