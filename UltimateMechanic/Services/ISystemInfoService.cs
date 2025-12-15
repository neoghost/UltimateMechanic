using UltimateMechanic.Models;

namespace UltimateMechanic.Services
{
    /// <summary>
    /// Service interface for retrieving system information including CPU, memory, and drive status.
    /// </summary>
    public interface ISystemInfoService
    {
        /// <summary>
        /// Gets complete system information asynchronously.
        /// </summary>
        /// <returns>A task that returns a SystemInfo object containing CPU, memory, and drive information.</returns>
        Task<SystemInfo> GetSystemInfoAsync();

        /// <summary>
        /// Gets the current CPU usage percentage asynchronously.
        /// </summary>
        /// <returns>A task that returns the CPU usage as a percentage.</returns>
        Task<double> GetCpuUsageAsync();

        /// <summary>
        /// Gets the total and used memory information asynchronously.
        /// </summary>
        /// <returns>A task that returns a tuple with total memory and used memory in megabytes.</returns>
        Task<(long total, long used)> GetMemoryInfoAsync();
    }
}
