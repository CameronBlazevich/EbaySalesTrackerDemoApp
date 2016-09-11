using EbaySalesTracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EbaySalesTrackerTests
{
    [TestClass]
    public class AssertHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void ShouldFailForDifferentPropertyValues()
        {
            var actual = new InventoryItem() { Id = 1 };
            var expected = new InventoryItem() { Id = 2 };
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void ShouldFailForDifferentPropertyValues_MoreThanOne()
        {
            var actual = new InventoryItem() { Id = 1, QuantitySold = 2, AverageSalesPrice = 1.02999 };
            var expected = new InventoryItem() { Id = 1, QuantitySold = 2, AverageSalesPrice = 1.03 };
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }

        [TestMethod]      
        public void HasEqualPropertyValues_Successful()
        {
            var actual = new InventoryItem() { Id = 1, QuantitySold = 2, AverageSalesPrice = 1.03 };
            var expected = new InventoryItem() { Id = 1, QuantitySold = 2, AverageSalesPrice = 1.03 };
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }


        //NEED TO FORTIFY THIS WITH A LOT MORE TESTS. WILL BE RELYING ON THIS METHOD FOR A HUGE AMOUNT OF UNIT TESTS
    }
}
