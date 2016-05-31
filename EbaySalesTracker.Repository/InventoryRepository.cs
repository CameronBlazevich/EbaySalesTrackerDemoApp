using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
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
            }
                return items;
        }

    }
}
