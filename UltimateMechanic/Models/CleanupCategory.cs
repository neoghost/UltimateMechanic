namespace UltimateMechanic.Models;

/// <summary>
/// Enumeration of cleanup categories that can be scanned and cleaned.
/// </summary>
public enum CleanupCategory
{
    /// <summary>Temporary files and system temp directories.</summary>
    TemporaryFiles,

    /// <summary>Files in the Recycle Bin.</summary>
    RecycleBin,

    /// <summary>Web browser cache files.</summary>
    BrowserCache,

    /// <summary>Windows thumbnail cache files.</summary>
    ThumbnailCache,

    /// <summary>Windows error and crash report files.</summary>
    ErrorReports,

    /// <summary>Windows system log files.</summary>
    WindowsLogs,

    /// <summary>Memory dump files.</summary>
    MemoryDumps
}
