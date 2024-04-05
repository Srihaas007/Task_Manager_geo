using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Task_Management.Models;
using Task_Management.Services;
using Task_Management.Views;

namespace Task_Management.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private readonly IAppNotificationService _notificationService;
        private readonly DatabaseService _databaseService;
        private readonly AuthenticationService _authenticationService;

        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

        public ICommand AddTaskCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ToggleTaskCompletionCommand { get; }
        public ICommand NavigateToPreviousTasksCommand { get; }

        public HomePageViewModel(
            IAppNotificationService notificationService,
            DatabaseService databaseService,
            AuthenticationService authenticationService)
        {
            _notificationService = notificationService;
            _databaseService = databaseService;
            _authenticationService = authenticationService;

            AddTaskCommand = new Command(async () => await ExecuteAddTaskCommand());
            LogoutCommand = new Command(async () => await Logout());
            ToggleTaskCompletionCommand = new Command<TaskItem>(async (task) => await ToggleTaskCompletion(task));
            NavigateToPreviousTasksCommand = new Command(async () => await NavigateToPreviousTasks());

            if (_authenticationService.IsLoggedIn())
            {
                LoadTasks();
            }
        }

        public async Task ToggleTaskCompletion(TaskItem task)
        {
            if (task == null || task.IsProcessing || task.IsCompleted) return;

            task.IsProcessing = true;

            bool confirmCompletion = await Application.Current.MainPage.DisplayAlert(
                "Confirm Task Completion",
                "Have you completed this task?",
                "Yes",
                "No"
            );

            if (confirmCompletion)
            {
                task.IsCompleted = true;
                await _databaseService.UpdateTaskAsync(task);
                Tasks.Remove(task);
                // If there is a need to add the task to previous tasks, it can be done here.
            }
            else
            {
                task.IsCompleted = false;
            }

            task.IsProcessing = false;
        }

        private async Task NavigateToPreviousTasks()
        {
            await Shell.Current.GoToAsync(nameof(CompletedTasksPage));
        }

        private void LoadTasks()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var userIdString = await SecureStorage.GetAsync("userId");
                if (int.TryParse(userIdString, out int userId))
                {
                    var tasks = await _databaseService.GetTasksAsync(userId);
                    Tasks.Clear();
                    foreach (var task in tasks)
                    {
                        Tasks.Add(task);
                    }
                }
            });
        }

        private async Task ExecuteAddTaskCommand()
        {
            string taskName = await Application.Current.MainPage.DisplayPromptAsync("New Task", "Enter task name:");
            if (string.IsNullOrWhiteSpace(taskName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Task name cannot be empty.", "OK");
                return;
            }

            string taskDetail = await Application.Current.MainPage.DisplayPromptAsync("Task Detail", "Enter task detail (optional):");

            var dateTimePickerViewModel = new DateTimePickerViewModel();
            var dateTimePickerPage = new CustomDateTimePickerPage(dateTimePickerViewModel);
            await Application.Current.MainPage.Navigation.PushModalAsync(dateTimePickerPage);

            DateTime? selectedDateTime = await dateTimePickerViewModel.CompletionTask;
            if (!selectedDateTime.HasValue)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select a due date and time.", "OK");
                return;
            }

            var userIdString = await SecureStorage.GetAsync("userId");
            if (!int.TryParse(userIdString, out int userId))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to identify user.", "OK");
                return;
            }

            var newTask = new TaskItem
            {
                UserId = userId,
                Name = taskName,
                Detail = taskDetail,
                DueTime = selectedDateTime.Value
            };

            await _databaseService.AddTaskAsync(newTask);
            Tasks.Add(newTask);

            await ScheduleReminders(newTask);
        }

        private async Task ScheduleReminders(TaskItem task)
        {
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due in 1 hour.", task.Reminder1);
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due in 30 minutes.", task.Reminder2);
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due in 15 minutes.", task.Reminder3);
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due now.", task.Deadline);
        }

        private async Task Logout()
        {
            await SecureStorage.SetAsync("userId", string.Empty); // Clears user session
            _authenticationService.LogOut(); // Logout using AuthenticationService
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
