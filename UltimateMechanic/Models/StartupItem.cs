using CommunityToolkit.Mvvm.ComponentModel;

namespace UltimateMechanic.Models;

public partial class StartupItem : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string path = string.Empty;

    [ObservableProperty]
    private bool isEnabled;

    [ObservableProperty]
    private string type = string.Empty; // "Registry", "Folder", or "Service"

    [ObservableProperty]
    private bool canModify = true;
}
