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

        //gets from the db
        List<Listing> GetAllListingsByUser(string userId);
        Listing GetListingById(long id);
        Listing AddListing(Listing listing);
        void DeleteListing(long id);
        void AssociateInventoryItem(long listingId, int inventoryItemId);
        void DissociateInventoryItem(long listingId);
        double CalculateProfit(long listingId);
        void UpdateProfit(long listingId);
        List<Listing> GetListingsByInventoryItem(string userId, int inventoryItemId);
        object GetListingDataByInventoryItem(string userId, int inventoryItemId);
    }
}
