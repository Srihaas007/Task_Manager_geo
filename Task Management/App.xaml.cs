using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Task_Management.Services;
using Task_Management.Views;

namespace Task_Management;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public static IServiceProvider ServiceProvider { get; private set; }

    private AuthenticationService _authenticationService;
    private GeolocationService _geoService;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        ServiceProvider = serviceProvider;
        _authenticationService = ServiceProvider.GetRequiredService<AuthenticationService>();
        _geoService = ServiceProvider.GetRequiredService<GeolocationService>();

        MainPage = new AppShell(_authenticationService);
    }

    protected override void OnStart()
    {
        base.OnStart();
        // Asynchronous call without awaiting since OnStart cannot be async
        MainThread.BeginInvokeOnMainThread(async () => await RequestLocationPermissions());
    }

    private async Task RequestLocationPermissions()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        if (status == PermissionStatus.Granted)
        {
            // Permission granted
            await _geoService.StartMonitoring();
        }
        else
        {
            // Handle the case where the user denies location permissions
            // For example, inform the user that location features will not be available
        }
    }

    protected override void OnSleep()
    {
        // Stop the geolocation service when the app goes to sleep if necessary
        _geoService?.StopGeneralMonitoring();
        base.OnSleep();
    }

    protected override void OnResume()
    {
        // Restart the geolocation service when the app resumes
        MainThread.BeginInvokeOnMainThread(async () => await _geoService.StartMonitoring());
        base.OnResume();
    }
}
