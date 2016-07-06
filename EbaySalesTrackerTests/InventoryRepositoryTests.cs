using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EbaySalesTrackerTests
{
    [TestClass]
    public class InventoryRepositoryTests
    {
        [TestMethod]
        public void CreateInventoryItem_Should_Call_SaveChanges_Successful()
        {
            //Arrange
            var item = InventoryRepositoryTestHelpers.CreateInventoryItemForTesting();

            var inventoryRepo = new Mock<InventoryRepository>();

            //Act
            inventoryRepo.Setup(x=>x.DataContext.InventoryItems.Add(It.IsAny<InventoryItem>())).Returns(item);
            inventoryRepo.Setup(x => x.DataContext.SaveChanges());
            inventoryRepo.Object.CreateInventoryItem(item);

            //Assert
            inventoryRepo.Verify();
        }

        [TestMethod]
        public void CalculateAverageSalesPriceAndProfit_Successful()
        {
            //Arrange

            var listings = InventoryRepositoryTestHelpers.CreateListingsForTesting();
            var itemId = 1;

            var inventoryRepo = new Mock<InventoryRepository>();
            inventoryRepo.Setup(x => x.GetListings(itemId)).Returns(listings.Where(y => y.InventoryItemId == itemId && y.TotalNetFees > 0 && y.QuantitySold > 0).ToList());

            //Act
            var averages = inventoryRepo.Object.CalculateAverageSalesPriceAndProfit(1);

            //Assert
            Assert.AreEqual(22.5, Math.Round(averages[1], 2));
            Assert.AreEqual(25.08, Math.Round(averages[0],2));

        }
    
    }
}
