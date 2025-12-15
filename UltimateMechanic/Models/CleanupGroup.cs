using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UltimateMechanic.Models;

/// <summary>
/// Represents a group of cleanup items that can be selected and cleaned together.
/// Extends ObservableCollection to support data binding and notifies when the IsSelected property changes.
/// </summary>
public class CleanupGroup : ObservableCollection<CleanupItem>
{
    /// <summary>Gets or sets the display title of this cleanup group.</summary>
    public string Title { get; set; } = string.Empty;

    private bool _isSelected;

    /// <summary>
    /// Gets or sets a value indicating whether this group and its items are selected for cleanup.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the CleanupGroup class with a title.
    /// </summary>
    /// <param name="title">The display title of the group.</param>
    public CleanupGroup(string title)
    {
        Title = title;
    }

    /// <summary>Occurs when a property value changes.</summary>
    public new event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="e">The event arguments containing the property name.</param>
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        PropertyChanged?.Invoke(this, e);
    }
}
