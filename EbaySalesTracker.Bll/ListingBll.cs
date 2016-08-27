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
    public class ListingBll : IListingBll
    {
        IListingRepository _ListingRepository;
        //IListingRepository _ListingRepository = ModelContainer.Instance.Resolve<IListingRepository>();
        IInventoryBll _InventoryBll;
        public ListingBll()
        {
        }
        public ListingBll(IListingRepository listingRepo, IInventoryBll inventoryBll)
        {
            _ListingRepository = listingRepo ?? new ListingRepository();
            _InventoryBll = inventoryBll ?? new InventoryBll();
        }
        public void AssociateListingToInventoryItem(long listingId, int inventoryItemId, string userId)
        {
            _ListingRepository.AssociateInventoryItem(listingId, inventoryItemId);            
            _InventoryBll.UpdateQuantitySold(inventoryItemId, userId);
        }

        public IEnumerable<Listing> GetListingsByInventoryItem(string userId, int inventoryItemId)
        {
           return  _ListingRepository.GetListingsByInventoryItem(userId, inventoryItemId);
        }

        public void DissociateInventoryItem(long listingId, string userId)
        {
            _ListingRepository.DissociateInventoryItem(listingId);
        }
    }
}
