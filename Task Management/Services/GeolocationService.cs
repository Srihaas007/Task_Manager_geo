using System;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;
using Task_Management.Models;
using Task_Management.Services;

namespace Task_Management.Services
{
    public class GeolocationService
    {
        private readonly IAppNotificationService _notificationService;
        private const double HomeRadiusKm = 0.804672;
        private Location _homeLocation;
        private Location _lastKnownLocation;
        private bool _isGeneralMonitoring = false;
        private CancellationTokenSource _ctsGeneral;

        public GeolocationService(IAppNotificationService notificationService)
        {
            _notificationService = notificationService;

        }

        public async Task StartMonitoring()
        {
            if (_isGeneralMonitoring)
            {
                Console.WriteLine("General monitoring is already in progress.");
                return;
            }

            _isGeneralMonitoring = true;
            _ctsGeneral = new CancellationTokenSource();

            try
            {
                while (!_ctsGeneral.IsCancellationRequested)
                {
                    var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });

                    if (location != null)
                    {
                        Console.WriteLine($"Current location: {location.Latitude}, {location.Longitude}");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), _ctsGeneral.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("General location monitoring was canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during general location monitoring: {ex.Message}");
            }
            finally
            {
                _isGeneralMonitoring = false;
            }
        }

        public void StopGeneralMonitoring()
        {
            _ctsGeneral?.Cancel();
        }

        public async Task StartMonitoringForTaskOutsideHome(TaskItem task)
        {
            _homeLocation = await GetHomeLocationAsync();
            if (_homeLocation == null)
            {
                Console.WriteLine("Home location is not set.");
                return; 
            }

            while (task.Location == TaskLocation.Outside && !task.IsCompleted)
            {
                var currentLocation = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

                if (IsOutsideHome(currentLocation))
                {
                    await _notificationService.ScheduleNotification(
                        task.Id,
                        "Task Reminder",
                        $"You are now more than half a mile away from home. Don't forget your task: {task.Name}",
                        DateTime.Now);
                }

                _lastKnownLocation = currentLocation;

                // after the time it checks the location again automatically
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        private bool IsOutsideHome(Location currentLocation)
        {
            if (currentLocation == null || _homeLocation == null) return false;

            var distance = Location.CalculateDistance(currentLocation, _homeLocation, DistanceUnits.Kilometers);
            return distance > HomeRadiusKm;
        }

        private async Task<Location> GetHomeLocationAsync()
        {
            var locationString = await SecureStorage.GetAsync("HomeLocation");
            if (!string.IsNullOrEmpty(locationString))
            {
                var parts = locationString.Split(',');
                if (parts.Length == 2 &&
                    double.TryParse(parts[0], out var latitude) &&
                    double.TryParse(parts[1], out var longitude))
                {
                    return new Location(latitude, longitude);
                }
            }
            return null;
        }
    }
}
