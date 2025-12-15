using Xunit;
using UltimateMechanic.Models;

namespace UltimateMechanic.Tests.UnitTests;

public class ModelTests
{
    [Fact]
    public void SystemInfo_Defaults_AreExpected()
    {
        var info = new SystemInfo();

        Assert.NotNull(info);
        Assert.Equal(string.Empty, info.CpuName);
        Assert.Equal(0, info.CpuUsage);
        Assert.Equal(0, info.TotalMemoryMB);
        Assert.Equal(0, info.UsedMemoryMB);
        Assert.Equal(0, info.MemoryUsagePercent);
        Assert.NotNull(info.Drives);
        Assert.Empty(info.Drives);
    }

    [Fact]
    public void DriveInfo_ManualValues_CalculationExample()
    {
        var drive = new DriveInfo
        {
            Name = "C:\\",
            TotalSpaceGB = 100,
            FreeSpaceGB = 40,
            UsedSpaceGB = 60,
            UsagePercent = 60.0
        };

        Assert.Equal("C:\\", drive.Name);
        Assert.Equal(100, drive.TotalSpaceGB);
        Assert.Equal(40, drive.FreeSpaceGB);
        Assert.Equal(60, drive.UsedSpaceGB);
        Assert.Equal(60.0, drive.UsagePercent);
    }
}
