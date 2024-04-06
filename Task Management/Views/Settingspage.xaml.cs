namespace Task_Management.Views
{

    public partial class SettingsPage : ContentPage
	{
        public SettingsPage(AuthenticationService authenticationService)
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(authenticationService);
        }

    }
}