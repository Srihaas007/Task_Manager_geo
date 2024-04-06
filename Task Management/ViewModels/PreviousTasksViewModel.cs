using System.Windows.Input;

namespace Task_Management.ViewModels
{
    public class PreviousTasksViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<TaskItem> CompletedTasks { get; } = new ObservableCollection<TaskItem>();
        public ICommand NavigateToSettingsCommand { get; private set; }

        public PreviousTasksViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));

            NavigateToSettingsCommand = new Command(async () => await Shell.Current.GoToAsync("///SettingsPage"));
        }


       
        public async Task InitLoadCompletedTasks()
        {
            var userIdString = await SecureStorage.GetAsync("userId");
            if (!int.TryParse(userIdString, out int userId))
            {
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
