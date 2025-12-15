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
