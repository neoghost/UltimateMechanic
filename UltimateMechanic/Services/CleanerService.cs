using System.IO;
using UltimateMechanic.Models;

namespace UltimateMechanic.Services
{
    /// <summary>
    /// Service for scanning and cleaning unwanted files from the system.
    /// Identifies cleanup items in various system directories including temp files, cache, and logs.
    /// </summary>
    public class CleanerService : ICleanerService
    {
        /// <summary>
        /// Scans the system for cleanup items asynchronously.
        /// Searches Windows temp, user temp, prefetch, recycle bin, browser caches, and other cleanup targets.
        /// </summary>
        /// <param name="progress">Optional progress reporter for scan operations.</param>
        /// <returns>A list of CleanupItem objects found on the system.</returns>
        public async Task<List<CleanupItem>> ScanForCleanupItemsAsync(IProgress<string>? progress = null)
        {
            var items = new List<CleanupItem>();

            await Task.Run(() =>
            {
                // Windows Temp Files
                progress?.Report("Scanning Windows Temp files...");
                ScanDirectory(items, Path.GetTempPath(), "Windows Temp Files", CleanupCategory.TemporaryFiles);

                // User Temp Files
                var userTemp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp";
                if (Directory.Exists(userTemp))
                {
                    progress?.Report("Scanning User Temp files...");
                    ScanDirectory(items, userTemp, "User Temp Files", CleanupCategory.TemporaryFiles);
                }

                // Windows Prefetch
                var prefetch = @"C:\Windows\Prefetch";
                if (Directory.Exists(prefetch))
                {
                    progress?.Report("Scanning Prefetch files...");
                    ScanDirectory(items, prefetch, "Windows Prefetch", CleanupCategory.TemporaryFiles);
                }

                // Recycle Bin
                progress?.Report("Scanning Recycle Bin...");
                ScanRecycleBin(items);

                // Browser Caches
                progress?.Report("Scanning Browser caches...");
                ScanBrowserCaches(items);

                // Thumbnail Cache
                progress?.Report("Scanning Thumbnail cache...");
                ScanThumbnailCache(items);

                // Windows Error Reports
                progress?.Report("Scanning Error reports...");
                ScanErrorReports(items);

                // Windows Logs
                progress?.Report("Scanning Windows logs...");
                ScanWindowsLogs(items);
            });

            return items;
        }

        /// <summary>
        /// Cleans up selected items from the system asynchronously.
        /// Deletes files and directories marked for cleanup, reporting progress as items are removed.
        /// </summary>
        /// <param name="items">The list of cleanup items to process. Only items with IsSelected=true are cleaned.</param>
        /// <param name="progress">Optional progress reporter for cleanup operations.</param>
        /// <returns>The total number of bytes cleaned.</returns>
        public async Task<long> CleanupItemsAsync(List<CleanupItem> items, IProgress<string>? progress = null)
        {
            long totalCleaned = 0;

            await Task.Run(() =>
            {
                foreach (var item in items.Where(i => i.IsSelected))
                {
                    try
                    {
                        progress?.Report($"Cleaning: {item.Name}");

                        if (File.Exists(item.Path))
                        {
                            File.Delete(item.Path);
                            totalCleaned += item.SizeBytes;
                        }
                        else if (Directory.Exists(item.Path))
                        {
                            Directory.Delete(item.Path, true);
                            totalCleaned += item.SizeBytes;
                        }
                    }
                    catch
                    {
                        // Skip files that can't be deleted (in use, permissions, etc.)
                    }
                }
            });

            return totalCleaned;
        }

        /// <summary>
        /// Gets the total size of all cleanup items that can be removed asynchronously.
        /// </summary>
        /// <returns>The total size in bytes of all cleanup items.</returns>
        public async Task<long> GetTotalCleanupSizeAsync()
        {
            var items = await ScanForCleanupItemsAsync();
            return items.Sum(i => i.SizeBytes);
        }

