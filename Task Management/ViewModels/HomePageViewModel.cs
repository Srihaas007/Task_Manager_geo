using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Task_Management.Models;
using Task_Management.Services;


namespace Task_Management.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private readonly IAppNotificationService _notificationService;
        private readonly DatabaseService _databaseService;
        private readonly AuthenticationService _authenticationService;
        private readonly GeolocationService _geolocationService;
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand MarkTaskAsDoneCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }

        public HomePageViewModel(
            IAppNotificationService notificationService,
            DatabaseService databaseService,
            AuthenticationService authenticationService,
            GeolocationService geolocationService)
        {
            _notificationService = notificationService;
            _databaseService = databaseService;
            _authenticationService = authenticationService;
            _geolocationService = geolocationService; 

            AddTaskCommand = new Command(async () => await ExecuteAddTaskCommand());
            NavigateToSettingsCommand = new Command(async () => await NavigateToSettings());
            EditTaskCommand = new Command<TaskItem>(async (task) => await EditTask(task));
            DeleteTaskCommand = new Command<TaskItem>(async (task) => await DeleteTask(task));
            MarkTaskAsDoneCommand = new Command<TaskItem>(async (task) => await MarkTaskAsDone(task));

            if (_authenticationService.IsLoggedIn())
            {
                LoadTasks();
            }
        }

        
        public async Task ShowTaskOptions(TaskItem task)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(
                "Task Options",
                "Cancel",
                null,
                "Edit", "Delete", "Done"
            );

            switch (action)
            {
                case "Edit":
                    await EditTask(task);
                    break;
                case "Delete":
                    await DeleteTask(task);
                    break;
                case "Done":
                    await MarkTaskAsDone(task);
                    break;
            }
        }

        private async Task SaveHomeLocation(Location location)
        {
            // Converting location to a string format
            string locationString = $"{location.Latitude},{location.Longitude}";
            await SecureStorage.SetAsync("HomeLocation", locationString);
        }


        private async Task<Location> GetHomeLocationAsync()
        {
            var locationString = await SecureStorage.GetAsync("HomeLocation");
            if (string.IsNullOrEmpty(locationString))
            {
                return null;
            }

            var parts = locationString.Split(',');
            if (parts.Length != 2)
            {
                return null;
            }

            if (double.TryParse(parts[0], out double latitude) && double.TryParse(parts[1], out double longitude))
            {
                return new Location(latitude, longitude);
            }

            return null;
        }

        private async Task SetHomeLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                await SaveHomeLocation(location);
            }
        }


        public async Task EditTask(TaskItem task)
        {
            // Prompt for new name
            string newName = await Application.Current.MainPage.DisplayPromptAsync("Edit Task", "Enter new task name:", initialValue: task.Name);
            if (newName != null)
            {
                // Prompt for new detail
                string newDetail = await Application.Current.MainPage.DisplayPromptAsync("Edit Task Detail", "Enter new task detail (optional):", initialValue: task.Detail);

                // Update task
                task.Name = newName;
                task.Detail = newDetail;

                // Update in database
                await _databaseService.UpdateTaskAsync(task);

                // Update UI
                int index = Tasks.IndexOf(task);
                Tasks[index] = task;
            }
        }

        public async Task DeleteTask(TaskItem task)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this task?",
                "Yes", "No"
            );

            if (confirm)
            {
                await _databaseService.DeleteTaskAsync(task);
                Tasks.Remove(task);
            }
        }


       
        public async Task MarkTaskAsDone(TaskItem task)
        {
            task.IsCompleted = true;
            await _databaseService.UpdateTaskAsync(task);
            Tasks.Remove(task);

            
            MessagingCenter.Send(this, "TaskUpdated", task);
        }

        private async Task NavigateToSettings()
        {
            await Shell.Current.GoToAsync("///SettingsPage");
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

            // Ask for task location
            string[] options = { "Home", "Outside" };
            string selectedLocation = await Application.Current.MainPage.DisplayActionSheet("Select Task Location", "Cancel", null, options);

            TaskLocation location;
            switch (selectedLocation)
            {
                case "Home":
                    location = TaskLocation.Home;
                    break;
                case "Outside":
                    location = TaskLocation.Outside;
                    break;
                default:
                    return; // User canceled task creation
            }

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
                DueTime = selectedDateTime.Value,
                Location = location
            };

            await _databaseService.AddTaskAsync(newTask);
            Tasks.Add(newTask);

            if (location == TaskLocation.Outside)
            {
                var homeLocation = await GetHomeLocationAsync();
                if (homeLocation == null)
                {
                    await SetHomeLocation(); // This sets the home location if not already set
                }

                // Use the _geolocationService directly
                if (_geolocationService != null)
                {
                    _geolocationService.StartMonitoringForTaskOutsideHome(newTask);
                }
            }


            await ScheduleReminders(newTask);
        }

        private async Task ScheduleReminders(TaskItem task)
        {
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due in 1 hour.", task.Reminder1);
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due in 30 minutes.", task.Reminder2);
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due in 15 minutes.", task.Reminder3);
            await _notificationService.ScheduleNotification(task.Id, $"Reminder for {task.Name}", "Task is due now.", task.Deadline);
        }

        
    }
}
