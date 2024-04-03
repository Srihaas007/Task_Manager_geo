using Task_Management.ViewModels;
using Microsoft.Maui.Controls; 
using Task_Management.Models; 

namespace Task_Management.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomePageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var taskItem = (TaskItem)checkBox.BindingContext;
            var vm = (HomePageViewModel)BindingContext;
            if (vm.ToggleTaskCompletionCommand.CanExecute(taskItem))
            {
                vm.ToggleTaskCompletionCommand.Execute(taskItem);
            }
        }
    }
}