        /// <summary>
        /// Scans a directory recursively for cleanup items.
        /// </summary>
        /// <param name="items">The list to add found items to.</param>
        /// <param name="path">The directory path to scan.</param>
        /// <param name="name">The display name for items found in this directory.</param>
        /// <param name="category">The cleanup category for found items.</param>
        private void ScanDirectory(List<CleanupItem> items, string path, string name, CleanupCategory category)
        {
            try
            {
                if (!Directory.Exists(path))
                    return;

                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                
                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        items.Add(new CleanupItem
                        {
                            Name = $"{name} - {Path.GetFileName(file)}",
                            Description = file,
                            Path = file,
                            SizeBytes = fileInfo.Length,
                            Category = category
                        });
                    }
                    catch
                    {
                        // Skip files we can't access
                    }
                }
            }
            catch
            {
                // Skip directories we can't access
            }
        }

        /// <summary>
        /// Scans the Recycle Bin for items that can be cleaned.
        /// </summary>
        /// <param name="items">The list to add recycle bin items to.</param>
        private void ScanRecycleBin(List<CleanupItem> items)
        {
            try
            {
                var recycleBinPath = @"C:\$Recycle.Bin";
                if (Directory.Exists(recycleBinPath))
                {
                    var size = GetDirectorySize(recycleBinPath);
                    if (size > 0)
                    {
                        items.Add(new CleanupItem
                        {
                            Name = "Recycle Bin",
                            Description = "Empty Recycle Bin",
                            Path = recycleBinPath,
                            SizeBytes = size,
                            Category = CleanupCategory.RecycleBin
                        });
                    }
                }
            }
            catch
            {
                // Skip if we can't access
            }
        }

        /// <summary>
        /// Scans browser cache directories for Chrome, Edge, and Firefox.
        /// </summary>
        /// <param name="items">The list to add browser cache items to.</param>
        private void ScanBrowserCaches(List<CleanupItem> items)
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Chrome Cache
            var chromeCache = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cache");
            if (Directory.Exists(chromeCache))
            {
                ScanDirectory(items, chromeCache, "Chrome Cache", CleanupCategory.BrowserCache);
            }

            // Edge Cache
            var edgeCache = Path.Combine(localAppData, @"Microsoft\Edge\User Data\Default\Cache");
            if (Directory.Exists(edgeCache))
            {
                ScanDirectory(items, edgeCache, "Edge Cache", CleanupCategory.BrowserCache);
            }

            // Firefox Cache
            var firefoxCache = Path.Combine(localAppData, @"Mozilla\Firefox\Profiles");
            if (Directory.Exists(firefoxCache))
            {
                try
                {
                    foreach (var profile in Directory.GetDirectories(firefoxCache))
                    {
                        var cacheFolder = Path.Combine(profile, "cache2");
                        if (Directory.Exists(cacheFolder))
                        {
                            ScanDirectory(items, cacheFolder, "Firefox Cache", CleanupCategory.BrowserCache);
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Scans the Windows thumbnail cache directory.
        /// </summary>
        /// <param name="items">The list to add thumbnail cache items to.</param>
        private void ScanThumbnailCache(List<CleanupItem> items)
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var thumbCache = Path.Combine(localAppData, @"Microsoft\Windows\Explorer");
            
            if (Directory.Exists(thumbCache))
            {
                ScanDirectory(items, thumbCache, "Thumbnail Cache", CleanupCategory.ThumbnailCache);
            }
        }

        /// <summary>
        /// Scans the Windows error reports directory.
        /// </summary>
        /// <param name="items">The list to add error report items to.</param>
        private void ScanErrorReports(List<CleanupItem> items)
        {
            var errorReports = @"C:\ProgramData\Microsoft\Windows\WER\ReportQueue";
            if (Directory.Exists(errorReports))
            {
                ScanDirectory(items, errorReports, "Error Reports", CleanupCategory.ErrorReports);
            }
        }

        /// <summary>
        /// Scans the Windows logs directory.
        /// </summary>
        /// <param name="items">The list to add Windows log items to.</param>
        private void ScanWindowsLogs(List<CleanupItem> items)
        {
            var logsPath = @"C:\Windows\Logs";
            if (Directory.Exists(logsPath))
            {
                ScanDirectory(items, logsPath, "Windows Logs", CleanupCategory.WindowsLogs);
            }
        }

        /// <summary>
        /// Calculates the total size of all files in a directory and its subdirectories.
        /// </summary>
        /// <param name="path">The directory path to calculate size for.</param>
        /// <returns>The total size in bytes.</returns>
        private long GetDirectorySize(string path)
        {
            try
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                return files.Sum(file =>
                {
                    try
                    {
                        return new FileInfo(file).Length;
                    }
                    catch
                    {
                        return 0;
                    }
                });
            }
            catch
            {
                return 0;
            }
        }
    }
}
