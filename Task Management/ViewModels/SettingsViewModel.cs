using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Task_Management.Services;


namespace Task_Management.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authenticationService;

        public ICommand ChangePasswordCommand { get; }
        public ICommand NavigateToPreviousTasksCommand { get; }
        public ICommand LogoutCommand { get; }

        public ICommand NavigateToHomeCommand { get; }

        public SettingsViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            ChangePasswordCommand = new Command(async () => await Shell.Current.GoToAsync("///ChangePasswordPage"));
            LogoutCommand = new Command(async () => await Logout());
            NavigateToPreviousTasksCommand = new Command(async () => await NavigateToPreviousTasks());
            NavigateToHomeCommand = new Command(async () => await NavigateToHome());
        }

        private async Task Logout()
        {
            await SecureStorage.SetAsync("userId", string.Empty); // Clears user session
            _authenticationService.LogOut(); // Logout using AuthenticationService
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public async Task NavigateToPreviousTasks()
        {
            await Shell.Current.GoToAsync("//CompletedTasksPage");

        }

        private async Task NavigateToHome()
        {
            await Shell.Current.GoToAsync("//HomePage");
        }
    }
}