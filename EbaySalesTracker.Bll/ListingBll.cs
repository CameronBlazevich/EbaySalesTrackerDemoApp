using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace EbaySalesTracker.Bll
{
    public class ListingBll : IListingBll
    {
        private IListingRepository _ListingRepository;
        private IListingDetailRepository _DetailRepository;
        public ListingBll():this(null,null)
        {

        }
        public ListingBll(IListingRepository listingRepo, IListingDetailRepository detailRepo)
        {
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _DetailRepository = detailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
        }

        public IEnumerable<Listing> GetSoldListingsByInventoryItem(int inventoryItemId, string userId)
        {
            return _ListingRepository.GetSoldListingsByInventoryItem(inventoryItemId, userId);
        }

        public IEnumerable<Listing> GetListingsByEndDate(DateTime startDate, DateTime endDate, string userId)
        {
            return _ListingRepository.GetListingsByEndDate(startDate, endDate, userId);
        }
       public Listing GetListingById(long listingId)
        {
            return _ListingRepository.GetListingById(listingId);
        }
       public IEnumerable<Listing> GetListingsByUser(int top, int skip, string userId)
        {
            return _ListingRepository.GetAllListingsByUser(top, skip, userId);
        }
        public int GetListingsCountByUser(string userId)
        {
            return _ListingRepository.GetListingsCountByUser(userId);
        }
        public void UpdateListing(Listing listing)
        {
            _ListingRepository.UpdateListing(listing);
        }
    }
}
