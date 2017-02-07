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
            getItemTransactionsCall.IncludeContainingOrder = true;
            //getItemTransactionsCall.OutputSelector = new string[] { "Item.ItemId", "TransactionId" };
            getItemTransactionsCall.Execute();

            var results = getItemTransactionsCall.ApiResponse.TransactionArray;
            var listingTransactions = new List<ListingTransaction>();
            foreach (TransactionType result in results)
            {               
                var transaction = MapResultToListingTransaction(result);
                transaction.ListingId = listingId;
                listingTransactions.Add(transaction);   
                             
            }
            return listingTransactions;
        }
        public ListingTransaction MapResultToListingTransaction(TransactionType result)
        {
            var listingTransaction = new ListingTransaction();
            listingTransaction.TransactionId = result.TransactionID;
            listingTransaction.ListingId = long.Parse(result.Item.ItemID);
            listingTransaction.BuyerHandlingCost = result.ActualHandlingCost?.Value ?? 0;
            listingTransaction.BuyerShippingCost = result.ActualShippingCost?.Value ?? 0;
            listingTransaction.QuantitySold = result.QuantityPurchased;
            listingTransaction.UnitPrice = result.TransactionPrice?.Value ?? 0;
            listingTransaction.TotalAmountPaid = result.AmountPaid?.Value ?? 0;
            listingTransaction.OrderId = result.ContainingOrder?.OrderID;
            listingTransaction.CreatedDate = result.CreatedDate;
            listingTransaction.Status = result.Status.CompleteStatus;
            return listingTransaction;

        }
    }
}
