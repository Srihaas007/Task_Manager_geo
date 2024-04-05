using Microsoft.Maui.Controls;
using Task_Management.Services;

namespace Task_Management;

public partial class AppShell : Shell
{
    private readonly AuthenticationService _authenticationService;

    public AppShell(AuthenticationService authenticationService)
    {
        InitializeComponent();
        _authenticationService = authenticationService;

        // Navigate to the appropriate page based on login status
        if (_authenticationService.IsLoggedIn())
        {
            GoToAsync("//MainPage");
        }
        else
        {
            GoToAsync("//LoginPage");
        }
    }
}