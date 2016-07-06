using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public class InventoryRepository:RepositoryBase<EbaySalesTrackerContext>, IInventoryRepository
    {
        public List<InventoryItem> GetInventoryItemsByUser(string userId)
        {
            List<InventoryItem> items = new List<InventoryItem>();
            
            using (DataContext)
            {
                items = DataContext.InventoryItems.Where(u => u.UserId == userId).ToList();
                foreach(var item in items)
                {                  
                    item.QuantitySold = CalculateQuantitySold(item.Id);
                    SetInventoryItemAverages(item);
                }
            }
                return items.OrderByDescending(x=>x.QuantitySold).ToList();
        }
        public InventoryItem GetInventoryItemById(int id)
        {          
            InventoryItem item = new InventoryItem();
            item = DataContext.InventoryItems.Find(id);            
            item.QuantitySold = CalculateQuantitySold(item.Id);
            SetInventoryItemAverages(item);
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

        public object CalculateItemProfitByMonth(int id)
        {
            //List<Listing> listings = DataContext.Listings.Where(x => x.InventoryItemId == id && x.QuantitySold == 1).ToList();
            double cost = DataContext.InventoryItems.Where(x => x.Id == id).Select(x => x.Cost).FirstOrDefault();

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

        public int CalculateQuantitySold(int id)
        {
            int qtySold = 0;
            List<Listing> listings = DataContext.Listings.Where(x => x.InventoryItemId == id &&  x.QuantitySold == 1).ToList();
            qtySold = listings.Count();         
            return qtySold;
        }
        private InventoryItem SetInventoryItemAverages(InventoryItem item)
        {
            double[] averages = new double[2];
            averages = CalculateAverageSalesPriceAndProfit(item.Id);
            item.AverageProfit = item.QuantitySold == 0 ? 0 : Math.Round(averages[1] - item.Cost, 2);
            item.AverageSalesPrice = Math.Round(averages[0], 2);
            return item;
        }
        public double[] CalculateAverageSalesPriceAndProfit(int id)
        {
            //{average price, average profit}
            double[] avgsArray = new double[2];
            double avgPrice = 0;
            double avgProfitBeforeCost = 0;
            double totalFees = 0;
            double avgTotalFees = 0;
            List<Listing> listings = GetListings(id);
            
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

        public virtual List<Listing> GetListings(int id)
        {
            List<Listing> listings = DataContext.Listings.Where(x => x.InventoryItemId == id && x.TotalNetFees != 0 && x.QuantitySold > 0).ToList();
            return listings;
        }
    }
}
