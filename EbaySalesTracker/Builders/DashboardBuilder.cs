using EbaySalesTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using EbaySalesTracker.Models;
using EbaySalesTracker.Bll;

namespace EbaySalesTracker.Builders
{
    public class DashboardBuilder
    {
        //IListingRepository _ListingRepository;
        //IInventoryBll _InventoryBll;
        IListingBll _ListingBll;
        //IListingDetailRepository _ListingDetailRepository;
        //IUserRepository _UserRepository;
        //IInventoryRepository _InventoryRepository;
        public DashboardBuilder() : this(null)
        {
        }
        public DashboardBuilder(IListingBll listingBll)
        {
            //_ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _ListingBll = listingBll ?? new ListingBll();
        }

        public double[,] AverageProfitOverTime(string userId, int inventoryItemId)
        {
            
            var listings = _ListingBll.GetSoldListingsByInventoryItem(inventoryItemId,userId);
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