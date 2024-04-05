using Microsoft.Maui.Controls;
using Task_Management.ViewModels;

namespace Task_Management.Views
{
   
    public partial class CompletedTasksPage : ContentPage
    {
        private PreviousTasksViewModel _viewModel;

        public CompletedTasksPage(PreviousTasksViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<HomePageViewModel, TaskItem>(this, "TaskUpdated", async (sender, task) =>
            {
                // Call to update the task list
                await _viewModel.InitLoadCompletedTasks();
            });

            // Initial load or refresh
            _viewModel.InitLoadCompletedTasks();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<HomePageViewModel, TaskItem>(this, "TaskUpdated");
        }

       


    }


}
