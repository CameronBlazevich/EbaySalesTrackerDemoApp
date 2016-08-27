using EbaySalesTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EbaySalesTracker.Models;

namespace EbaySalesTracker.Bll
{
    public class InventoryBll : IInventoryBll
    {
        IInventoryRepository _InventoryRepository;
        IListingBll _ListingBll;
        public InventoryBll()
        {
        }
        public InventoryBll(IInventoryRepository inventoryRepo, IListingBll listingBll)
        {
            _InventoryRepository = inventoryRepo ?? new InventoryRepository();
            _ListingBll = listingBll ?? new ListingBll();
        }

        public IEnumerable<InventoryItem> GetInventoryItemsByUser(string userId)
        {
            var inventoryItems = _InventoryRepository.GetInventoryItemsByUser(userId);
            foreach(var inventoryItem in inventoryItems)
            {
                inventoryItem.QuantitySold = CalculateQuantitySold(inventoryItem.Id, userId);
            }
            return inventoryItems;
        }

        public int CalculateQuantitySold(int inventoryItemId, string userId)
        {
            int qtySold;

            var listings = _ListingBll.GetListingsByInventoryItem(userId, inventoryItemId).ToList();
            //method above calculates profits, need to refactor this
            qtySold = listings.Count;

            return qtySold;
        }

        public void UpdateQuantitySold(int inventoryItemId, string userId)
        {
            var qtySold = CalculateQuantitySold(inventoryItemId, userId);
           // _InventoryRepository.UpdateQuantitySold(inventoryItemId, qtySold);
        }
    }
}
