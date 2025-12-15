using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using UltimateMechanic.Models;

namespace UltimateMechanic.Services;

public class StartupService : IStartupService
{
    private const string RunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public async Task<List<StartupItem>> GetStartupAppsAsync()
    {
        var items = new List<StartupItem>();

        await Task.Run(() =>
        {
            // 1. Scan Current User Registry
            using (var key = Registry.CurrentUser.OpenSubKey(RunPath))
            {
                if (key != null)
                {
                    foreach (var name in key.GetValueNames())
                    {
                        items.Add(new StartupItem
                        {
                            Name = name,
                            Path = key.GetValue(name)?.ToString() ?? "",
                            IsEnabled = true,
                            Type = "Registry (User)"
                        });
                    }
                }
            }

            // 2. Scan Disabled Startup Items (Simple check in specialized registry keys is complex, 
            // generally strictly managing Enabled items is safer for non-admin apps)
            
            // 3. Scan Common Startup Folder
            var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (Directory.Exists(startupPath))
            {
                foreach (var file in Directory.GetFiles(startupPath))
                {
                    items.Add(new StartupItem
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        Path = file,
                        IsEnabled = true,
                        Type = "Startup Folder"
                    });
                }
            }
        });

        return items;
    }

    public async Task ToggleStartupAppAsync(StartupItem item, bool enable)
    {
        await Task.Run(() =>
        {
            if (item.Type.Contains("Registry"))
            {
                using var key = Registry.CurrentUser.OpenSubKey(RunPath, true);
                if (enable)
                {
                    key?.SetValue(item.Name, item.Path);
                }
                else
                {
                    key?.DeleteValue(item.Name, false);
                }
            }
            // Note: Folder startup items require moving files to a "Disabled" folder, skipped for safety here.
        });
    }
}
