using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UltimateMechanic.Models;

public class CleanupGroup : ObservableCollection<CleanupItem>
{
    public string Title { get; set; } = string.Empty;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelected)));
                
                // CRITICAL FIX: Propagate selection to all children
                foreach (var item in this)
                {
                    item.IsSelected = value;
                }
            }
        }
    }

    public CleanupGroup(string title)
    {
        Title = title;
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        PropertyChanged?.Invoke(this, e);
    }
}
