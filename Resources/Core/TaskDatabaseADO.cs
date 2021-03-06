using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Data.Sqlite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace ToDoList.Core
{
    class TaskDatabase
    {

        static object locker = new object();

        public SqliteConnection connection;

        public string path;

        public TaskDatabase(string dbPath)
        {
            var output = "";
            path = dbPath;
            // create the tables
            bool exists = File.Exists(dbPath);

            if (!exists)
            {
                connection = new SqliteConnection("Data Source=" + dbPath);

                connection.Open();
                var commands = new[] {
                    "CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Description NTEXT, Complete INTEGER, DueDate DATETIME, CompleteDate DATETTIME);"
                };
                foreach (var command in commands)
                {
                    using (var c = connection.CreateCommand())
                    {
                        c.CommandText = command;
                        var i = c.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // already exists, do nothing. 
            }
            Console.WriteLine(output);
        }

        /// <summary>Convert from DataReader to Task object</summary>
		Task FromReader(SqliteDataReader r)
        {
            var t = new Task();
            t.ID = Convert.ToInt32(r["_id"]);
            t.Name = r["Name"].ToString();
            t.Description = r["Notes"].ToString();
            t.Complete = Convert.ToInt32(r["Done"]) == 1 ? true : false;
            return t;
        }

        public IEnumerable<Task> GetItems()
        {
            var tl = new List<Task>();

            lock (locker)
            {
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var contents = connection.CreateCommand())
                {
                    contents.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items]";
                    var r = contents.ExecuteReader();
                    while (r.Read())
                    {
                        tl.Add(FromReader(r));
                    }
                }
                connection.Close();
            }
            return tl;
        }

        public Task GetItem(int id)
        {
            var t = new Task();
            lock (locker)
            {
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items] WHERE [_id] = ?";
                    command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = id });
                    var r = command.ExecuteReader();
                    while (r.Read())
                    {
                        t = FromReader(r);
                        break;
                    }
                }
                connection.Close();
            }
            return t;
        }

        public int SaveItem(Task item)
        {
            int r;
            lock (locker)
            {
                if (item.ID != 0)
                {
                    connection = new SqliteConnection("Data Source=" + path);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE [Items] SET [Name] = ?, [Notes] = ?, [Done] = ? WHERE [_id] = ?;";
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Name });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Description });
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = item.Complete });
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = item.ID });
                        r = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return r;
                }
                else
                {
                    connection = new SqliteConnection("Data Source=" + path);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [Items] ([Name], [Description], [DueDate]) VALUES (? ,?, ?)";
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Name });
                        command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Description });
                        command.Parameters.Add(new SqliteParameter(DbType.DateTime) { Value = item.DueDate });
                        r = command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return r;
                }

            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                int r;
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [Items] WHERE [_id] = ?;";
                    command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = id });
                    r = command.ExecuteNonQuery();
                }
                connection.Close();
                return r;
            }
        }
    }

}