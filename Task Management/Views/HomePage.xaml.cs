using Microsoft.Maui.Controls;
using Task_Management.Models;
using Task_Management.ViewModels;
using System;

namespace Task_Management.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomePageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnTaskOptionsButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var taskItem = button?.BindingContext as TaskItem;

            if (taskItem != null)
            {
                string action = await DisplayActionSheet(
                    "Task Options",
                    "Cancel",
                    null,
                    "Edit", "Delete", "Done"
                );

                var viewModel = BindingContext as HomePageViewModel;

                if (viewModel != null)
                {
                    
                    switch (action)
                    {
                        case "Edit":
                            await viewModel.EditTask(taskItem);
                            break;
                        case "Delete":
                            await viewModel.DeleteTask(taskItem);
                            break;
                        case "Done":
                            await viewModel.MarkTaskAsDone(taskItem);
                            break;
                    }
                }
            }
        }
    }
}
