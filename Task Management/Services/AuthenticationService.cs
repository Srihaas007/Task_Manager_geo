
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Task_Management.Services
{
    public class AuthenticationService
    {
        public void LogIn()
        {
            Preferences.Set("IsLoggedIn", true);
            MessagingCenter.Send(this, "LoginStatusChanged");
        }

        public void LogOut()
        {
            Preferences.Set("IsLoggedIn", false);
            MessagingCenter.Send(this, "LoginStatusChanged");
        }

        public bool IsLoggedIn()
        {
            return Preferences.Get("IsLoggedIn", false);
        }
    }
}
