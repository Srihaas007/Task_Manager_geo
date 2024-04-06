using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Task_Management.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private string email;
        private string username;
        private string password;
        private bool acceptTerms;
        private readonly DatabaseService _databaseService;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

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

        public bool AcceptTerms
        {
            get => acceptTerms;
            set => SetProperty(ref acceptTerms, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; } 

        public RegistrationViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            RegisterCommand = new Command(async () => await OnRegisterClicked());
            NavigateToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));
        }

        private async Task OnRegisterClicked()
        {
            if (!ValidateInput())
            {
                return;
            }

            var user = await _databaseService.GetUserAsync(Username);
            if (user != null)
            {
                MessagingCenter.Send(this, "Alert", "User already exists.");
                return;
            }

            await _databaseService.AddUserAsync(new User { Username = Username, Password = Password, Email = Email });
            MessagingCenter.Send(this, "Alert", "Registration successful!");
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email) || !AcceptTerms)
            {
                MessagingCenter.Send(this, "Alert", "All fields are required, and you must accept the terms and conditions.");
                return false;
            }

            if (Password.Length < 8 ||
                !Regex.IsMatch(Password, @"\d") ||
                !Regex.IsMatch(Password, @"[a-z]") ||
                !Regex.IsMatch(Password, @"[A-Z]") ||
                !Regex.IsMatch(Password, @"[^a-zA-Z\d]"))
            {
                MessagingCenter.Send(this, "Alert", "Password must be at least 8 characters long and include a number, an uppercase letter, a lowercase letter, and a special character.");
                return false;
            }

            return true;
        }
    }
}