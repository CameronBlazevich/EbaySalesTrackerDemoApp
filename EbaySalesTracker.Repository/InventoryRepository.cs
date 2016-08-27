using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.Unity;


namespace EbaySalesTracker.Repository
{
    public class InventoryRepository:RepositoryBase<EbaySalesTrackerContext>, IInventoryRepository
    {
        IListingRepository _ListingRepository;
        public InventoryRepository() 
        {

        }

        public InventoryRepository(IListingRepository listingRepo)
        {
            
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            //_ListingDetailRepository = listingDetailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
           // _UserRepository = userRepo ?? ModelContainer.Instance.Resolve<IUserRepository>();
            //_InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>();

          
        }
        public IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId)
        {
            List<InventoryItem> items = new List<InventoryItem>();
            
            using (DataContext)
            {
                items = DataContext.InventoryItems.Where(u => u.UserId == userId).ToList();
                foreach(var item in items)
                {                  
                    item.QuantitySold = CalculateQuantitySold(item.Id,userId);
                    SetInventoryItemAverages(item,userId);
                }
            }
                return items.OrderByDescending(x=>x.QuantitySold).ToList();
        }
        public InventoryItem GetInventoryItemById(int id,string userId)
        {          
            InventoryItem item = new InventoryItem();
            item = DataContext.InventoryItems.Where(x=>x.UserId == userId && x.Id == id).FirstOrDefault();            
            item.QuantitySold = CalculateQuantitySold(item.Id,userId);
            SetInventoryItemAverages(item,userId);
            return item;
        }

        public void CreateInventoryItem(InventoryItem item)
        {
            DataContext.InventoryItems.Add(item);
            DataContext.SaveChanges();
        }
        public InventoryItem EditInventoryItem(InventoryItem item)
        {
            DataContext.Entry(item).State = EntityState.Modified;
            DataContext.SaveChanges();            
            return item;
        }

        public void DeleteInventoryItem(int id)
        {
            InventoryItem item = DataContext.InventoryItems.Find(id);
            if (item == null)
                return;

            DataContext.InventoryItems.Remove(item);
            DataContext.SaveChanges();
        }

        public object CalculateItemProfitByMonth(int id, string userId)
        {
            //List<Listing> listings = DataContext.Listings.Where(x => x.InventoryItemId == id && x.QuantitySold == 1).ToList();
            double cost = DataContext.InventoryItems.Where(x => x.Id == id && x.UserId == userId).Select(x => x.Cost).FirstOrDefault();
            
            var result = from listing in DataContext.Listings
                         //join details in DataContext.ListingDetails on listing.ItemId equals details.ItemId
                         //join items in DataContext.InventoryItems on listing.InventoryItemId equals items.Id
                         where (listing.InventoryItemId == id && listing.QuantitySold == 1 && listing.TotalNetFees > 0)
                         group listing by listing.EndDate.Month
                         into g
                         select new {Id = id, Cost = cost, Month = g.Key, QtySold = g.Sum(s => s.QuantitySold), TotalSales = g.Sum(s => s.CurrentPrice), Fees = g.Sum(s => s.TotalNetFees)};
            // TotalProfit = (g.Sum(s => s.CurrentPrice)- g.Sum(s => s.QuantitySold)*cost- g.Sum(s => s.TotalNetFees)),
            return result;
        }

        public int CalculateQuantitySold(int id, string userId)
        {
            int qtySold = 0;
            //var listingRepo = new IListingRepository();
            var listings = _ListingRepository.GetListingsByInventoryItem(userId,id);
            //List<Listing> listings = DataContext.Listings.Where(x => x.InventoryItemId == id &&  x.QuantitySold == 1).ToList();
            qtySold = listings.Count();         
            return qtySold;
        }
        private InventoryItem SetInventoryItemAverages(InventoryItem item,string userId)
        {
            double[] averages = new double[2];
            averages = CalculateAverageSalesPriceAndProfit(item.Id,userId);
            item.AverageProfit = item.QuantitySold == 0 ? 0 : Math.Round(averages[1] - item.Cost, 2);
            item.AverageSalesPrice = Math.Round(averages[0], 2);
            return item;
        }
        public double[] CalculateAverageSalesPriceAndProfit(int id, string userId)
        {
            //{average price, average profit}
            double[] avgsArray = new double[2];
            double avgPrice = 0;
            double avgProfitBeforeCost = 0;
            double totalFees = 0;
            double avgTotalFees = 0;

            //List<Listing> listings = GetListings(id);
            var listings = _ListingRepository.GetListingsByInventoryItem(userId, id);
            
            if (listings.Count() == 0)
                return avgsArray;

            foreach (var listing in listings)
            {
                avgPrice += listing.CurrentPrice;
                totalFees += listing.TotalNetFees;
            }

            avgTotalFees = totalFees / listings.Count();

            avgPrice = avgPrice / listings.Count();
            avgProfitBeforeCost = avgPrice - avgTotalFees;

            avgsArray[0] = avgPrice;
            avgsArray[1] = avgProfitBeforeCost;
            return avgsArray;
        }
        public InventoryItem GetBestSellingItem(string userId)
        {
            //var item = new InventoryItem();
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
    }
}
