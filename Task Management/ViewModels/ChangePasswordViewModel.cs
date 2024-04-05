using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Task_Management.Services;
using Task_Management.Models;

namespace Task_Management.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly AuthenticationService _authenticationService;

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ICommand ChangePasswordCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }

        public ChangePasswordViewModel(DatabaseService databaseService, AuthenticationService authenticationService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            NavigateToSettingsCommand = new Command(async () => await NavigateToSettings());
            ChangePasswordCommand = new Command(async () => await ChangePassword());
        }

        private async Task ChangePassword()
        {
            var username = GetCurrentUsername(); // This should be implemented to get the current user's username

            if (string.IsNullOrEmpty(username))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Username not found.", "OK");
                return;
            }

            var user = await _databaseService.GetUserAsync(username);
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User not found.", "OK");
                return;
            }

            // Validate the current password
            if (user.Password != CurrentPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Current password is incorrect.", "OK");
                return;
            }

            // Check if new password and confirm password match
            if (NewPassword != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "New passwords do not match.", "OK");
                return;
            }

            // Update the password in the database
            user.Password = NewPassword;
            await _databaseService.UpdateUserAsync(user);

            // Optionally, log the user out or inform them to log in again
            _authenticationService.LogOut();
            await Shell.Current.GoToAsync("//LoginPage");

            await Application.Current.MainPage.DisplayAlert("Success", "Password changed successfully. Please log in with your new password.", "OK");
        }
        private async Task NavigateToSettings()
        {
            await Shell.Current.GoToAsync("///SettingsPage");
        }


        private string GetCurrentUsername()
        {
            // Implement this method based on how you're tracking the currently logged-in user
            // For example, you might be storing the username in SecureStorage or Preferences
            return Preferences.Get("CurrentUsername", null);
        }

    }
}
