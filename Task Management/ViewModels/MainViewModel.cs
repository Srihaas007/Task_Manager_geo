using System.Windows.Input;

namespace Task_Management.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authenticationService;

        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public MainViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            NavigateToLoginCommand = new Command(async () => await NavigateToLogin());
            NavigateToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//RegistrationPage"));
        }

        private async Task NavigateToLogin()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}