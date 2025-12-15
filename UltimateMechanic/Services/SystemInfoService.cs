using System.Diagnostics;
using System.IO;
using System.Management;
using UltimateMechanic.Models;

namespace UltimateMechanic.Services
{
    public class SystemInfoService : ISystemInfoService
    {
        private PerformanceCounter? _cpuCounter;

        public SystemInfoService()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _cpuCounter.NextValue(); // First call always returns 0
            }
            catch
            {
                // Performance counters might not be available
            }
        }

        public async Task<SystemInfo> GetSystemInfoAsync()
        {
            var info = new SystemInfo
            {
                CpuName = GetCpuName(),
                CpuUsage = await GetCpuUsageAsync(),
                Drives = GetDriveInfo()
            };

            var (total, used) = await GetMemoryInfoAsync();
            info.TotalMemoryMB = total;
            info.UsedMemoryMB = used;
            info.MemoryUsagePercent = total > 0 ? (double)used / total * 100 : 0;

            return info;
        }

        public async Task<double> GetCpuUsageAsync()
        {
            if (_cpuCounter == null)
                return 0;

            await Task.Delay(100); // Small delay for accurate reading
            return Math.Round(_cpuCounter.NextValue(), 1);
        }

        public Task<(long total, long used)> GetMemoryInfoAsync()
        {
            var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024 * 1024);
            var usedMemory = (GC.GetGCMemoryInfo().TotalAvailableMemoryBytes - 
                             GC.GetGCMemoryInfo().MemoryLoadBytes) / (1024 * 1024);

            // Get actual system memory using WMI
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    var total = Convert.ToInt64(obj["TotalVisibleMemorySize"]) / 1024; // Convert KB to MB
                    var free = Convert.ToInt64(obj["FreePhysicalMemory"]) / 1024;
                    return Task.FromResult((total, total - free));
                }
            }
            catch
            {
                // Fallback to GC info
            }

            return Task.FromResult((totalMemory, usedMemory));
        }

        private string GetCpuName()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["Name"]?.ToString() ?? "Unknown CPU";
                }
            }
            catch
            {
                return "Unknown CPU";
            }

            return "Unknown CPU";
        }

        private List<Models.DriveInfo> GetDriveInfo()
        {
            var drives = new List<Models.DriveInfo>();

            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    var totalGB = drive.TotalSize / (1024 * 1024 * 1024);
                    var freeGB = drive.AvailableFreeSpace / (1024 * 1024 * 1024);
                    var usedGB = totalGB - freeGB;

                    drives.Add(new Models.DriveInfo
                    {
                        Name = drive.Name,
                        TotalSpaceGB = totalGB,
                        FreeSpaceGB = freeGB,
                        UsedSpaceGB = usedGB,
                        UsagePercent = totalGB > 0 ? (double)usedGB / totalGB * 100 : 0
                    });
                }
            }

            return drives;
        }
    }
}
