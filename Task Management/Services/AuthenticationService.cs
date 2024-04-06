namespace Task_Management.Services
{
    public class AuthenticationService
    {
        public void LogIn(int userId)
        {
            Preferences.Set("IsLoggedIn", true);
            SecureStorage.SetAsync("userId", userId.ToString()).Wait(); 
            MessagingCenter.Send(this, "LoginStatusChanged");
        }

        public void LogOut()
        {
            Preferences.Set("IsLoggedIn", false);
            SecureStorage.SetAsync("userId", string.Empty).Wait(); 
            MessagingCenter.Send(this, "LoginStatusChanged");
        }

        public bool IsLoggedIn()
        {
            var isLoggedIn = Preferences.Get("IsLoggedIn", false);
            var userId = SecureStorage.GetAsync("userId").Result;
            return isLoggedIn && !string.IsNullOrEmpty(userId);
        }
    }
}