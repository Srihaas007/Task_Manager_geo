using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Task_Management.Services;

namespace Task_Management;

public partial class App : Application
{
    private readonly AuthenticationService _authenticationService;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _authenticationService = serviceProvider.GetRequiredService<AuthenticationService>();
        MainPage = serviceProvider.GetRequiredService<AppShell>();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        if (_authenticationService.IsLoggedIn())
        {
            await Shell.Current.GoToAsync("//HomePage");
        }
        else
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
