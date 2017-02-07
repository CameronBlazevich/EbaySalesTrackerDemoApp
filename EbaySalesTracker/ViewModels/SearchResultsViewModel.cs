using eBay.Service.Core.Soap;
using eBay.Services.Finding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbaySalesTracker.ViewModels
{
    public class SearchResultsViewModel
    {
        public SuggestedCategoryTypeCollection Categories { get; set; }
        public List<SearchResultBreakdown> SearchResultBreakdowns { get; set; }
        public string SearchTerm { get; set; }
    }
    public class SearchResultBreakdown
    {
        public SearchResultBreakdown(SearchResultBreakdownType type, IEnumerable<Models.SearchItem> listings)
        {
            Listings = listings;
            Type = type;
        }
        public IEnumerable<Models.SearchItem> Listings { get; set; }
        public IEnumerable<Models.SearchItem> SoldListings
        {
            get
            {
                return Listings.Where(r => r.SellingStatus.sellingState == "EndedWithSales").ToList();
            }
        }
        public SearchResultBreakdownType Type { get; set; }
        public double AverageSalePrice
        {
            get
            {
                if (SoldListings.Any())
                    return Math.Round(SoldListings.Average(l => l.SellingStatus.currentPrice.Value),2);

                return 0;
            }
        }
        public double SellThroughPct
        {
            get
            {
                if(Listings.Any())
                    return Math.Round((double)SoldListings.Count() / Listings.Count(), 3) * 100;

                return 0;
            }
        }
    }
    public enum SearchResultBreakdownType
    {
        All,
        NewCondition,
        UsedCondition,
        FixedPriced,
        Auctions,
        StoreInventory,
        UsedFixedPrice,
        UsedAuction,
        UsedStoreInventory,
        NewFixedPrice,
        NewAuction,
        NewStoreInventory
    }
}