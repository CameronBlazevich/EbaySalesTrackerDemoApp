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
        List<InventoryItem> GetInventoryItemsByUser(string userId);
        InventoryItem GetInventoryItemById(int id);
        void CreateInventoryItem(InventoryItem item);
        InventoryItem EditInventoryItem(InventoryItem item);
        void DeleteInventoryItem(int id);
        double[] CalculateAverageSalesPriceAndProfit(int id);
        int CalculateQuantitySold(int id);
        object CalculateItemProfitByMonth(int id);
    }
}
