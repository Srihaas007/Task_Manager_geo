using Microsoft.Maui.Controls;
using Task_Management.Models;
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

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var taskItem = checkBox?.BindingContext as TaskItem;
            var viewModel = BindingContext as HomePageViewModel;

            if (e.Value && taskItem != null && !taskItem.IsCompleted)
            {
                // Execute the ToggleTaskCompletionCommand
                await viewModel?.ToggleTaskCompletion(taskItem);
                checkBox.IsChecked = taskItem.IsCompleted;
            }
            else if (!e.Value)
            {
                // Handle uncheck logic if needed
            }
        }
    }
}
