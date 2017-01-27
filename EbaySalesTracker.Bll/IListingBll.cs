using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;

namespace EbaySalesTracker.Bll
{
    public interface IListingBll
    {
        IEnumerable<Listing> GetSoldListingsByInventoryItem(int inventoryItemId, string userId);
        IEnumerable<Listing> GetListingsByEndDate(DateTime startDate, DateTime endDate, string userId);
        Listing GetListingById(long listingId);
        Listing UpdateListing(Listing listing);
        void UpdateListings(string userId);
        int GetListingsCountByUser(string userId);
        IEnumerable<Listing> GetListingsByUser(int top, int skip, string userId);
        IEnumerable<Listing> GetListingsBySoldDate(DateTime startDate, DateTime endDate, string userId);
    }
}
