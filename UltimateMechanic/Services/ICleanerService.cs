using UltimateMechanic.Models;

namespace UltimateMechanic.Services
{
    public interface ICleanerService
    {
        Task<List<CleanupItem>> ScanForCleanupItemsAsync(IProgress<string>? progress = null);
        Task<long> CleanupItemsAsync(List<CleanupItem> items, IProgress<string>? progress = null);
        Task<long> GetTotalCleanupSizeAsync();
    }
}
