using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UltimateMechanic.Models;
using UltimateMechanic.Services;

namespace UltimateMechanic.ViewModels
{
    /// <summary>
    /// View model for displaying and refreshing system information.
    /// Provides CPU, memory, and drive usage data with automatic periodic refresh.
    /// </summary>
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly System.Windows.Threading.DispatcherTimer _refreshTimer;
        private bool _isAutoRefreshing;

        /// <summary>Gets or sets the name of the processor.</summary>
        [ObservableProperty]
        private string _cpuName = "Loading...";

        /// <summary>Gets or sets the current CPU usage percentage.</summary>
        [ObservableProperty]
        private double _cpuUsage;

        /// <summary>Gets or sets the total physical memory in megabytes.</summary>
        [ObservableProperty]
        private long _totalMemoryMB;

        /// <summary>Gets or sets the used physical memory in megabytes.</summary>
        [ObservableProperty]
        private long _usedMemoryMB;

        /// <summary>Gets or sets the memory usage as a percentage.</summary>
        [ObservableProperty]
        private double _memoryUsagePercent;

        /// <summary>Gets or sets the collection of disk drive information.</summary>
        [ObservableProperty]
        private ObservableCollection<Models.DriveInfo> _drives = new();

        /// <summary>Gets or sets a value indicating whether system information is being loaded.</summary>
        [ObservableProperty]
        private bool _isLoading = true;

        /// <summary>
        /// Initializes a new instance of the DashboardViewModel class.
        /// Sets up automatic refresh timer to update system information every 5 seconds.
        /// </summary>
        /// <param name="systemInfoService">The system information service.</param>
        public DashboardViewModel(ISystemInfoService systemInfoService)
        {
            _systemInfoService = systemInfoService;
            _refreshTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _refreshTimer.Tick += async (s, e) =>
            {
                if (_isAutoRefreshing) return;
                _isAutoRefreshing = true;
                try { await LoadSystemInfoAsync(); }
                finally { _isAutoRefreshing = false; }
            };
            _refreshTimer.Start();
        }

        /// <summary>
        /// Loads system information asynchronously from the system information service.
        /// Updates all observable properties with current system data.
        /// </summary>
        [RelayCommand]
        private async Task LoadSystemInfoAsync()
        {
            IsLoading = true;

            try
            {
                var info = await _systemInfoService.GetSystemInfoAsync();
                
                CpuName = info.CpuName;
                CpuUsage = info.CpuUsage;
                TotalMemoryMB = info.TotalMemoryMB;
                UsedMemoryMB = info.UsedMemoryMB;
                MemoryUsagePercent = info.MemoryUsagePercent;
                
                Drives.Clear();
                foreach (var drive in info.Drives)
                {
                    Drives.Add(drive);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Refreshes the system information display by reloading data from the service.
        /// </summary>
        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadSystemInfoAsync();
        }
    }
}
