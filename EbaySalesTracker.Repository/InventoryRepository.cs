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
                    item.AverageSalesPrice = CalculateAverageSalesPrice(item.Id);
                    item.AverageProfit = Math.Round(Convert.ToDouble(item.AverageSalesPrice - item.Cost),2);

                    item.AverageSalesPrice = Math.Round(Convert.ToDouble(item.AverageSalesPrice), 2);
                    //item.AverageProfit = Math.Round(Convert.ToDouble(item.AverageProfit), 2);

                }


            }
                return items;
        }
        public InventoryItem GetInventoryItemById(int id)
        {
            InventoryItem item = new InventoryItem();
            item = DataContext.InventoryItems.Find(id);
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

        public double CalculateAverageSalesPrice(int id)
        {
            double avgPrice = 0;
            List<Listing> listings = DataContext.Listings.Where(x => x.InventoryItemId == id).ToList();
            foreach(var listing in listings)
            {
                avgPrice += listing.CurrentPrice;
            }

            avgPrice = avgPrice / listings.Count();

            return avgPrice;
        }
    }
}
