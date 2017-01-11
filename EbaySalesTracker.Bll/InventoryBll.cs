using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbaySalesTracker.Bll
{
    public class InventoryBll : IInventoryBll
    {
        private IListingBll _ListingBll;
        private IInventoryRepository _InventoryRepository;
        public InventoryBll():this(null, null) { }
        public InventoryBll(IListingBll listingBll,IInventoryRepository inventoryRepo)
        {
            _ListingBll = listingBll ?? new ListingBll();
            _InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>(); 
        }

        public InventoryItem GetInventoryItemById(int inventoryItemId, string userId)
        {
            var inventoryItem = _InventoryRepository.GetInventoryItemById(inventoryItemId, userId);
            if (inventoryItem != null)
            {
                var listings = _ListingBll.GetSoldListingsByInventoryItem(inventoryItem.Id, userId);
                inventoryItem.QuantitySold = listings.Count();
                SetInventoryItemAverages(listings, inventoryItem, userId);
            }
            return inventoryItem;
        }

        public IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId)
        {
            var inventoryItems = _InventoryRepository.GetInventoryItemsByUser(userId);
            foreach (var inventoryItem in inventoryItems)
            {
                var listings = _ListingBll.GetSoldListingsByInventoryItem(inventoryItem.Id, userId);
                if (listings.Count() == 0)
                {
                    inventoryItem.QuantitySold = 0;
                    inventoryItem.AverageProfit = 0;
                    inventoryItem.AverageSalesPrice = 0;
                }
                else {
                    inventoryItem.QuantitySold = listings.Count();
                    SetInventoryItemAverages(listings, inventoryItem, userId);
                }
            }
            return inventoryItems.OrderByDescending(x => x.QuantitySold).ToList(); ;
        }

        public IEnumerable<Listing> GetListingsByInventoryItem(int inventoryItemId, string userId)
        {
            var listings = _ListingBll.GetSoldListingsByInventoryItem(inventoryItemId, userId);
            foreach (var listing in listings)
            {
                listing.Profit = CalculateProfitPerListing(listing.ItemId, userId);
            }
            return listings;
        }

        //listings should always only be listings associated to the inventory item being passed in
        public void SetInventoryItemAverages(IEnumerable<Listing> listings, InventoryItem inventoryItem, string userId)
        {           
            double avgPrice = 0;
            double avgProfitBeforeCost = 0;
            double totalFees = 0;
            double avgTotalFees = 0;
            int listingsCount = listings.Count();

            foreach (var listing in listings)
            {
                avgPrice += listing.CurrentPrice;
                totalFees += listing.TotalNetFees;
            }

            avgTotalFees = totalFees / listingsCount;
            avgPrice = avgPrice / listingsCount;
            avgProfitBeforeCost = avgPrice - avgTotalFees;

            inventoryItem.AverageProfit = Math.Round(avgProfitBeforeCost - inventoryItem.Cost, 2);
            inventoryItem.AverageSalesPrice = Math.Round(avgPrice, 2);
        }

        public int CalculateQuantitySold(int inventoryItemId, string userId)
        {
            var listings = _ListingBll.GetSoldListingsByInventoryItem(inventoryItemId, userId);
            return listings.Count();    
        }

        public InventoryItem GetBestSellingItem(string userId)
        {
            var items = GetInventoryItemsByUser(userId);
            var item = items.Aggregate((curMax, x) => (curMax == null || x.QuantitySold > curMax.QuantitySold ? x : curMax));
            return item;
        }
        public InventoryItem GetHighestAverageProfitItem(string userId)
        {
            var items = GetInventoryItemsByUser(userId);
            var item = items.Aggregate((curMax, x) => (curMax == null || x.AverageProfit > curMax.AverageProfit ? x : curMax));
            return item;
        }

       

        public IEnumerable<Listing> GetListingsByUser(int top, int skip, string userId)
        {
            var listings = _ListingBll.GetListingsByUser(top, skip, userId);
            foreach (var listing in listings)
            {
                SetProfitPerListing(listing, userId);
            }
            return listings;
        }

        

        public object GetListingDataByInventoryItem(int inventoryItemId, string userId)
        {
            var listings = _ListingBll.GetSoldListingsByInventoryItem(inventoryItemId, userId);
            var data = listings
                .Select(x => new { profit = x.Profit, endDate = x.EndDate });
            return data;
        }
        public double CalculateProfitPerListing(long listingId, string userId)
        {
            double profit = 0;
            var inventoryItem = new InventoryItem();
            var listing = _ListingBll.GetListingById(listingId);
            if (listing.QuantitySold > 0)
            {
                if (listing.InventoryItemId != null)
                {
                    inventoryItem = _InventoryRepository.GetInventoryItemById((int)listing.InventoryItemId, userId);
                }

                profit = Math.Round(listing.CurrentPrice - listing.TotalNetFees - inventoryItem.Cost, 2);
            }
            return profit;
        }
        public void SetProfitPerListing(Listing listing, string userId)
        {
            if (listing.InventoryItemId != null)
            {
                listing.InventoryItem = _InventoryRepository.GetInventoryItemById((int)listing.InventoryItemId, userId);
            }

            if (listing.QuantitySold > 0)
            {               
                double inventoryItemCost = listing.InventoryItem?.Cost ?? 0;

                listing.Profit = Math.Round(listing.CurrentPrice - listing.TotalNetFees - inventoryItemCost, 2);
            }
        }

        public void AssociateInventoryItemToListing(long listingId, int inventoryItemId, string userId)
        {
            var listing = _ListingBll.GetListingById(listingId);
            listing.InventoryItemId = inventoryItemId;
            listing.Profit = CalculateProfitPerListing(listingId, userId);
            _ListingBll.UpdateListing(listing);

        }
        public double GetSalesByMonth(int year, int month, string userId)
        {
            var firstDayOfMonth = new DateTime(year, month, 01);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var listings = _ListingBll.GetListingsByEndDate(firstDayOfMonth, lastDayOfMonth, userId);

            return CalculateSales(listings);
        }

        public double GetSalesByYear(int year, string userId)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            var listings = _ListingBll.GetListingsByEndDate(firstDayOfYear, lastDayOfYear, userId);

            return CalculateSales(listings);
        }

        public double GetSalesLastSevenDays(string userId)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var sevenDaysAgo = today.AddDays(-7);

            var listings = _ListingBll.GetListingsByEndDate(today, sevenDaysAgo, userId);

            return CalculateSales(listings);
        }

        private double CalculateSales(IEnumerable<Listing> listings)
        {
            double totalSales = 0;

            foreach (var listing in listings)
            {
                double sellingPrice = 0;
                if (listing.QuantitySold > 0)
                {
                    sellingPrice = listing.CurrentPrice;
                }
                totalSales += sellingPrice;
            }
            return Math.Round(totalSales, 2);
        }
        public double GetProfitLastSevenDays(string userId)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var sevenDaysAgo = today.AddDays(-7);

            var listings = _ListingBll.GetListingsByEndDate(today, sevenDaysAgo, userId);
            return CalculateProfit(listings, userId);
        }
        public double GetProfitByMonth(int year, int month, string userId)
        {
            var firstDayOfMonth = new DateTime(year, month, 01);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var listings = _ListingBll.GetListingsByEndDate(firstDayOfMonth, lastDayOfMonth, userId);

            return CalculateProfit(listings, userId);
        }
        public double GetProfitByYear(int year, string userId)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            var listings = _ListingBll.GetListingsByEndDate(firstDayOfYear, lastDayOfYear, userId);

            return CalculateProfit(listings, userId);
        }
        private double CalculateProfit(IEnumerable<Listing> listings, string userId)
        {
            double totalProfit = 0;
            foreach (var listing in listings)
            {
                var inventoryItem = new InventoryItem();
                double profit = 0;
                if (listing.InventoryItemId != null)
                {
                    inventoryItem = _InventoryRepository.GetInventoryItemById((int)listing.InventoryItemId, userId);
                }
                if (listing.QuantitySold > 0)
                {
                    profit = listing.CurrentPrice - listing.TotalNetFees - inventoryItem.Cost;
                }
                totalProfit += profit;
            }
            return Math.Round(totalProfit, 2);
        }

        public double GetFeesLastSevenDays(string userId)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var sevenDaysAgo = today.AddDays(-7);

            var listings = _ListingBll.GetListingsByEndDate(today, sevenDaysAgo, userId);

            return CalculateFees(listings);
        }

       
        public double GetFeesByMonth(int year, int month, string userId)
        {
            var firstDayOfMonth = new DateTime(year, month, 01);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var listings = _ListingBll.GetListingsByEndDate(firstDayOfMonth, lastDayOfMonth, userId);

            return CalculateFees(listings);
        }

        public double GetFeesByYear(int year, string userId)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            var listings = _ListingBll.GetListingsByEndDate(firstDayOfYear, lastDayOfYear, userId);

            return CalculateFees(listings);
        }

        private double CalculateFees(IEnumerable<Listing> listings)
        {
            //not sure if I should care about quantity sold > 0
            double totalFees = 0;

            foreach (var listing in listings)
            {
                double fees = 0;
                if (listing.QuantitySold > 0)
                {
                    fees = listing.TotalNetFees;
                }
                totalFees += fees;
            }
            return Math.Round(totalFees, 2);
        }
    }
}
