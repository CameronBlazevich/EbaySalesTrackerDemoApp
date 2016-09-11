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

        //[TestMethod]
        //public void CalculateAverageSalesPriceAndProfit_Successful()
        //{
        //    //Arrange

        //    var listings = InventoryRepositoryTestHelpers.CreateListingsForTesting();
        //    var itemId = 1;
        //    var userId = "1";

        //    var listingsRepo = new Mock<IListingRepository>();

        //    var inventoryRepo = new InventoryRepository(listingsRepo.Object);

        //    listingsRepo.Setup(x => x.GetListingsByInventoryItem(itemId,userId)).Returns(listings.Where(y => y.InventoryItemId == itemId && y.TotalNetFees > 0 && y.QuantitySold > 0).ToList());

        //    //Act
        //    var testListings = listingsRepo.Object.GetListingsByInventoryItem(itemId,"1");
        //    var averages = inventoryRepo.CalculateAverageSalesPriceAndProfit(itemId,"1");

        //    //Assert
        //    Assert.AreEqual(22.5, Math.Round(averages[1], 2));
        //    Assert.AreEqual(25.08, Math.Round(averages[0],2));

        //}
        //[TestMethod]
        //public void CalculateQuantitySold_Successful()
        //{
        //    //Arrange
        //    var listings = InventoryRepositoryTestHelpers.CreateListingsForTesting();
        //    var itemId = 1;
        //    var userId = "1";
            
        //    var listingsRepo = new Mock<IListingRepository>();
        //    var inventoryRepo = new InventoryRepository(listingsRepo.Object);

        //    listingsRepo.Setup(x => x.GetListingsByInventoryItem(itemId,userId)).Returns(listings.Where(y => y.InventoryItemId == itemId && y.TotalNetFees > 0 && y.QuantitySold > 0).ToList());

        //    //Act
        //    var testListings = listingsRepo.Object.GetListingsByInventoryItem(itemId,userId);
        //   var quantitySold = inventoryRepo.CalculateQuantitySold(itemId,"1");

        //    //Assert
        //    Assert.AreEqual(3, quantitySold);

        //}
        //[TestMethod]
        //public void GetBestSellingItem_Successful()
        //{
        //    //NEED TO MOCK DATA.CONTEXT
        //    //var mockRepositoryBase = new Mock<RepositoryBase<EbaySalesTrackerContext>>();
        //    //mockRepositoryBase.Setup(x => x.)



        //    //Arrange
        //    var items = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting();
        //    var userId = "1";
        //    var mockInventoryRepo = new Mock<IInventoryRepository>();
        //    mockInventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId)).Returns(items);

        //    //var inventoryRepo = new Mock<InventoryRepository();
        //    //Act
        //    var item = mockInventoryRepo.Object.GetBestSellingItem(userId);

        //    //Assert
        //    Assert.AreEqual(0, item.Id);
        //}
    
    }
}
