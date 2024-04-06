using Microsoft.Maui.Hosting;
using Task_Management.Platforms;
using Task_Management.Services;
using Task_Management.Models;
using Task_Management.ViewModels;
using Task_Management.Views;
using Microsoft.Maui;

namespace Task_Management;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
           
            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        // Register ViewModels
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegistrationViewModel>();
        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddTransient<PreviousTasksViewModel>();
        builder.Services.AddSingleton<DateTimePickerViewModel>();
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<ChangePasswordViewModel>();




        // Register Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<CompletedTasksPage>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<ChangePasswordPage>();


        // Register other services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<GeolocationService>();
        builder.Services.AddSingleton<CustomDateTimePickerPage>();
        builder.Services.AddSingleton<IAppNotificationService, NotificationService>();
        builder.Services.AddSingleton<AuthenticationService>();


        builder.Services.AddTransient<AppShell>();


        return builder.Build();
    }
}
