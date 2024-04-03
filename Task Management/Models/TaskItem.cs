using System;
using SQLite;

namespace Task_Management.Models
{
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public DateTime DueTime { get; set; }
        public int UserId { get; set; }  // Foreign key for the User

        public bool IsCompleted { get; set; } // To mark the task as completed
        // Additional properties to store the notification times
        public DateTime Reminder1 => DueTime.AddHours(-1);
        public DateTime Reminder2 => DueTime.AddMinutes(-30);
        public DateTime Reminder3 => DueTime.AddMinutes(-15);
        public DateTime Deadline => DueTime;  // Assuming DueTime is the deadline
    }
}
