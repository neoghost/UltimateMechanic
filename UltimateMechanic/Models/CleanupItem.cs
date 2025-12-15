using CommunityToolkit.Mvvm.ComponentModel;

namespace UltimateMechanic.Models;

/// <summary>
/// Represents a single item that can be scanned and cleaned from the system.
/// Uses the MVVM Community Toolkit for automatic property notification.
/// </summary>
public partial class CleanupItem : ObservableObject
{
    /// <summary>Gets or sets the display name of the cleanup item.</summary>
    [ObservableProperty]
    private string name = string.Empty;

    /// <summary>Gets or sets the description of the cleanup item.</summary>
    [ObservableProperty]
    private string description = string.Empty;

    /// <summary>Gets or sets the file system path of the cleanup item.</summary>
    [ObservableProperty]
    private string path = string.Empty;

    /// <summary>Gets or sets the size of the item in bytes.</summary>
    [ObservableProperty]
    private long sizeBytes;

    /// <summary>Gets or sets a value indicating whether this item is selected for cleanup.</summary>
    [ObservableProperty]
    private bool isSelected;

    /// <summary>Gets or sets the category of this cleanup item.</summary>
    [ObservableProperty]
    private CleanupCategory category;

    /// <summary>Gets a human-readable display of the item size in megabytes.</summary>
    public string DisplaySize => $"{SizeBytes / 1024.0 / 1024.0:F2} MB";
}
