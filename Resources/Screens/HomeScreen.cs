using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ToDoList
{
    [Activity(Label = "ToDoList", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeScreen : Activity
    {
     
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "H" layout resource
            SetContentView(Resource.Layout.HomeScreen);

            ImageButton addNewTask = FindViewById<ImageButton>(Resource.Id.imbNewTaskIcon);
            addNewTask.Click += (sender, e) =>
            {
                StartActivity(typeof(NewTaskScreen));
            };

          
        }
    }
}

