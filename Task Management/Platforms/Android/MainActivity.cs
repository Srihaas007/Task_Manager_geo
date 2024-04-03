using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Task_Management;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Create notification channel for Android versions >= Oreo
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            NotificationChannel channel = new NotificationChannel(
                "task_reminders", // Internal ID of the channel
                "Task Reminders", // User visible name of the channel
                NotificationImportance.High) // The importance level for the notifications
            {
                Description = "Reminders for tasks" // User visible description of the channel
            };

            // Get the NotificationManager system service
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);

            // Register the notification channel with the system
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
