
using Microsoft.Maui.Controls;
using Task_Management.Services;

namespace Task_Management;

public partial class App : Application
{
    private readonly AuthenticationService _authenticationService;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _authenticationService = serviceProvider.GetRequiredService<AuthenticationService>();
        MainPage = new AppShell(_authenticationService);

        // Navigate to the appropriate page based on login status
        if (_authenticationService.IsLoggedIn())
        {
            Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
