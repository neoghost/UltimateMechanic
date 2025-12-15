using UltimateMechanic.Models;

namespace UltimateMechanic.Services
{
    /// <summary>
    /// Service interface for scanning and cleaning unwanted files from the system.
    /// Provides methods to identify cleanup items and remove selected files.
    /// </summary>
    public interface ICleanerService
    {
        /// <summary>
        /// Scans the system for cleanup items asynchronously.
        /// </summary>
        /// <param name="progress">Optional progress reporter for scan operations.</param>
        /// <returns>A task that returns a list of CleanupItem objects found on the system.</returns>
        Task<List<CleanupItem>> ScanForCleanupItemsAsync(IProgress<string>? progress = null);

        /// <summary>
        /// Cleans up selected items from the system asynchronously.
        /// </summary>
        /// <param name="items">The list of cleanup items to process. Only items with IsSelected=true are cleaned.</param>
        /// <param name="progress">Optional progress reporter for cleanup operations.</param>
        /// <returns>A task that returns the total number of bytes cleaned.</returns>
        Task<long> CleanupItemsAsync(List<CleanupItem> items, IProgress<string>? progress = null);

        /// <summary>
        /// Gets the total size of all cleanup items that can be removed asynchronously.
        /// </summary>
        /// <returns>A task that returns the total size in bytes of all cleanup items.</returns>
        Task<long> GetTotalCleanupSizeAsync();
    }
}
