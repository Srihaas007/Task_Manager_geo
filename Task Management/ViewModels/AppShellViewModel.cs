namespace Task_Management.ViewModels
{
    public class AppShellViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationService _authenticationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public AppShellViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            // Subscribing to the event that notifies about login status changes
            MessagingCenter.Subscribe<AuthenticationService>(this, "LoginStatusChanged", (sender) =>
            {
                OnPropertyChanged(nameof(IsUserLoggedIn));
            });
        }

        public bool IsUserLoggedIn => _authenticationService.IsLoggedIn();

        public Command LogoutCommand => new Command(() =>
        {
            _authenticationService.LogOut();
            Shell.Current.GoToAsync("//LoginPage").ConfigureAwait(false);
        });

        protected void OnPropertyChanged(string propertyName)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }
    }
}
