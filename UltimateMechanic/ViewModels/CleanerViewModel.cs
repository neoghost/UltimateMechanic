using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UltimateMechanic.Models;
using UltimateMechanic.Services;

namespace UltimateMechanic.ViewModels
{
    public partial class CleanerViewModel : ObservableObject
    {
        private readonly ICleanerService _cleanerService;

        [ObservableProperty]
        private ObservableCollection<CleanupItem> _cleanupItems = new();

        [ObservableProperty]
        private bool _isScanning;

        [ObservableProperty]
        private bool _isCleaning;

        [ObservableProperty]
        private string _statusMessage = "Ready to scan";

        [ObservableProperty]
        private long _totalSizeBytes;

        [ObservableProperty]
        private int _selectedCount;

        public string TotalSizeFormatted => FormatBytes(TotalSizeBytes);

        public CleanerViewModel(ICleanerService cleanerService)
        {
            _cleanerService = cleanerService;
        }

        [RelayCommand]
        private async Task ScanAsync()
        {
            IsScanning = true;
            CleanupItems.Clear();
            TotalSizeBytes = 0;

            try
            {
                var progress = new Progress<string>(msg => StatusMessage = msg);
                var items = await _cleanerService.ScanForCleanupItemsAsync(progress);

                foreach (var item in items)
                {
                    CleanupItems.Add(item);
                }

                TotalSizeBytes = items.Sum(i => i.SizeBytes);
                SelectedCount = items.Count(i => i.IsSelected);
                StatusMessage = $"Found {items.Count} items ({TotalSizeFormatted})";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsScanning = false;
            }
        }

        [RelayCommand]
        private async Task CleanAsync()
        {
            if (!CleanupItems.Any(i => i.IsSelected))
            {
                StatusMessage = "No items selected for cleanup";
                return;
            }

            IsCleaning = true;

            try
            {
                var progress = new Progress<string>(msg => StatusMessage = msg);
                var selectedItems = CleanupItems.Where(i => i.IsSelected).ToList();
                var cleaned = await _cleanerService.CleanupItemsAsync(selectedItems, progress);

                StatusMessage = $"Cleaned {FormatBytes(cleaned)}";

                // Refresh the list
                await ScanAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsCleaning = false;
            }
        }

        [RelayCommand]
        private void SelectAll()
        {
            foreach (var item in CleanupItems)
            {
                item.IsSelected = true;
            }
            SelectedCount = CleanupItems.Count;
        }

        [RelayCommand]
        private void DeselectAll()
        {
            foreach (var item in CleanupItems)
            {
                item.IsSelected = false;
            }
            SelectedCount = 0;
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        partial void OnTotalSizeBytesChanged(long value)
        {
            OnPropertyChanged(nameof(TotalSizeFormatted));
        }
    }
}
