using EbaySalesTracker.Models;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System;
using System.Linq;
using eBay.Service.Call;
using eBay.Service.Core.Soap;

namespace EbaySalesTracker.Repository.Helpers
{
    public class ListingDetailEngine:Engine
    {
        

        public List<ListingDetail> GetAllListingDetailsFromEbay(List<long> itemIds, string userToken)
        {
            var listingDetails = new List<ListingDetail>();

            foreach(var id in itemIds)
            {
                listingDetails = listingDetails.Concat(GetListingDetailsByItemIdFromEbay(id,userToken)).ToList();
            }

            return listingDetails;
        }



        public List<ListingDetail> GetListingDetailsByItemIdFromEbay(long itemId, string userToken)
        {
            List <ListingDetail> listingDetails = new List<ListingDetail>();
            var context = RequestBuilder.CreateNewApiCall(userToken);
            GetAccountCall getAccountCall = new GetAccountCall(context);
            getAccountCall.ItemID = itemId.ToString();
            getAccountCall.Execute();

            var accountEntryList = getAccountCall.AccountEntryList;
            if (accountEntryList != null)
            {
                foreach (AccountEntryType accountEntry in accountEntryList)
                {
                    listingDetails.Add(mapResultToListingDetail(accountEntry));
                }
            }

            return listingDetails;
        }

        private ListingDetail mapResultToListingDetail(AccountEntryType accountEntry)
        {
            ListingDetail listingDetail = new ListingDetail();
            listingDetail.ItemId = Convert.ToInt64(accountEntry.ItemID);
            listingDetail.RefNumber = Convert.ToInt64(accountEntry.RefNumber);
            listingDetail.Description = accountEntry.Description;
            listingDetail.GrossAmount = accountEntry.GrossDetailAmount.Value;
            listingDetail.NetAmount = accountEntry.NetDetailAmount.Value;
            listingDetail.PostDate = accountEntry.Date;
            listingDetail.Type = accountEntry.AccountDetailsEntryType;

            return listingDetail;
        }

       
    }
}
