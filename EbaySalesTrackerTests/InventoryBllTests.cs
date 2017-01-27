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
            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object,inventoryRepo.Object, transRepo.Object);

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
            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);

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
            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);

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
            var transaction1 = CreateTransaction(3, 1);
            
            var expected = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting().Where(x=>x.Id == inventoryItemId).ToList();

            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1 }
            };

            var listings = new List<Listing>() { listing1 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(expected);
            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);

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
            var transaction1 = CreateTransaction(3, 1);

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
                Transactions = new List<ListingTransaction> { transaction1 }
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1 }
            };

            var listings = new List<Listing>() { listing1, listing2 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(mockRepoReturnInventoryItem);

            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);

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

            var transaction1 = new ListingTransaction
            {
                QuantitySold = 3,
                UnitPrice = 10
            };
            var transaction2 = new ListingTransaction
            {
                QuantitySold = 1,
                UnitPrice = 10
            };
            var transaction3 = new ListingTransaction
            {
                QuantitySold = 0,
                UnitPrice = 10
            };
            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                Transactions = new List<ListingTransaction> { transaction1 }
                
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                Transactions = new List<ListingTransaction> { transaction2 }
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                CurrentPrice = 3.00,
                Transactions = new List<ListingTransaction> { transaction3 }
            };

            var listings = new List<Listing>() { listing1, listing2, listing3 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings.Where(l => l.InventoryItemId == inventoryItemId));

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(items);

            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);
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

            var transaction1 = CreateTransaction(3, 1);
            var transaction2 = CreateTransaction(5, 1);
            var transaction3 = CreateTransaction(.01, 1);


            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1 }
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1 }
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678912,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction2 }
            };
            var listing4 = new Listing()
            {
                ItemId = 12345678913,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction3 }
            };

            var listings = new List<Listing>() { listing1, listing2, listing3, listing4 };
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetSoldListingsByInventoryItem(inventoryItemId, It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemsByUser(userId))
                .Returns(items);
            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);
            var expected = items.Where(x => x.Id == 2).First();

            //Act
            var actual = inventoryBll.GetHighestAverageProfitItem(userId);

            //Assert
            AssertHelper.HasEqualPropertyValues(expected, actual);
        }
        private ListingTransaction CreateTransaction(double unitPrice, int qtySold)
        {
            return new ListingTransaction { UnitPrice = unitPrice, QuantitySold = qtySold };
        }
       [TestMethod]
       public void GetProfitByMonth_Successful()
        {
            //Arrange
            var items = InventoryRepositoryTestHelpers.CreateListOfInventoryItemsForTesting();

            var transaction1 = CreateTransaction(3, 1);
            var transaction2 = CreateTransaction(5, 1);
            var transaction3 = CreateTransaction(0.01, 1);
            
            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1}
            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1 }
            };
            var listing3 = new Listing()
            {
                ItemId = 12345678912,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction2 }
            };
            var listing4 = new Listing()
            {
                ItemId = 12345678913,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction3 }
            };

            var listings = new List<Listing>() { listing1, listing2, listing3, listing4 };

            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingsBySoldDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(listings);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById(1, (It.IsAny<string>())))
                .Returns(items.Where(y => y.Id == 1).First());

            inventoryRepo.Setup(x => x.GetInventoryItemById(2, (It.IsAny<string>())))
                .Returns(items.Where(y => y.Id == 2).First());
            var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);
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
            var transaction1 = new ListingTransaction()
            {
                QuantitySold = 4,
                UnitPrice = 8
            };
            var transaction2 = new ListingTransaction()
            {
                QuantitySold = 0,
                UnitPrice = 10
            };
            var transaction3 = new ListingTransaction()
            {
                QuantitySold = 1,
                UnitPrice = 5.01
            };

            var listing1 = new Listing()
            {
                ItemId = 12345678910,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction1 }

            };
            var listing2 = new Listing()
            {
                ItemId = 12345678911,
                InventoryItemId = 2,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction2 }

    };
            var listing3 = new Listing()
            {
                ItemId = 12345678912,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction3 }

        };
            var listing4 = new Listing()
            {
                ItemId = 12345678913,
                InventoryItemId = 1,
                UserId = "1",
                TotalNetFees = 1.00,
                Transactions = new List<ListingTransaction> { transaction2 }
            };

            var listings = new List<Listing>() { listing1, listing2, listing3, listing4 };

            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingsBySoldDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(listings);

          var transRepo = new Mock<IListingTransactionRepository>();

            var inventoryBll = new InventoryBll(listingBll.Object, null, transRepo.Object);
            var expected = 37.01;
            //Act
            var actual = inventoryBll.GetSalesByMonth(2016, 1, "1");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateProfitPerListing_Successful()
        {
            var userId = "1";
            var listingId = 123456789;
            //Arrange 
            var transaction = new ListingTransaction
            {
                QuantitySold = 1,
                UnitPrice = 54.51
            };
            var transactions = new List<ListingTransaction>()
            {
                transaction
            };
            var listing = new Listing()
            {
                ItemId = listingId,
                TotalNetFees = 13.24,
                InventoryItemId = 1,
                UserId = userId,
                Transactions = transactions
            };

            var invItem = new InventoryItem()
            {
                Id = 1,
                UserId = "1",
                Cost = 12.01
            };




            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingById(listingId)).Returns(listing);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById((int)listing.InventoryItemId, userId)).Returns(invItem);
            var transRepo = new Mock<IListingTransactionRepository>();
            transRepo.Setup(x => x.GetTransactionsForListing(It.IsAny<long>())).Returns(transactions);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);

            //Act
            var actual = inventoryBll.CalculateProfitPerListing(listing, userId);
            var expected = Math.Round(54.51 - 13.24 - 12.01,2);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CalculateProfitPerListing_NegativeProfit_Successful()
        {
            var userId = "1";
            var listingId = 123456789;
            //Arrange 
            var transaction = new ListingTransaction()
            {
                QuantitySold = 1,
                UnitPrice = 54.51
            };
            var transactions = new List<ListingTransaction>()
            {
                transaction
            };
            var listing = new Listing()
            {
                ItemId = listingId,
                TotalNetFees = 13.24,
                InventoryItemId = 1,
                UserId = userId,
                Transactions = transactions
            };

            var invItem = new InventoryItem()
            {
                Id = 1,
                UserId = "1",
                Cost = 112.01
            };



            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingById(listingId)).Returns(listing);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById((int)listing.InventoryItemId, userId)).Returns(invItem);

            var transRepo = new Mock<IListingTransactionRepository>();
            transRepo.Setup(x => x.GetTransactionsForListing(It.IsAny<long>())).Returns(transactions);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);
            //Act
            var actual = inventoryBll.CalculateProfitPerListing(listing, userId);
            var expected = Math.Round(54.51 - 13.24 - 112.01, 2);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CalculateProfitPerListing_NoAssociatedInvItem_Successful()
        {
            var userId = "1";
            var listingId = 123456789;
            //Arrange 
            var transaction = new ListingTransaction()
            {
                QuantitySold = 1,
                UnitPrice = 54.51
            };
            var transactions = new List<ListingTransaction>()
            {
                transaction
            };
            var listing = new Listing()
            {
                ItemId = listingId,
                CurrentPrice = 54.51,
                TotalNetFees = 13.24,
                InventoryItemId = 1,
                QuantitySold = 1,
                UserId = userId,
                Transactions = transactions
            };



            var invItem = new InventoryItem();
           
            var listingBll = new Mock<IListingBll>();
            listingBll.Setup(x => x.GetListingById(listingId)).Returns(listing);

            var inventoryRepo = new Mock<IInventoryRepository>();
            inventoryRepo.Setup(x => x.GetInventoryItemById((int)listing.InventoryItemId, userId)).Returns(invItem);

            var transRepo = new Mock<IListingTransactionRepository>();
            transRepo.Setup(x => x.GetTransactionsForListing(It.IsAny<long>())).Returns(transactions);

            var inventoryBll = new InventoryBll(listingBll.Object, inventoryRepo.Object, transRepo.Object);

            //Act
            var actual = inventoryBll.CalculateProfitPerListing(listing, userId);
            var expected = Math.Round(54.51 - 13.24, 2);

            //Assert
            Assert.AreEqual(expected, actual);
        }

    }    
}
