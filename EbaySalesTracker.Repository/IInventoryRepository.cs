using EbaySalesTracker.Models;
using System.Collections.Generic;

namespace EbaySalesTracker.Repository
{
    public interface IInventoryRepository
    {
        IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId);
        InventoryItem GetInventoryItemById(int id,string userId);
        void CreateInventoryItem(InventoryItem item);
        InventoryItem EditInventoryItem(InventoryItem item);
        void DeleteInventoryItem(int id);
        object CalculateItemProfitByMonth(int id,string userId);

    }
}
