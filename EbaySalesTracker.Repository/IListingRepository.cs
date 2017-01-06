using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;

namespace EbaySalesTracker.Repository
{
    public interface IListingRepository
    {
        //gets from ebay
        //Listing GetListingByItemIdFromEbay(long itemId);
        Listing GetListingByItemIdFromEbay(long itemId, string userToken);
        List<Listing> GetListingsByEndDateFromEbay(DateTime EndDateFrom, DateTime EndDateTo, string userToken);
        List<Listing> GetAllListingsSinceDateFromEbay(DateTime startDate,string userId);
        List<Listing> GetListingsByStartDateFromEbay(DateTime startDateFrom, DateTime startDateTo, string userId, string userToken);
        void UpdateFeesById(long itemId,string userToken);
        void UpdateListings(DateTime sinceDate, string userId);

        //gets from the db
        IEnumerable<Listing> GetAllListingsByUser(int top, int skip,string userId);
        Listing GetListingById(long id);
        Listing AddListing(Listing listing);
        void DeleteListing(long id);
        void AssociateInventoryItem(long listingId, int inventoryItemId);
        void DissociateInventoryItem(long listingId);
        void UpdateListing(Listing listing);
        IEnumerable<Listing> GetSoldListingsByInventoryItem(int inventoryItemId, string userId);
        IEnumerable<Listing> GetListingsByEndDate(DateTime startDate, DateTime endDate, string userId);
        int GetListingsCountByUser(string userId);
        DateTime? GetLastListingsUpdate(string userId);

        // List<Listing> GetListingsByInventoryItem(int inventoryItemId);
        //object GetListingDataByInventoryItem(string userId, int inventoryItemId);       
    }
}
