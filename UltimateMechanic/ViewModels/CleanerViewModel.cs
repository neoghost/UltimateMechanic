using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using UltimateMechanic.Models;
using UltimateMechanic.Services;

namespace UltimateMechanic.ViewModels
{
    public partial class CleanerViewModel : ObservableObject
    {
        private readonly ICleanerService _cleanerService;

        [ObservableProperty]
        private ObservableCollection<CleanupGroup> _cleanupGroups = new();

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

        // Call this to update counts when selections change
        public void UpdateSelectedCount()
        {
            SelectedCount = CleanupGroups.Sum(g => g.Count(i => i.IsSelected));
            TotalSizeBytes = CleanupGroups.Sum(g => g.Sum(i => i.SizeBytes));
        }

        [RelayCommand]
        private async Task ScanAsync()
        {
            IsScanning = true;
            CleanupGroups.Clear();
            TotalSizeBytes = 0;

            try
            {
                var progress = new Progress<string>(msg => StatusMessage = msg);
                var items = await _cleanerService.ScanForCleanupItemsAsync(progress);

                // Group items by category
                var groups = items.GroupBy(i => i.Category)
                    .Select(g => new Models.CleanupGroup(GetGroupName(g.Key))
                    {
                        // initialize items
                    })
                    .ToList();

                // Populate groups and their items
                foreach (var g in items.GroupBy(i => i.Category))
                {
                    var grp = new Models.CleanupGroup(GetGroupName(g.Key));
                    foreach (var item in g)
                    {
                        grp.Add(item);
                        item.PropertyChanged += CleanupItem_PropertyChanged;
                    }

                    // listen to group and future item changes
                    grp.PropertyChanged += Group_PropertyChanged;
                    grp.CollectionChanged += Group_CollectionChanged;

                    CleanupGroups.Add(grp);
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
            if (!CleanupGroups.Any(g => g.Any(i => i.IsSelected)))
            {
                StatusMessage = "No items selected for cleanup";
                return;
            }

            IsCleaning = true;

            try
            {
                var progress = new Progress<string>(msg => StatusMessage = msg);
                var selectedItems = CleanupGroups.SelectMany(g => g.Where(i => i.IsSelected)).ToList();
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
            foreach (var grp in CleanupGroups)
            {
                grp.IsSelected = true;
            }
            UpdateSelectedCount();
        }

        [RelayCommand]
        private void DeselectAll()
        {
            foreach (var grp in CleanupGroups)
            {
                grp.IsSelected = false;
            }
            UpdateSelectedCount();
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

        private string GetGroupName(CleanupCategory category)
        {
            return category switch
            {
                CleanupCategory.TemporaryFiles => "Temporary Files",
                CleanupCategory.BrowserCache => "Browser Cache",
                CleanupCategory.RecycleBin => "Recycle Bin",
                CleanupCategory.WindowsLogs => "Windows Logs",
                CleanupCategory.ThumbnailCache => "Thumbnail Cache",
                CleanupCategory.MemoryDumps => "Memory Dumps",
                CleanupCategory.ErrorReports => "Error Reports",
                _ => category.ToString(),
            };
        }


        private void Group_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CleanupGroup.IsSelected))
            {
                UpdateSelectedCount();
            }
        }

        private void Group_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var it in e.NewItems.Cast<CleanupItem>())
                    it.PropertyChanged += CleanupItem_PropertyChanged;
            }
            if (e.OldItems != null)
            {
                foreach (var it in e.OldItems.Cast<CleanupItem>())
                    it.PropertyChanged -= CleanupItem_PropertyChanged;
            }
            UpdateSelectedCount();
        }

        private void CleanupItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CleanupItem.IsSelected))
            {
                UpdateSelectedCount();
            }
        }
    }
}
