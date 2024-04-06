namespace Task_Management.Views
{
    public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "LoginAlert", (sender, alert) =>
            {
                DisplayAlert("Login Error", alert, "OK");
            });


        }

        protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<LoginViewModel, string>(this, "LoginAlert");
    }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var userId = await SecureStorage.GetAsync("userId");
            if (!string.IsNullOrEmpty(userId))
            {
                // when User is logged in, navigate to the HomePage
                await Shell.Current.GoToAsync("//HomePage");
            }
        }

    }
}