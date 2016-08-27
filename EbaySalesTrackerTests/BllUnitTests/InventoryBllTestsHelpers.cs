using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTrackerTests.BllUnitTests
{
    public class InventoryBllTestsHelpers
    {
        public static List<InventoryItem> CreateListOfInventoryItemsForTesting()
        {
            var items = new List<InventoryItem>();
            var item0 = new InventoryItem
            {
                Id = 0,
                Description = "TestItem 0",
                Cost = 1.23,
                QuantitySold = 4,
                Quantity = 2,
                UserId = "2",
                AverageSalesPrice = null,
                AverageProfit = null

            };
            var item1 = new InventoryItem
            {
                Id = 1,
                Description = "TestItem 1",
                Cost = 2.34,
                QuantitySold = 3,
                Quantity = 4,
                UserId = "1",
                AverageSalesPrice = null,
                AverageProfit = null
            };
            var item2 = new InventoryItem
            {
                Id = 2,
                Description = "TestItem 2",
                Cost = 3.45,
                QuantitySold = 2,
                Quantity = 6,
                UserId = "1",
                AverageSalesPrice = null,
                AverageProfit = null
            };
            var item3 = new InventoryItem
            {
                Id = 3,
                Description = "TestItem 3",
                Cost = 4.56,
                QuantitySold = 1,
                Quantity = 0,
                UserId = "1",
                AverageSalesPrice = null,
                AverageProfit = null
            };
            var item4 = new InventoryItem
            {
                Id = 4,
                Description = "TestItem 4",
                Cost = 5.67,
                QuantitySold = 0,
                Quantity = 8,
                UserId = "1",
                AverageSalesPrice = null,
                AverageProfit = null
            };

            items.Add(item0);
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);

            return items;
        }
        public static List<Listing> CreateListingsForTesting()
        {
            var listing = new Listing
            {
                ItemId = 123456789,
                StartDate = new DateTime(2016, 1, 30),
                EndDate = new DateTime(2016, 2, 2),
                Title = "TestTitle",
                CurrentPrice = 5.51,
                QuantitySold = 1,
                //ListingStatus = 
                TotalNetFees = 1.50,
                UserId = "2",
                InventoryItemId = 1
            };
            var listing1 = new Listing
            {
                ItemId = 1234567890,
                StartDate = new DateTime(2016, 1, 30),
                EndDate = new DateTime(2016, 2, 2),
                Title = "TestTitle1",
                CurrentPrice = 64.23,
                QuantitySold = 1,
                //ListingStatus = 
                TotalNetFees = 4.25,
                UserId = "1",
                InventoryItemId = 1
            };
            var listing2 = new Listing
            {
                ItemId = 1234567891,
                StartDate = new DateTime(2016, 1, 30),
                EndDate = new DateTime(2016, 2, 2),
                Title = "TestTitle2",
                CurrentPrice = 5.51,
                QuantitySold = 1,
                //ListingStatus = 
                TotalNetFees = 2.00,
                UserId = "1",
                InventoryItemId = 1
            };
            var listing3 = new Listing
            {
                ItemId = 1234567891,
                StartDate = new DateTime(2016, 1, 30),
                EndDate = new DateTime(2016, 2, 2),
                Title = "TestTitle2",
                CurrentPrice = 5.51,
                QuantitySold = 1,
                //ListingStatus = 
                TotalNetFees = 2.00,
                UserId = "1",
                InventoryItemId = 2
            };
            var listing4 = new Listing
            {
                ItemId = 1234567891,
                StartDate = new DateTime(2016, 1, 30),
                EndDate = new DateTime(2016, 2, 2),
                Title = "TestTitle2",
                CurrentPrice = 5.51,
                QuantitySold = 0,
                //ListingStatus = 
                TotalNetFees = 2.00,
                UserId = "1",
                InventoryItemId = 1
            };

            var listings = new List<Listing>();
            listings.Add(listing);
            listings.Add(listing1);
            listings.Add(listing2);
            listings.Add(listing3);
            listings.Add(listing4);

            return listings;
        }
    }
}
