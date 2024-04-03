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

        // Subscribe to a message that notifies when the login state changes
        MessagingCenter.Subscribe<AuthenticationService>(this, "LoginStatusChanged", sender => UpdateFlyoutItemsVisibility());

        // Initial visibility update
        UpdateFlyoutItemsVisibility();
    }

    private void UpdateFlyoutItemsVisibility()
    {
        bool isLoggedIn = _authenticationService.IsLoggedIn();

        // Manually set the visibility for the LoginPage and RegistrationPage
        var loginItem = (ShellContent)this.FindByName("LoginItem");
        if (loginItem != null)
        {
            loginItem.IsVisible = !isLoggedIn;
        }

        var registerItem = (ShellContent)this.FindByName("RegisterItem");
        if (registerItem != null)
        {
            registerItem.IsVisible = !isLoggedIn;
        }

        // Set the visibility for the MainPage, which should only be shown when logged in
        var mainPageItem = (FlyoutItem)this.FindByName("MainPageItem");
        if (mainPageItem != null)
        {
            mainPageItem.IsVisible = isLoggedIn;
        }
    }
}
