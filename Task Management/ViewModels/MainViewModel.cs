using System.Windows.Input;
using Microsoft.Maui.Controls;
using Task_Management.ViewModels;
using Task_Management.Views;

namespace Task_Management.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }
    
    private bool _isLoggedIn;
    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set => SetProperty(ref _isLoggedIn, value);
    }
    
    public MainViewModel()
    {
        IsLoggedIn = new AuthenticationService().IsLoggedIn();
        NavigateToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
        NavigateToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));
        NavigateToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//RegistrationPage"));
    }
}