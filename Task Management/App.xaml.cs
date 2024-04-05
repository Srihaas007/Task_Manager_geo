using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Task_Management.Services;
using Task_Management.Models;
using Task_Management.ViewModels;
using Task_Management.Views;
using Microsoft.Maui;

namespace Task_Management;

public partial class App : Application
{
    // Property to access the service provider
    public new static App Current => (App)Application.Current;
    public static IServiceProvider ServiceProvider { get; private set; }

    private readonly AuthenticationService _authenticationService;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        ServiceProvider = serviceProvider;

        var authenticationService = ServiceProvider.GetRequiredService<AuthenticationService>();
        MainPage = new AppShell(authenticationService);
    }
}