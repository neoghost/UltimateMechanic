using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateMechanic.Models;

namespace UltimateMechanic.Tests.UnitTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void CleanupItem_Properties_WorkCorrectly()
        {
            // Arrange
            var item = new CleanupItem
            {
                Name = "Test Item",
                SizeBytes = 1024 * 1024 * 5 // 5 MB
            };

            // Act & Assert
            Assert.AreEqual("5.00 MB", item.DisplaySize);
            Assert.AreEqual("Test Item", item.Name);
        }

        [TestMethod]
        public void CleanupGroup_IsSelected_TriggersNotification()
        {
            // Arrange
            var group = new CleanupGroup("Test Group");
            bool eventFired = false;
            group.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == "IsSelected") eventFired = true;
            };

            // Act
            group.IsSelected = true;

            // Assert
            Assert.IsTrue(eventFired);
            Assert.IsTrue(group.IsSelected);
        }
    }
}
