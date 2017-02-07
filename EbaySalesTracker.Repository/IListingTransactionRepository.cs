using EbaySalesTracker.Models;
using System.Collections.Generic;
using System;

namespace EbaySalesTracker.Repository
{
    public interface IListingTransactionRepository
    {
        ICollection<ListingTransaction> GetListingTransactionsByListingIdFromEbay(long listingId, string userToken);
        IEnumerable<ListingTransaction> GetTransactionsForListing(long listingId);        
    }
}
