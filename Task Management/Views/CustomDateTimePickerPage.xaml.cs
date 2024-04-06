namespace Task_Management.Views
{

    public partial class CustomDateTimePickerPage : ContentPage
    {
        public CustomDateTimePickerPage(DateTimePickerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}