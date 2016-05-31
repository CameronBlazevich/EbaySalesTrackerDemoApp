using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EbaySalesTracker.Repository.Helpers
{

    public class ListingEngine : Engine
    {        
        public Listing GetListingByItemIdFromEbay(long itemId, string userToken)
        {
            Listing listing = new Listing();
            var context = RequestBuilder.CreateNewApiCall(userToken);

            GetItemCall itemCall = new GetItemCall(context);
            itemCall.ItemID = itemId.ToString();
            itemCall.Execute();
            var res = itemCall.ApiResponse.Item;

            listing = MapResultToListing(res);            
            return listing;
        }

        public List<Listing> GetAllListingsSinceDateFromEbay(DateTime startDate, string userToken)
        {
            List<Listing> listings = new List<Listing>();
            DateTime startDateFrom = startDate;
            DateTime startDateTo = startDate.AddDays(115);

            for (DateTime start = startDateFrom; start <= DateTime.Now; start = start.AddDays(115))
            {               
                listings = listings.Concat(GetListingsByStartDateFromEbay(start, startDateTo, userToken)).ToList();
                startDateTo = startDateTo.AddDays(115);
            }

            return listings;
        }

        public List<Listing> GetListingsByStartDateFromEbay(DateTime startDateFrom, DateTime startDateTo, string userToken)
        {
            List <Listing> listings = new List<Listing>();
            var context = RequestBuilder.CreateNewApiCall(userToken);
            GetSellerListCall getSellerListCall = new GetSellerListCall(context);
            getSellerListCall.Pagination = new PaginationType();
            getSellerListCall.Pagination.EntriesPerPage = 200;
            getSellerListCall.Pagination.PageNumber = 1;
            getSellerListCall.GranularityLevel = GranularityLevelCodeType.Medium;
            getSellerListCall.StartTimeFrom = startDateFrom;
            getSellerListCall.StartTimeTo = startDateTo;
            getSellerListCall.Execute();

            var results = getSellerListCall.ApiResponse.ItemArray;

            foreach (ItemType result in results)
            {
                Listing listing = new Listing();
                listing = MapResultToListing(result);

                listings.Add(listing);
            }
            return listings;
        }

        public List<Listing> GetListingsByEndDateFromEbay(DateTime endDateFrom, DateTime endDateTo, string userToken)
        {

            List<Listing> listings = new List<Listing>();
            var context = RequestBuilder.CreateNewApiCall(userToken);
            GetSellerListCall getSellerListCall = new GetSellerListCall(context);
            getSellerListCall.Pagination = new PaginationType();
            getSellerListCall.Pagination.EntriesPerPage = 200;
            getSellerListCall.Pagination.PageNumber = 1;
            getSellerListCall.GranularityLevel = GranularityLevelCodeType.Medium;
            getSellerListCall.EndTimeFrom = endDateFrom;
            getSellerListCall.EndTimeTo = endDateTo;
            getSellerListCall.Execute();

            var results = getSellerListCall.ApiResponse.ItemArray;

            foreach (ItemType result in results)
            {
                Listing listing = new Listing();
                listing = MapResultToListing(result);
                listings.Add(listing);
            }
            return listings;
        }

        #region Helpers   

        private Listing MapResultToListing(ItemType res)
        {
            var listing = new Listing();
            listing.StartDate = res.ListingDetails.StartTime;
            listing.CurrentPrice = res.SellingStatus.CurrentPrice.Value;
            listing.ItemId = Convert.ToInt64(res.ItemID);
            listing.StartDate = res.ListingDetails.StartTime;
            listing.EndDate = res.ListingDetails.EndTime;
            listing.Title = res.Title;
            listing.QuantitySold = res.Quantity;
            listing.ListingStatus = res.SellingStatus.ListingStatus;
            return listing;
        }




        #endregion

    }

}

