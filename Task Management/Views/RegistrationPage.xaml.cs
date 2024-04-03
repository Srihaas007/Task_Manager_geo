using Task_Management.ViewModels;
using System.Text.RegularExpressions;

namespace Task_Management.Views
{

    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            MessagingCenter.Subscribe<RegistrationViewModel, string>(this, "Alert", (sender, message) =>
            {
                DisplayAlert("Alert", message, "OK");
            });
        }
    }
}