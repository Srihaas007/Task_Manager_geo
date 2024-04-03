using System;
using System.Threading.Tasks; // Make sure to include this namespace
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Task_Management.ViewModels;

public class DateTimePickerViewModel : BaseViewModel
{
    private DateTime? _finalSelectedDateTime;
    public DateTime? FinalSelectedDateTime
    {
        get => _finalSelectedDateTime;
        private set => SetProperty(ref _finalSelectedDateTime, value);
    }

    // This TaskCompletionSource will allow other parts of your app to await the user's date/time selection
    private TaskCompletionSource<DateTime?> _completionSource = new TaskCompletionSource<DateTime?>();

    // Expose the Task from the TaskCompletionSource
    public Task<DateTime?> CompletionTask => _completionSource.Task;

    public DateTime SelectedDate { get; set; } = DateTime.Today;
    public TimeSpan SelectedTime { get; set; } = TimeSpan.FromHours(12); // Midday

    public DateTime Today => DateTime.Today;
    public DateTime OneYearFromToday => DateTime.Today.AddYears(1);

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public DateTimePickerViewModel()
    {
        SaveCommand = new Command(Save);
        CancelCommand = new Command(Cancel);
    }

    private void Save()
    {
        FinalSelectedDateTime = SelectedDate.Add(SelectedTime);
        _completionSource.SetResult(FinalSelectedDateTime); // Notify of completion with the selected date/time
        ClosePage();
    }

    private void Cancel()
    {
        FinalSelectedDateTime = null;
        _completionSource.SetResult(null); // Notify of completion without a date/time
        ClosePage();
    }

    private async void ClosePage()
    {
        if (Application.Current.MainPage?.Navigation?.ModalStack?.LastOrDefault() is Page modalPage)
        {
            await modalPage.Navigation.PopModalAsync();
        }
    }
}
