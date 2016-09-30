using EbaySalesTracker.Models;
using System.Collections.Generic;

namespace EbaySalesTracker.Bll
{

    public interface IInventoryBll
    {
        InventoryItem GetInventoryItemById(int inventoryItemId, string userId);
        IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId);
        InventoryItem GetBestSellingItem(string userId);
        InventoryItem GetHighestAverageProfitItem(string userId);
        double GetProfitByMonth(int year, int month, string userId);
        double GetSalesByMonth(int year, int month, string userId);
        object GetListingDataByInventoryItem(int inventoryItemId, string userId);
        void AssociateInventoryItemToListing(long listingId, int inventoryItemId, string userId);
        IEnumerable<Listing> GetListingsByUser(int top, int skip,string userId);

        //should probably refactor listings controller so that I don't need this calculate method exposed 
        double CalculateProfitPerListing(long listingId, string userId);
    }
}
