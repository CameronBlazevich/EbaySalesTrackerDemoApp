using EbaySalesTracker.Bll;
using EbaySalesTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBay.Service.Core.Soap;

namespace EbaySalesTracker.Controllers
{
    public class SearcherController : Controller
    {
        public enum ConditionId
        {
            New = 1000,
            NewOther = 1500,
            NewWithDefects = 1750,
            ManufacturerRefurbished = 2000,
            SellerRefurbished = 2500,
            Used = 3000,
            VeryGood = 4000,
            Good = 5000,
            Acceptable = 6000,
            ForPartsNotWorking = 7000
        }
        
        private ISearcherBll _seacherBll;
        public ISearcherBll SearcherBll
        {
            get
            {
                return _seacherBll ?? new SearcherBll();
            }
            private set
            {
                _seacherBll = value;
            }
        }
       

        // GET: Searcher
        public ActionResult Index(string searchTerm = null)
        {
           
            var vM = new SearchResultsViewModel();
            if (searchTerm != null)
            {
                vM.SearchTerm = searchTerm;
                vM.Categories = SearcherBll.GetSuggestedCategories(searchTerm, "AgAAAA**AQAAAA**aAAAAA**5aFyWA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AFl4enAJGBpAqdj6x9nY+seQ**JUgDAA**AAMAAA**kpn6ddRezaL9hKw9yeKXpVk4E6vO4fvDpeViYkVIaS0A5J0sP0jAX9ZWfVY34BqCgBgNKdKECco5LmoX1QxWWvD0OaFreyvT5ROo9PepEces++SuKgvAmS2JZBgxdPeAJtOrIZ1sbDSqMvMjxO/733g+iA3VuXw3XLY+y6S9rEGjyVh0AFGvLPvXEDdxnHHbspo4gB1SrHbRPkuz1tVv9KVaPonWw6Pjvrz4+HhRxGOLDmRPpuEZCCNJKj29VuWnLhNKNAzOYo4apXlGbQoTa7y/W6wnI/9Sqb6QfbyYSoqNkQFlZtPgLas4Z6SFhEF86JlhctHzBD+QqYrcE9lU3SJS8TaQcwM18i3PaG0qO2UYcDgygChFtXwSgHKE45q4qpFO16GLvfl3VQoMYasT9xBw65/2f4AWa3s0XZt7zhPo18famPCsTnEr6NvfFLyrCMi3X20faqjvnAlQhtA3RpoLdOLNVjR0ttCBAj08KqXBtEQ31r97ExlU01JBHkwpDvT26SNVvU9h8Pc9bISOskPVYdh31Gfa80XukvZrkL8L3hDJMc7DAK0V4lMQVGHrnJ9cyNVNo/KULkcXRYKkQYGhWdXAR5PuYPATWH7q6FNg4GpdETcgBv7kNdWVm197dCYVFmo4EAB9jA5q1nO26M7Sx3p70ZizFXcX0EqZuB1c7Mhkx2gllApq+2c+IF4SuRfEeRIC6JyxQzx7Yhd4N3wfOan3KfPnXfxXbPqR7MEe89dMHSCLqHSnROhh004Q");
            }
                return View(vM);
        }

        public ActionResult GetSearchItemsByCategory(string categoryId, string searchTerm)
        {
            var vM = new SearchResultsViewModel();
            vM.SearchResultBreakdowns = new List<SearchResultBreakdown>();
            vM.SearchTerm = searchTerm;
            var results = SearcherBll.SearchByKeywordAndCategory(searchTerm, categoryId, 1000);
            

            if (results.Count > 0)
            {
                var soldListings = results.Where(r => r.SellingStatus.sellingState == "EndedWithSales").ToList();

                var allListings = new SearchResultBreakdown(SearchResultBreakdownType.All, results);
                vM.SearchResultBreakdowns.Add(allListings);


                //New Item Listings
                var newItemListings = results.Where(l => l.Condition.conditionIdSpecified && l.Condition.conditionId == (int)ConditionId.New).ToList();
                var newItemListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.NewCondition, newItemListings);
                vM.SearchResultBreakdowns.Add(newItemListingsBreakdown);

                //Used Item Listings
                var usedItemListings = results.Where(l => l.Condition.conditionIdSpecified && l.Condition.conditionId == (int)ConditionId.Used).ToList();
                var usedItemListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedCondition, usedItemListings);
                vM.SearchResultBreakdowns.Add(usedItemListingsBreakdown);

                //FixedPrice Listings
                var fixedPriceListings = results.Where(l => l.ListingInfo.listingType == "FixedPrice").ToList();
                var fixedPriceListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.FixedPriced, fixedPriceListings);
                vM.SearchResultBreakdowns.Add(fixedPriceListingsBreakdown);

                //Auction Listings
                var auctionListings = results.Where(l => l.ListingInfo.listingType == "Auction").ToList();
                var auctionListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.Auctions, auctionListings);
                vM.SearchResultBreakdowns.Add(auctionListingsBreakdown);

                //Store Inventory Listings
                var storeInventoryListings = results.Where(l => l.ListingInfo.listingType == "StoreInventory").ToList();
                var storeInventoryListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.StoreInventory, storeInventoryListings);
                vM.SearchResultBreakdowns.Add(storeInventoryListingsBreakdown);


                //Used +
                var usedFixedPriceListings = usedItemListings.Intersect(fixedPriceListings).ToList();
                var usedFixedPriceListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedFixedPrice, usedFixedPriceListings);
                vM.SearchResultBreakdowns.Add(usedFixedPriceListingsBreakdown);

                var usedAuctionListings = usedItemListings.Intersect(auctionListings).ToList();
                var usedAuctionListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedAuction, usedAuctionListings);
                vM.SearchResultBreakdowns.Add(usedAuctionListingsBreakdown);

                var usedStoreInventoryListings = usedItemListings.Intersect(storeInventoryListings).ToList();
                var usedStoreInventoryListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedStoreInventory, usedStoreInventoryListings);
                vM.SearchResultBreakdowns.Add(usedStoreInventoryListingsBreakdown);

                //New + 
                var newFixedPriceListings = newItemListings.Intersect(fixedPriceListings).ToList();
                var newFixedPriceListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedFixedPrice, usedFixedPriceListings);
                vM.SearchResultBreakdowns.Add(newFixedPriceListingsBreakdown);

                var newAuctionListings = newItemListings.Intersect(auctionListings).ToList();
                var newAuctionListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedAuction, usedAuctionListings);
                vM.SearchResultBreakdowns.Add(newAuctionListingsBreakdown);

                var newStoreInventoryListings = newItemListings.Intersect(storeInventoryListings).ToList();
                var newStoreInventoryListingsBreakdown = new SearchResultBreakdown(SearchResultBreakdownType.UsedStoreInventory, usedStoreInventoryListings);
                vM.SearchResultBreakdowns.Add(newStoreInventoryListingsBreakdown);







            }
            return View("Index", vM);
        }
    }
}