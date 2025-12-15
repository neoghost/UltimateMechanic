using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateMechanic.Models;

namespace UltimateMechanic.Tests.UnitTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void SystemInfo_Defaults_AreExpected()
        {
            var info = new SystemInfo();

            Assert.IsNotNull(info);
            Assert.AreEqual(string.Empty, info.CpuName);
            Assert.AreEqual(0, info.CpuUsage);
            Assert.AreEqual(0, info.TotalMemoryMB);
            Assert.AreEqual(0, info.UsedMemoryMB);
            Assert.AreEqual(0, info.MemoryUsagePercent);
            Assert.IsNotNull(info.Drives);
            Assert.AreEqual(0, info.Drives.Count);
        }

        [TestMethod]
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

            Assert.AreEqual("C:\\", drive.Name);
            Assert.AreEqual(100, drive.TotalSpaceGB);
            Assert.AreEqual(40, drive.FreeSpaceGB);
            Assert.AreEqual(60, drive.UsedSpaceGB);
            Assert.AreEqual(60.0, drive.UsagePercent);
        }
    }
}
