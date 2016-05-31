using EbaySalesTracker.Models;
using System.Collections.Generic;

namespace EbaySalesTracker.Repository
{
    public interface IListingDetailRepository
    {      
        List<ListingDetail> GetListingDetailsByItemIdFromEbay(long itemId, string userToken);
        List<ListingDetail> GetAllListingDetailsFromEbay(List<long> itemIds, string userToken);
    }
}
