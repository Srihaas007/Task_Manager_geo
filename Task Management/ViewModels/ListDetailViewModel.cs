using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Task_Management.ViewModels;

public partial class ListDetailViewModel : ObservableObject
{
    readonly SampleDataService dataService;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    ObservableCollection<SampleItem> items;

    public ListDetailViewModel(SampleDataService service)
    {
        dataService = service;
    }

    [RelayCommand]
    async Task OnRefreshing()
    {
        IsRefreshing = true;

        try
        {
            await LoadDataAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task LoadMore()
    {
        var moreItems = await dataService.GetItems();

        foreach (var item in moreItems)
        {
            Items.Add(item);
        }
    }

    public async Task LoadDataAsync()
    {
        Items = new ObservableCollection<SampleItem>(await dataService.GetItems());
    }

    [RelayCommand]
    async Task GoToDetails(SampleItem item)
    {
        if (item == null) return;
        // Make sure SampleItem has an Id property or adjust to use the correct identifier
        await Shell.Current.GoToAsync($"{nameof(ListDetailDetailPage)}?ItemId={item.Id}");
    }
}
