using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;

namespace EbaySalesTracker.Repository
{
    public interface IListingRepository
    {
        //gets from ebay
        //Listing GetListingByItemIdFromEbay(long itemId);
        Listing GetListingByItemIdFromEbay(long itemId, string userId);
        List<Listing> GetListingsByEndDateFromEbay(DateTime EndDateFrom, DateTime EndDateTo, string userId);
        List<Listing> GetAllListingsSinceDateFromEbay(DateTime startDate,string userId);
        List<Listing> GetListingsByStartDateFromEbay(DateTime startDateFrom, DateTime startDateTo, string userId);
        void UpdateFeesById(long itemId,string userId);
        void UpdateListings(DateTime sinceDate, string userId);

        //gets from the db
        IEnumerable<Listing> GetAllListingsByUser(int top, int skip,string userId);
        Listing GetListingById(long id);
        Listing AddListing(Listing listing);
        void DeleteListing(long id);      
        Listing UpdateListing(Listing listing);
        IEnumerable<Listing> GetSoldListingsByInventoryItem(int inventoryItemId, string userId);
        IEnumerable<Listing> GetListingsByEndDate(DateTime startDate, DateTime endDate, string userId);
        int GetListingsCountByUser(string userId);
        DateTime? GetLastListingsUpdate(string userId);
        IEnumerable<Listing> GetListingsBySoldDate(DateTime startDate, DateTime endDate, string userId);
    }
}
