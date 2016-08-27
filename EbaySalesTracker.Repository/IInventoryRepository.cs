using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public interface IInventoryRepository
    {
        IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId);
        InventoryItem GetInventoryItemById(int id,string userId);
        void CreateInventoryItem(InventoryItem item);
        InventoryItem EditInventoryItem(InventoryItem item);
        void DeleteInventoryItem(int id);
        double[] CalculateAverageSalesPriceAndProfit(int id,string userId);
        int CalculateQuantitySold(int id, string userId);
        object CalculateItemProfitByMonth(int id,string userId);
        InventoryItem GetBestSellingItem(string userId);
        InventoryItem GetHighestAverageProfitItem(string userId);
    }
}
