using Microsoft.Maui.Controls;
using Task_Management.ViewModels;

namespace Task_Management.Views
{

	public partial class ChangePasswordPage : ContentPage
	{
        public ChangePasswordPage(DatabaseService databaseService, AuthenticationService authenticationService)
        {
            InitializeComponent();
            BindingContext = new ChangePasswordViewModel(databaseService, authenticationService);
        }



    }
}