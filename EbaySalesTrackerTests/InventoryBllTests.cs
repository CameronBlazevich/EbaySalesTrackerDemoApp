using EbaySalesTracker.Bll;
using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbaySalesTrackerTests
{
    [TestClass]
    public class InventoryBllTests
    {
        [TestMethod]
        public void GetInventoryItemById_Successful()
        {
            //Arrange
            var inventoryItemId = 1;
            var userId = "1";
            var expected = new InventoryItem()
            {
                Id = 1,
                UserId = "1",
                AverageProfit = 1.00,
                AverageSalesPrice = 2.00,
                Cost = 1.00,
                QuantitySold = 1,
                Quantity = 3
            };
            var listingBll = new Mock<IListingBll>();
            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById(inventoryItemId, userId))
                .Returns(expected);

            var inventoryBll = new InventoryBll(listingBll.Object,inventoryRepo.Object);

            //Act
            var actual = inventoryBll.GetInventoryItemById(inventoryItemId, userId);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetInventoryItemById_IdDoesNotExist()
        {
            //Arrange
            var inventoryItemId = 1;
            var userId = "1";
            var expected = new InventoryItem();

            var listingBll = new Mock<IListingBll>();
            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById(inventoryItemId, userId))
                .Returns(expected);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);

            //Act
            var actual = inventoryBll.GetInventoryItemById(inventoryItemId, userId);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetInventoryItemsByUser_Successful_OneInvItem_ZeroListing()
        { //Arrange
            var userId = "1";
            var inventoryItemId = 0;

            var mockRepoReturnInventoryItem = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting().Where(x => x.Id == inventoryItemId).ToList();
            var expected = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting().Where(x => x.Id == inventoryItemId).ToList();
            //setting these up separate from mock to ensure calculations are checked
            expected.First().AverageProfit = 0;
            expected.First().AverageSalesPrice = 0;
            expected.First().QuantitySold = 0;

            var listings = new List<Listing>() { };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(mockRepoReturnInventoryItem);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);

            //Act
            var actual = inventoryBll.GetInventoryItemsByUser(userId);
            //Assert
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }

        [TestMethod]
        public void GetInventoryItemsByUser_Successful_OneInvItem_OneListing()
        { //Arrange
            var userId = "1";
            var inventoryItemId = 1;
            
            var expected = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting().Where(x=>x.Id == inventoryItemId).ToList();

            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };

            var listings = new List<Listing>() { listing1 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(expected);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);

            //Act
            var actual = inventoryBll.GetInventoryItemsByUser(userId);
            //Assert
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }

        [TestMethod]
        public void GetInventoryItemsByUser_Successful_OneInvItem_TwoListing()
        { 
            //Arrange
            var userId = "1";
            var inventoryItemId = 2;

            var mockRepoReturnInventoryItem = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting().Where(x => x.Id == inventoryItemId).ToList();
            var expected = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting().Where(x => x.Id == inventoryItemId).ToList();
            //setting these up separate from mock to ensure calculations are checked
            expected.First().QuantitySold = 2;
            expected.First().AverageProfit = 1.50;
            expected.First().AverageSalesPrice = 3.00;

            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };

            var listings = new List<Listing>() { listing1, listing2 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(mockRepoReturnInventoryItem);
            

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);

            //Act
            var actual = inventoryBll.GetInventoryItemsByUser(userId);
            
            //Assert
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }

        [TestMethod]
        public void GetBestSellingItem_Successful()
        {
            //Arrange
            var userId = "1";
            var inventoryItemId = 2;
            var items = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting();            
           
            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };

            var listings = new List<Listing>() { listing1, listing2, listing3 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(items);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);
            var expected = items.Where(x => x.Id == 2).First();

            //Act
            var actual = inventoryBll.GetBestSellingItem(userId);

            //Assert
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }

        [TestMethod]
        public void GetHighestAverageProfitItem_Successful()
        {
            //Arrange
            var userId = "1";
            var inventoryItemId = 2;
            var items = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting();

            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678912,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 5.00,
                QuantitySold = 1
            };
            var listing4 = new Listing()
            {
                ItemId = 12345678913,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 0.01,
                QuantitySold = 1
            };

            var listings = new List<Listing>() { listing1, listing2, listing3, listing4 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(items);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);
            var expected = items.Where(x => x.Id == 2).First();

            //Act
            var actual = inventoryBll.GetHighestAverageProfitItem(userId);

            //Assert
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }
       [TestMethod]
       public void GetProfitByMonth_Successful()
        {
            //Arrange
            var items = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting();

            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678912,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 5.00,
                QuantitySold = 1
            };
            var listing4 = new Listing()
            {
                ItemId = 12345678913,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 0.01,
                QuantitySold = 1
            };

            var listings = new List<Listing>() { listing1, listing2, listing3, listing4 };

            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingsByEndDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById(1, (It.IsAny<string>())))
                .Returns(items.Where(y => y.Id == 1).First());

            inventoryRepo.Setup(x => x.GetInventoryItemById(2, (It.IsAny<string>())))
                .Returns(items.Where(y => y.Id == 2).First());

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object);
            var expected = 5.01;
            //Act
            var actual = inventoryBll.GetProfitByMonth(2016, 1, "1");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetSalesByMonth_Successful()
        {
            //Arrange
            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                QuantitySold = 1
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678912,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 5.00,
                QuantitySold = 1
            };
            var listing4 = new Listing()
            {
                ItemId = 12345678913,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 0.01,
                QuantitySold = 1
            };

            var listings = new List<Listing>() { listing1, listing2, listing3, listing4 };

            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingsByEndDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(listings);

          
            var inventoryBll = new InventoryBll(listingBll.Object, null);
            var expected = 11.01;
            //Act
            var actual = inventoryBll.GetSalesByMonth(2016, 1, "1");

            //Assert
            Assert.AreEqual(expected, actual);
        }

    }    
}
