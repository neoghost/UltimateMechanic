namespace UltimateMechanic.Models
{
    /// <summary>
    /// Contains comprehensive information about the system including CPU, memory, and drive usage.
    /// </summary>
    public class SystemInfo
    {
        /// <summary>Gets or sets the name of the processor.</summary>
        public string CpuName { get; set; } = string.Empty;

        /// <summary>Gets or sets the current CPU usage percentage.</summary>
        public double CpuUsage { get; set; }

        /// <summary>Gets or sets the total physical memory in megabytes.</summary>
        public long TotalMemoryMB { get; set; }

        /// <summary>Gets or sets the used physical memory in megabytes.</summary>
        public long UsedMemoryMB { get; set; }

        /// <summary>Gets or sets the memory usage as a percentage.</summary>
        public double MemoryUsagePercent { get; set; }

        /// <summary>Gets or sets the collection of disk drive information.</summary>
        public List<DriveInfo> Drives { get; set; } = new();
    }

    /// <summary>
    /// Contains information about a single disk drive including space usage.
    /// </summary>
    public class DriveInfo
    {
        /// <summary>Gets or sets the drive letter or name (e.g., "C:\").</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the total drive capacity in gigabytes.</summary>
        public long TotalSpaceGB { get; set; }

        /// <summary>Gets or sets the free available space on the drive in gigabytes.</summary>
        public long FreeSpaceGB { get; set; }

        /// <summary>Gets or sets the used space on the drive in gigabytes.</summary>
        public long UsedSpaceGB { get; set; }

        /// <summary>Gets or sets the usage percentage of the drive.</summary>
        public double UsagePercent { get; set; }
    }
}
