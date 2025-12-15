using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using UltimateMechanic.Models;
using UltimateMechanic.Services;

namespace UltimateMechanic.ViewModels
{
    /// <summary>
    /// View model for the disk cleanup functionality.
    /// Manages scanning for cleanup items, selecting items for cleanup, and performing the cleanup operation.
    /// </summary>
    public partial class CleanerViewModel : ObservableObject
    {
        private readonly ICleanerService _cleanerService;

        /// <summary>Gets or sets the collection of cleanup item groups organized by category.</summary>
        [ObservableProperty]
        private ObservableCollection<CleanupGroup> _cleanupGroups = new();

        /// <summary>Gets or sets a value indicating whether a scan operation is in progress.</summary>
        [ObservableProperty]
        private bool _isScanning;

        /// <summary>Gets or sets a value indicating whether a cleanup operation is in progress.</summary>
        [ObservableProperty]
        private bool _isCleaning;

        /// <summary>Gets or sets the status message for the current operation.</summary>
        [ObservableProperty]
        private string _statusMessage = "Ready to scan";

        /// <summary>Gets or sets the total size of all cleanup items in bytes.</summary>
        [ObservableProperty]
        private long _totalSizeBytes;

        /// <summary>Gets or sets the number of selected cleanup items.</summary>
        [ObservableProperty]
        private int _selectedCount;

        /// <summary>Gets the total size of all cleanup items formatted as a human-readable string.</summary>
        public string TotalSizeFormatted => FormatBytes(TotalSizeBytes);

        /// <summary>
        /// Initializes a new instance of the CleanerViewModel class.
        /// </summary>
        /// <param name="cleanerService">The cleaner service for performing scan and cleanup operations.</param>
        public CleanerViewModel(ICleanerService cleanerService)
        {
            _cleanerService = cleanerService;
        }

        /// <summary>
        /// Updates the selected item count and total size based on current selections.
        /// Should be called whenever item selections change.
        /// </summary>
        public void UpdateSelectedCount()
        {
            SelectedCount = CleanupGroups.Sum(g => g.Count(i => i.IsSelected));
            TotalSizeBytes = CleanupGroups.Sum(g => g.Sum(i => i.SizeBytes));
        }

        /// <summary>
        /// Scans the system for cleanup items asynchronously.
        /// Groups found items by category and updates the UI with results.
        /// </summary>
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

        /// <summary>
        /// Performs cleanup on all selected items asynchronously.
        /// Removes selected files and directories, then rescans to update the list.
        /// </summary>
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

        /// <summary>
        /// Selects all cleanup items for cleaning.
        /// </summary>
        [RelayCommand]
        private void SelectAll()
        {
            foreach (var grp in CleanupGroups)
            {
                grp.IsSelected = true;
            }
            UpdateSelectedCount();
        }

        /// <summary>
        /// Deselects all cleanup items.
        /// </summary>
        [RelayCommand]
        private void DeselectAll()
        {
            foreach (var grp in CleanupGroups)
            {
                grp.IsSelected = false;
            }
            UpdateSelectedCount();
        }

        /// <summary>
        /// Formats a byte value into a human-readable size string.
        /// </summary>
        /// <param name="bytes">The size in bytes.</param>
        /// <returns>A formatted string like "1.50 MB" or "2.5 GB".</returns>
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

        /// <summary>
        /// Handles the TotalSizeBytes changed event to update the formatted size display.
        /// </summary>
        /// <param name="value">The new total size in bytes.</param>
        partial void OnTotalSizeBytesChanged(long value)
        {
            OnPropertyChanged(nameof(TotalSizeFormatted));
        }

        /// <summary>
        /// Gets the display name for a cleanup category.
        /// </summary>
        /// <param name="category">The cleanup category.</param>
        /// <returns>A user-friendly display name for the category.</returns>
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


        /// <summary>
        /// Handles property changed events from cleanup groups.
        /// Updates the selected count when a group's selection state changes.
        /// </summary>
        /// <param name="sender">The group that changed.</param>
        /// <param name="e">The property change event arguments.</param>
        private void Group_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CleanupGroup.IsSelected))
            {
                UpdateSelectedCount();
            }
        }

        /// <summary>
        /// Handles collection changed events from cleanup groups.
        /// Subscribes to property changes on new items and unsubscribes from removed items.
        /// </summary>
        /// <param name="sender">The group collection that changed.</param>
        /// <param name="e">The collection change event arguments.</param>
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

        /// <summary>
        /// Handles property changed events from individual cleanup items.
        /// Updates the selected count when an item's selection state changes.
        /// </summary>
        /// <param name="sender">The cleanup item that changed.</param>
        /// <param name="e">The property change event arguments.</param>
        private void CleanupItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CleanupItem.IsSelected))
            {
                UpdateSelectedCount();
            }
        }
    }
}
