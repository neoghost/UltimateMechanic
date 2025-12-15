namespace UltimateMechanic.Models
{
    public class CleanupItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public bool IsSelected { get; set; } = true;
        public CleanupCategory Category { get; set; }
    }

    public enum CleanupCategory
    {
        TemporaryFiles,
        BrowserCache,
        RecycleBin,
        WindowsLogs,
        ThumbnailCache,
        MemoryDumps,
        ErrorReports
    }
}
