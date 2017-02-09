using eBay.Service.Core.Soap;
using eBay.Services.Finding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MoreLinq;

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
                return Listings.Where(r => r.SellingStatus.sellingState == "EndedWithSales").OrderByDescending(l => l.ListingInfo.endTime).ToList();
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
        public Models.SearchItem HighestPricedListing
        {
            get
            {
                if(SoldListings.Any())
                    return SoldListings.MaxBy(l => l.CurrentPrice);

                return new Models.SearchItem();
            }
        }
        public IEnumerable<Models.SearchItem> MostRecentSoldListings
        {
            get
            {
                if(SoldListings.Any())
                    return SoldListings.Take(3);

                return null;
            }
        }
    }
    public enum SearchResultBreakdownType
    {
        [Description("All Listings")]
        All,
        [Description("New")]
        NewCondition,
        [Description("Used")]
        UsedCondition,
        [Description("Fixed Price")]
        FixedPriced,
        [Description("Auction")]
        Auctions,
        [Description("Store")]
        StoreInventory,
        [Description("Used-Fixed")]
        UsedFixedPrice,
        [Description("Used-Auction")]
        UsedAuction,
        [Description("Used-Store")]
        UsedStoreInventory,
        [Description("New-Fixed")]
        NewFixedPrice,
        [Description("New-Auction")]
        NewAuction,
        [Description("New-Store")]
        NewStoreInventory
    }

    public static class SearchResultBreakdownTypeExtensions
    {
        public static string ToDescriptionString(this SearchResultBreakdownType val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}