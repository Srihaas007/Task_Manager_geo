using System.Windows.Input;
using Microsoft.Maui.Controls;
using Task_Management.Services;

namespace Task_Management.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authenticationService;

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        public MainViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            IsLoggedIn = _authenticationService.IsLoggedIn();
            NavigateToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));
            NavigateToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//RegistrationPage"));
        }
    }
}
