using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Task_Management.Services;

namespace Task_Management.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string username;
        private string password;
        private readonly DatabaseService _databaseService;
        private readonly AuthenticationService _authenticationService; // Add this

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(DatabaseService databaseService, AuthenticationService authenticationService) // Modify this
        {
            _databaseService = databaseService;
            _authenticationService = authenticationService; // Initialize
            LoginCommand = new Command(async () => await OnLoginClicked());
            NavigateToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//RegistrationPage"));
        }

        private async Task OnLoginClicked()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessagingCenter.Send(this, "LoginAlert", "Username and password are required.");
                return;
            }

            var user = await _databaseService.GetUserAsync(Username);
            if (user == null || user.Password != Password)
            {
                MessagingCenter.Send(this, "LoginAlert", "Invalid username or password.");
                return;
            }

            await SecureStorage.SetAsync("userId", user.Id.ToString());
            _authenticationService.LogIn(); // Update login status

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("//HomePage");
            });
        }


    }
}
