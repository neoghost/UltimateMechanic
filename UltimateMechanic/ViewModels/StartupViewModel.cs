using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UltimateMechanic.Models;
using UltimateMechanic.Services;
using System.Windows;

namespace UltimateMechanic.ViewModels;

public partial class StartupViewModel : ObservableObject
{
    private readonly IStartupService _startupService;

    [ObservableProperty]
    private ObservableCollection<StartupItem> _startupItems = new();

    [ObservableProperty]
    private bool _isLoading;

    public StartupViewModel(IStartupService startupService)
    {
        _startupService = startupService;
    }

    [RelayCommand]
    private async Task LoadStartupItemsAsync()
    {
        IsLoading = true;
        StartupItems.Clear();

        try
        {
            var items = await _startupService.GetStartupAppsAsync();
            foreach (var item in items)
            {
                StartupItems.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ToggleItemAsync(StartupItem item)
    {
        // Simply calls the service to toggle state
        await _startupService.ToggleStartupAppAsync(item, item.IsEnabled);
    }

    [RelayCommand]
    private async Task DeleteItemAsync(StartupItem item)
    {
        // Ask for confirmation before deleting permanently
        var result = MessageBox.Show($"Are you sure you want to delete '{item.Name}' from startup?", 
                                     "Confirm Delete", 
                                     MessageBoxButton.YesNo, 
                                     MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            await _startupService.DeleteStartupAppAsync(item);
            StartupItems.Remove(item);
        }
    }
}
