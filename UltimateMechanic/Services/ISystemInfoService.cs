using UltimateMechanic.Models;

namespace UltimateMechanic.Services
{
    public interface ISystemInfoService
    {
        Task<SystemInfo> GetSystemInfoAsync();
        Task<double> GetCpuUsageAsync();
        Task<(long total, long used)> GetMemoryInfoAsync();
    }
}
