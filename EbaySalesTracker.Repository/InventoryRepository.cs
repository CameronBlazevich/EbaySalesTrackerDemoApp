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
        public InventoryRepository() { }

        public InventoryRepository(IListingRepository listingRepo)
        {           
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
        }
        public IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId)
        {
            List<InventoryItem> items = new List<InventoryItem>();
            
            using (DataContext)
            {
                items = DataContext.InventoryItems.Where(u => u.UserId == userId).ToList();
            }
                return items;
        }
        public InventoryItem GetInventoryItemById(int id,string userId)
        {          
            return DataContext.InventoryItems.Where(x=>x.UserId == userId && x.Id == id).FirstOrDefault();                                    
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

        public void DeleteInventoryItem(int id, string userId)
        {
            
            InventoryItem item = DataContext.InventoryItems.Where(i => i.Id == id && i.UserId == userId).FirstOrDefault();
            if (item == null)
                return;
            if (!InventoryItemIsAssociatedToListing(id))
            {
                DataContext.InventoryItems.Remove(item);
                DataContext.SaveChanges();
            }
            
        }

        private bool InventoryItemIsAssociatedToListing(int id)
        {
            return DataContext.Listings.Where(x => x.InventoryItemId == id).Any();
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
    }
}
