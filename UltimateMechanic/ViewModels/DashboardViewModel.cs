using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UltimateMechanic.Models;
using UltimateMechanic.Services;

namespace UltimateMechanic.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly System.Windows.Threading.DispatcherTimer _refreshTimer;
        private bool _isAutoRefreshing;

        [ObservableProperty]
        private string _cpuName = "Loading...";

        [ObservableProperty]
        private double _cpuUsage;

        [ObservableProperty]
        private long _totalMemoryMB;

        [ObservableProperty]
        private long _usedMemoryMB;

        [ObservableProperty]
        private double _memoryUsagePercent;

        [ObservableProperty]
        private ObservableCollection<Models.DriveInfo> _drives = new();

        [ObservableProperty]
        private bool _isLoading = true;

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

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadSystemInfoAsync();
        }
    }
}
