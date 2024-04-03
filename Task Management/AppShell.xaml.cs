namespace Task_Management;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ListDetailDetailPage), typeof(ListDetailDetailPage));

        // Call this method to update the UI immediately on app startup
        UpdateFlyoutItemsVisibility();

        // Subscribe to a message that notifies when the login state changes
        MessagingCenter.Subscribe<LoginViewModel>(this, "LoginStateChanged", (sender) =>
        {
            UpdateFlyoutItemsVisibility();
        });
    }

    private void UpdateFlyoutItemsVisibility()
    {
        // Retrieve the user ID from secure storage to determine if the user is logged in
        var userId = SecureStorage.GetAsync("userId").Result;
        var isLoggedIn = !string.IsNullOrEmpty(userId);

        // Loop through FlyoutItems and set IsVisible based on login status
        foreach (var item in this.Items)
        {
            if (item is FlyoutItem flyoutItem)
            {
                // Set visibility based on whether we have a valid user ID
                flyoutItem.IsVisible = isLoggedIn;
            }
        }
    }

}


