using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbaySalesTracker.Repository;
using Moq;
using System.Linq;

namespace EbaySalesTrackerTests.BllUnitTests
{
    [TestClass]
    public class InventoryBllTests
    {
        [TestMethod]
        public void CalculateQuantitySold_Successful()
        {
            //Arrange
            var listings = InventoryBllTestsHelpers.CreateListingsForTesting();

            var listingRepoMock = new Mock<IListingRepository>();
            listingRepoMock.Setup(x => x.GetListingsByInventoryItem(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(listings);

            var inventoryRepo = new InventoryRepository(listingRepoMock.Object);

            //Act
            var actual = inventoryRepo.CalculateQuantitySold(1, "test");

            //Assert
            Assert.AreEqual(listings.Count, actual);
        }

    }
}
