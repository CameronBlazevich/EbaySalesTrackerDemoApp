using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Bll
{
    public interface IListingBll
    {
        void AssociateListingToInventoryItem(long listingId, int inventoryId, string userId);
        IEnumerable<Listing> GetListingsByInventoryItem(string userId, int inventoryItemId);
        void DissociateInventoryItem(long listingId, string userId);
    }
}
