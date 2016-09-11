using eBay.Service.Call;
using eBay.Service.Core.Soap;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;

namespace EbaySalesTracker.Repository.Helpers
{
    public class ListingTransactionEngine : Engine
    {
        public ICollection<ListingTransaction> GetListingTransactionByListingIdFromEbay(long listingId, string userToken)
        {
            var context = RequestBuilder.CreateNewApiCall(userToken);
            var getItemTransactionsCall = new GetItemTransactionsCall(context);
            getItemTransactionsCall.ItemID = listingId.ToString();
            getItemTransactionsCall.OutputSelector = new string[] { "Item.ItemId", "TransactionId" };
            getItemTransactionsCall.Execute();

            var results = getItemTransactionsCall.ApiResponse.TransactionArray;
            var listingTransactions = new List<ListingTransaction>();
            foreach (TransactionType result in results)
            {
                var transaction = MapResultToListingTransaction(result);
                listingTransactions.Add(transaction);
            }
            return listingTransactions;
        }
        private ListingTransaction MapResultToListingTransaction(TransactionType result)
        {
            var listingTransaction = new ListingTransaction();
            listingTransaction.ListingId = Convert.ToInt64(result.Item.ItemID);
            listingTransaction.TransactionId = result.TransactionID;
            return listingTransaction;

        }
    }
}
