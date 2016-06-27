using System;



namespace ToDoList.Core
{
    public class Task
    {
        public Task()
        {

        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Complete { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CompletionDate { get; set; }
        
    }
}