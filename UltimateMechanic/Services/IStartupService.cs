using UltimateMechanic.Models;

namespace UltimateMechanic.Services;

public interface IStartupService
{
    Task<List<StartupItem>> GetStartupAppsAsync();
    Task ToggleStartupAppAsync(StartupItem item, bool enable);
}
