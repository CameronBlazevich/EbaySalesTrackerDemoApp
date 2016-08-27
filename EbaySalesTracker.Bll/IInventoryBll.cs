using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Bll
{
    public interface IInventoryBll
    {
        IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId);
        void UpdateQuantitySold(int inventoryItemId, string userId);
    }
}
