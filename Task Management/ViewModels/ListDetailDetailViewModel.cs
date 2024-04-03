using CommunityToolkit.Mvvm.ComponentModel;

namespace Task_Management.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class ListDetailDetailViewModel : ObservableObject
{
    [ObservableProperty]
    SampleItem item;
}
