using EbaySalesTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using EbaySalesTracker.Models;

namespace EbaySalesTracker.Builders
{
    public class DashboardBuilder
    {
        IListingRepository _ListingRepository;
        //IListingDetailRepository _ListingDetailRepository;
        //IUserRepository _UserRepository;
        //IInventoryRepository _InventoryRepository;
        public DashboardBuilder() : this(null)
        {
        }
        public DashboardBuilder(IListingRepository listingRepo)
        {
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
        }

        public double[,] AverageProfitOverTime(string userId, int inventoryItemId)
        {
            
            List<Listing> listings = _ListingRepository.GetListingsByInventoryItem(userId, inventoryItemId);
            double[,] data = new double[listings.Count(), 2];
            var count = 0;
            foreach (var listing in listings)
            {
                data[count, 0] = count;
                data[count, 1] = listing.Profit;
            }
            return data;
        }
    }
}