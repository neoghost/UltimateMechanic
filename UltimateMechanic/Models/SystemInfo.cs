namespace UltimateMechanic.Models
{
    public class SystemInfo
    {
        public string CpuName { get; set; } = string.Empty;
        public double CpuUsage { get; set; }
        public long TotalMemoryMB { get; set; }
        public long UsedMemoryMB { get; set; }
        public double MemoryUsagePercent { get; set; }
        public List<DriveInfo> Drives { get; set; } = new();
    }

    public class DriveInfo
    {
        public string Name { get; set; } = string.Empty;
        public long TotalSpaceGB { get; set; }
        public long FreeSpaceGB { get; set; }
        public long UsedSpaceGB { get; set; }
        public double UsagePercent { get; set; }
    }
}
