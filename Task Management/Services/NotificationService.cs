#if ANDROID
using Android.App;
using Android.Content;
using AndroidX.Core.App;
using Resource = Microsoft.Maui.Controls.Resource;
#endif

namespace Task_Management.Platforms
{
    public class NotificationService : IAppNotificationService
    {
#if ANDROID
        private const string ChannelId = "task_reminders";
#endif

        public async Task ScheduleNotification(int id, string title, string message, DateTime notifyTime)
        {
            // If we're not on Android, we can't schedule a notification
#if !ANDROID
            return;
#endif

            TimeSpan delay = notifyTime - DateTime.Now;
            if (delay <= TimeSpan.Zero)
            {
                Console.WriteLine("Scheduled time is in the past. Notification not scheduled.");
                return;
            }

#if ANDROID
            var intent = new Intent(Platform.CurrentActivity, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(Platform.CurrentActivity, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(Platform.CurrentActivity, ChannelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.logo) // replace with your app's notification icon
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);

            var notificationManager = NotificationManagerCompat.From(Platform.CurrentActivity);
            await Task.Delay(delay); // Wait until the notification should be shown
            notificationManager.Notify(id, notificationBuilder.Build());
#endif
        }

        public Task CancelNotification(int id)
        {
#if !ANDROID
            return Task.CompletedTask; // No-op if we're not on Android
#else
            var notificationManager = NotificationManagerCompat.From(Platform.CurrentActivity);
            notificationManager.Cancel(id);
            Console.WriteLine($"Notification {id} cancelled.");
            return Task.CompletedTask;
#endif
        }
    }
}
