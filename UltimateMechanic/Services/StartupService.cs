using Microsoft.Win32;
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

            // 2. Scan Startup Folder
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
                    // For registry items, disabling usually means deleting the value.
                    // To keep it "disabled" but visible would require a separate storage mechanism 
                    // or moving it to a "disabled" registry key. 
                    // For this simple implementation, disabling removes it from Run.
                    key?.DeleteValue(item.Name, false);
                }
            }
        });
    }

    public async Task DeleteStartupAppAsync(StartupItem item)
    {
        await Task.Run(() =>
        {
            try 
            {
                if (item.Type.Contains("Registry"))
                {
                    using var key = Registry.CurrentUser.OpenSubKey(RunPath, true);
                    // The 'false' parameter means "don't throw error if missing"
                    key?.DeleteValue(item.Name, false);
                }
                else if (item.Type.Contains("Folder") && File.Exists(item.Path))
                {
                    File.Delete(item.Path);
                }
            }
            catch 
            {
                // Handle permission errors or locks gracefully
            }
        });
    }
}
