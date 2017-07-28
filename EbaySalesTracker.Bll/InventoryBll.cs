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
        private IListingBll _listingBll;
        private IInventoryRepository _inventoryRepository;
        private IListingTransactionRepository _transRepo;

        public IListingBll ListingBll
        {
            get
            {
                return _listingBll ?? new ListingBll();
            }
            private set
            {
                _listingBll = value;
            }
        }
        public IInventoryRepository InventoryRepository
        {
            get
            {
                return _inventoryRepository ?? ModelContainer.Instance.Resolve<IInventoryRepository>();
            }
            private set
            {
                _inventoryRepository = value;
            }
        }
        public IListingTransactionRepository TransRepo
        {
            get
            {
                return _transRepo ?? ModelContainer.Instance.Resolve<IListingTransactionRepository>();
            }
            private set
            {
                _transRepo = value;
            }
        }
        public InventoryBll()
        {

        }
        public InventoryBll(IListingBll listingBll,IInventoryRepository inventoryRepo, IListingTransactionRepository transRepo)
        {
            ListingBll = listingBll;
            InventoryRepository = inventoryRepo;
            TransRepo = transRepo;
        }

        public InventoryItem GetInventoryItemById(int inventoryItemId, string userId)
        {
            var inventoryItem = InventoryRepository.GetInventoryItemById(inventoryItemId, userId);
            if (inventoryItem != null)
            {
                var listings = ListingBll.GetSoldListingsByInventoryItem(inventoryItem.Id, userId);
                inventoryItem.QuantitySold = listings.Count();
                SetInventoryItemAverages(inventoryItem, userId);
            }
            return inventoryItem;
        }

        public IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId)
        {
            var inventoryItems = InventoryRepository.GetInventoryItemsByUser(userId);
            foreach (var inventoryItem in inventoryItems)
            {
                    SetInventoryItemAverages(inventoryItem, userId);               
            }
            return inventoryItems.OrderByDescending(x => x.QuantitySold).ToList(); ;
        }

        public IEnumerable<Listing> GetListingsByInventoryItem(int inventoryItemId, string userId)
        {
            var listings = ListingBll.GetSoldListingsByInventoryItem(inventoryItemId, userId);
            foreach (var listing in listings)
            {
                listing.Profit = CalculateProfitPerListing(listing, userId);
            }
            return listings;
        }

        //listings should always only be listings associated to the inventory item being passed in
        public void SetInventoryItemAverages(InventoryItem inventoryItem, string userId)
        {           
            double avgPrice = 0;
            double avgPriceNumerator = 0;
            double avgProfitBeforeCost = 0;
            double totalFees = 0;
            double avgTotalFees = 0;

            var listings = ListingBll.GetSoldListingsByInventoryItem(inventoryItem.Id, userId);
            if (listings.Count() == 0)
            {
                inventoryItem.QuantitySold = 0;
                inventoryItem.AverageProfit = 0;
                inventoryItem.AverageSalesPrice = 0;

            }
            else
            {
                inventoryItem.QuantitySold = listings.Sum(l => l.CalculateQuantitySold());

                foreach (var listing in listings)
                {
                    foreach (var trans in listing.Transactions)
                    {
                        avgPriceNumerator += trans.QuantitySold * trans.UnitPrice;
                    }
                    totalFees += listing.TotalNetFees;
                }

                avgTotalFees = totalFees / inventoryItem.QuantitySold;
                avgPrice = avgPriceNumerator / inventoryItem.QuantitySold;
                avgProfitBeforeCost = avgPrice - avgTotalFees;

                inventoryItem.AverageProfit = Math.Round(avgProfitBeforeCost - inventoryItem.Cost, 2);
                inventoryItem.AverageSalesPrice = Math.Round(avgPrice, 2);
            }
        }

        public InventoryItem GetBestSellingItem(string userId)
        {
            var items = GetInventoryItemsByUser(userId);
            var item = new InventoryItem();
            if(items.Any())
                item = items.Aggregate((curMax, x) => (curMax == null || x.QuantitySold > curMax.QuantitySold ? x : curMax));

            return item;
        }
        public InventoryItem GetHighestAverageProfitItem(string userId)
        {
            var items = GetInventoryItemsByUser(userId);
            var item = new InventoryItem();
            if (items.Any())
                item = items.Aggregate((curMax, x) => (curMax == null || x.AverageProfit > curMax.AverageProfit ? x : curMax));

            return item;
        }

        public IEnumerable<Listing> GetListingsByUser(int top, int skip, string userId)
        {
            var listings = ListingBll.GetListingsByUser(top, skip, userId);
            foreach (var listing in listings)
            {
                SetProfitPerListing(listing, userId);
            }
            return listings;
        }

        

        public object GetListingDataByInventoryItem(int inventoryItemId, string userId)
        {
            var listings = ListingBll.GetSoldListingsByInventoryItem(inventoryItemId, userId);
            var data = listings
                .Select(x => new { profit = x.Profit, endDate = x.EndDate });
            return data;
        }

        public void SetProfitPerListing(Listing listing, string userId)
        {
            if (listing.InventoryItemId != null)
            {
                listing.InventoryItem = InventoryRepository.GetInventoryItemById((int)listing.InventoryItemId, userId);
            }

            if (listing.QuantitySold > 0)
            {               
                double inventoryItemCost = listing.InventoryItem?.Cost ?? 0;
                listing.Profit = CalculateProfitPerListing(listing, userId);
            }
        }

        public Listing AssociateInventoryItemToListing(long listingId, int? inventoryItemId, string userId)
        {
            var listing = ListingBll.GetListingById(listingId);
            listing.InventoryItemId = inventoryItemId;
            listing.Profit = CalculateProfitPerListing(listing, userId);
            return ListingBll.UpdateListing(listing);
            
        }
        public double GetSalesByMonth(int year, int month, string userId)
        {
            var firstDayOfMonth = new DateTime(year, month, 01);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var listings = ListingBll.GetListingsBySoldDate(firstDayOfMonth, lastDayOfMonth, userId);
            return CalculateSales(listings);
        }

        public double GetSalesByYear(int year, string userId)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            var listings = ListingBll.GetListingsBySoldDate(firstDayOfYear, lastDayOfYear, userId);
            return CalculateSales(listings);
        }

        public double GetSalesLastSevenDays(string userId)
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var sevenDaysAgo = today.AddDays(-7);

            var listings = ListingBll.GetListingsBySoldDate(sevenDaysAgo, today, userId);           
            return CalculateSales(listings);
        }

        private double CalculateSales(IEnumerable<Listing> listings)
        {
            var sales = listings.Sum(x => x.CalculateGrossSales());
            return Math.Round(sales, 2);
        }
        public double GetProfitLastSevenDays(string userId)
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var sevenDaysAgo = today.AddDays(-7);

            var listings = ListingBll.GetListingsBySoldDate(sevenDaysAgo, today, userId);
            return CalculateProfit(listings, userId);
        }
        public double GetProfitByMonth(int year, int month, string userId)
        {
            var firstDayOfMonth = new DateTime(year, month, 01);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var listings = ListingBll.GetListingsBySoldDate(firstDayOfMonth, lastDayOfMonth, userId);

            return CalculateProfit(listings, userId);
        }
        public double GetProfitByYear(int year, string userId)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            var listings = ListingBll.GetListingsBySoldDate(firstDayOfYear, lastDayOfYear, userId);

            return CalculateProfit(listings, userId);
        }
        private double CalculateProfit(IEnumerable<Listing> listings, string userId)
        {
            double totalProfit = 0;
            foreach (var listing in listings)
            {               
                totalProfit += CalculateProfitPerListing(listing, userId);
            }
            return Math.Round(totalProfit, 2);
        }

        public double CalculateProfitPerListing(Listing listing, string userId)
        {           
            double profit = 0;                 
            var grossSales = listing.CalculateGrossSales();
            var qtySold = listing.CalculateQuantitySold();
            
                if (listing.InventoryItemId != null)
                {
                    listing.InventoryItem = InventoryRepository.GetInventoryItemById((int)listing.InventoryItemId, userId);
                }

                profit = Math.Round(grossSales - listing.TotalNetFees - (listing.InventoryItem?.Cost ?? 0) * qtySold, 2);
            
            return profit;
        }
        public double GetFeesLastSevenDays(string userId)
        {
            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var sevenDaysAgo = today.AddDays(-7);

            var listings = ListingBll.GetListingsBySoldDate(sevenDaysAgo, today, userId);

            return CalculateFees(listings);
        }

       
        public double GetFeesByMonth(int year, int month, string userId)
        {
            var firstDayOfMonth = new DateTime(year, month, 01);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var listings = ListingBll.GetListingsBySoldDate(firstDayOfMonth, lastDayOfMonth, userId);

            return CalculateFees(listings);
        }

        public double GetFeesByYear(int year, string userId)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            var listings = ListingBll.GetListingsBySoldDate(firstDayOfYear, lastDayOfYear, userId);
        
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
