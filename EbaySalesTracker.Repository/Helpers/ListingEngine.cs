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
            GetSellerEventsCall getSellerEventsCall = new GetSellerEventsCall(context);

            getSellerEventsCall.StartTimeFrom = startDateFrom;
            getSellerEventsCall.StartTimeTo = startDateTo;
            getSellerEventsCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            getSellerEventsCall.OutputSelector = new string[] { "SellingStatus", "ItemId", "ListingDetails", "Title", "ListingType" };
            getSellerEventsCall.Execute();

            var results = getSellerEventsCall.ApiResponse.ItemArray;

            foreach (ItemType result in results)
            {                
                var listing = MapResultToListing(result);

                listings.Add(listing);
            }
            return listings;
        }

        public List<Listing> GetListingsByEndDateFromEbay(DateTime endDateFrom, DateTime endDateTo, string userToken)
        {

            List<Listing> listings = new List<Listing>();
            var context = RequestBuilder.CreateNewApiCall(userToken);
            GetSellerEventsCall getSellerEventsCall = new GetSellerEventsCall(context);
            getSellerEventsCall.EndTimeFrom = endDateFrom;
            getSellerEventsCall.EndTimeTo = endDateTo;
            getSellerEventsCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            getSellerEventsCall.Execute();

            var results = getSellerEventsCall.ApiResponse.ItemArray;

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
            listing.CurrentPrice = res.SellingStatus.CurrentPrice.Value;
            listing.ItemId = Convert.ToInt64(res.ItemID);
            listing.StartDate = res.ListingDetails.StartTime;
            listing.EndDate = res.ListingDetails.EndTime;
            listing.Title = res.Title;
            listing.QuantitySold = res.SellingStatus.QuantitySold;
            listing.ListingStatus = res.SellingStatus.ListingStatus;
            listing.Type = res.ListingType;
            return listing;
        }

        #endregion

        #region sandboxMethods
        public List<Listing> GetSandboxListingsByEndDateFromEbay(DateTime endDateFrom, DateTime endDateTo, string userToken)
        {

            List<Listing> listings = new List<Listing>();
            var context = RequestBuilder.CreateNewSandboxApiCall();
            GetSellerEventsCall getSellerEventsCall = new GetSellerEventsCall(context);
            getSellerEventsCall.EndTimeFrom = endDateFrom;
            getSellerEventsCall.EndTimeTo = endDateTo;
            getSellerEventsCall.Execute();

            var results = getSellerEventsCall.ApiResponse.ItemArray;

            foreach (ItemType result in results)
            {
                Listing listing = new Listing();
                listing = MapResultToListing(result);
                listings.Add(listing);
            }
            return listings;
        }


        #endregion

    }

}

