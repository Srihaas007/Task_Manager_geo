using Task_Management.ViewModels;

namespace Task_Management.Views
{ 

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
}