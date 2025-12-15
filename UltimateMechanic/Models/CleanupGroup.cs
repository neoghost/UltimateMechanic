using System.Collections.ObjectModel;
using System.Linq;

namespace UltimateMechanic.Models
{
    public class CleanupGroup : ObservableCollection<CleanupItem>
    {
        public CleanupGroup(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        // Exposes items as the collection itself for convenience
        public new ObservableCollection<CleanupItem> Items => this;

        private bool _isSelected = true;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                foreach (var item in Items)
                    item.IsSelected = value;
            }
        }

        public long TotalSize => Items.Sum(i => i.SizeBytes);
    }
}
