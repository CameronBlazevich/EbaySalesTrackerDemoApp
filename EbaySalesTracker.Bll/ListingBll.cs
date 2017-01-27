using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace EbaySalesTracker.Bll
{
    public class ListingBll : IListingBll
    {
        private IListingRepository _listingRepository;
        private IListingDetailRepository _detailRepository;
        public IListingRepository ListingRepository
        {
            get
            {
                return _listingRepository ?? ModelContainer.Instance.Resolve<IListingRepository>();
            }
            private set
            {
                _listingRepository = value;
            }
        }
        public IListingDetailRepository DetailRepository
        {
            get
            {
                return _detailRepository ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
            }
            private set
            {
                _detailRepository = value;
            }
        }

        public ListingBll()
        {

        }
        public ListingBll(IListingRepository listingRepo, IListingDetailRepository detailRepo)
        {
            ListingRepository = listingRepo;
            DetailRepository = detailRepo;
        }

        public IEnumerable<Listing> GetSoldListingsByInventoryItem(int inventoryItemId, string userId)
        {
            return ListingRepository.GetSoldListingsByInventoryItem(inventoryItemId, userId);
        }

        public IEnumerable<Listing> GetListingsByEndDate(DateTime startDate, DateTime endDate, string userId)
        {
            return ListingRepository.GetListingsByEndDate(startDate, endDate, userId);
        }
       public Listing GetListingById(long listingId)
        {
            return ListingRepository.GetListingById(listingId);
        }
       public IEnumerable<Listing> GetListingsByUser(int top, int skip, string userId)
        {
            return ListingRepository.GetAllListingsByUser(top, skip, userId);
        }
        public int GetListingsCountByUser(string userId)
        {
            return ListingRepository.GetListingsCountByUser(userId);
        }
        public Listing UpdateListing(Listing listing)
        {
            return ListingRepository.UpdateListing(listing);
        }

        public void UpdateListings(string userId)
        {
            var lastListingUpdate = ListingRepository.GetLastListingsUpdate(userId);
            
            if (lastListingUpdate == null)
            {
                var twoYearsAgo = DateTime.Now.AddYears(-2);
                lastListingUpdate = twoYearsAgo;
            }
            
            ListingRepository.UpdateListings(((DateTime)lastListingUpdate).AddDays(-10), userId);


        }

        public IEnumerable<Listing> GetListingsBySoldDate(DateTime startDate, DateTime endDate, string userId)
        {
            return ListingRepository.GetListingsBySoldDate(startDate, endDate, userId);
        }
    }
}
