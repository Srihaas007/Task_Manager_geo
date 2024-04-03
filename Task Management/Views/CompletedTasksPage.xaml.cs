using Microsoft.Maui.Controls;
using Task_Management.ViewModels;

namespace Task_Management.Views
{
    public partial class CompletedTasksPage : ContentPage
    {
        public CompletedTasksPage()
        {
            InitializeComponent();
            BindingContext = App.ServiceProvider.GetService<PreviousTasksViewModel>();
        }

    }

}
