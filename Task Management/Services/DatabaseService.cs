using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Task_Management.Models;

namespace Task_Management.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TaskManagementDb.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<TaskItem>().Wait(); // Ensure TaskItem table is created
        }

        public Task<int> AddUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserAsync(string username)
        {
            return _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        // Add task to the database
        public Task<int> AddTaskAsync(TaskItem task)
        {
            if (task.UserId == 0)
            {
                throw new ArgumentException("Task must be associated with a user.");
            }
            return _database.InsertAsync(task);
        }

        // Retrieve tasks for a specific user
        public async Task<List<TaskItem>> GetTasksAsync(int userId)
        {
            return (await _database.Table<TaskItem>().ToListAsync()).Where(t => t.UserId == userId).ToList();
        }



        // Update an existing task
        public Task<int> UpdateTaskAsync(TaskItem task)
        {
            return _database.UpdateAsync(task);
        }
        // In DatabaseService.cs

        public Task<int> DeleteTaskAsync(TaskItem task)
        {
            return _database.DeleteAsync(task);
        }

    }
}
