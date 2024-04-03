using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Task_Management.Models;
using Task_Management.Services;

namespace Task_Management.ViewModels
{
    public class PreviousTasksViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<TaskItem> CompletedTasks { get; } = new ObservableCollection<TaskItem>();

        public PreviousTasksViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            LoadCompletedTasks();
        }

        public async Task LoadCompletedTasks()
        {
            var userIdString = await SecureStorage.GetAsync("userId");
            if (!int.TryParse(userIdString, out int userId))
            {
                // Handle the error or notify the user that the user ID could not be retrieved.
                return;
            }

            var tasks = await _databaseService.GetTasksAsync(userId);
            var completedTasks = tasks.Where(t => t.IsCompleted).ToList();

            CompletedTasks.Clear();
            foreach (var task in completedTasks)
            {
                CompletedTasks.Add(task);
            }
        }
    }
}
