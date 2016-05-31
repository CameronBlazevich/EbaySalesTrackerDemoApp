using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EbaySalesTracker.Repository
{
    public class ListingDetailRepository : RepositoryBase<EbaySalesTrackerContext>, IListingDetailRepository
    {
        ListingDetailEngine engine = new ListingDetailEngine();
        public List<ListingDetail> GetAllListingDetailsFromEbay(List<long> itemIds, string userToken)
        {
            //there is likely a better way to do this, than to make a call per id. should be able to 
            //get account with no id parameter and parse the xml
            var listingDetails = new List<ListingDetail>();
            listingDetails = engine.GetAllListingDetailsFromEbay(itemIds, userToken);
            SaveDetailsToDb(listingDetails);
            return listingDetails;
        }

        public List<ListingDetail> GetListingDetailsByItemIdFromEbay(long itemId, string userToken)
        {                                  
            List<ListingDetail> listingDetails = engine.GetListingDetailsByItemIdFromEbay(itemId, userToken);
            SaveDetailsToDb(listingDetails);           
            return listingDetails;
        }

        private OperationStatus SaveDetailsToDb(List<ListingDetail> listingDetails)
        {
            var opStatus = new OperationStatus();
            if (listingDetails != null && listingDetails.Count > 0)

                using (DataContext)
                {
                    foreach (var listDet in listingDetails)
                    {
                        //check if listing detail with this ref number exists
                        var exists = DataContext.ListingDetails.Where(ld => ld.RefNumber == listDet.RefNumber).Any();
                        if (!exists)
                        {
                            DataContext.ListingDetails.Add(listDet);
                            opStatus = Save(listDet);
                            if (!opStatus.Status)
                            {
                                listDet.Description = "Error getting listing detail.";
                            }
                        }
                    }

                }
            return opStatus;
        }

    }
}

