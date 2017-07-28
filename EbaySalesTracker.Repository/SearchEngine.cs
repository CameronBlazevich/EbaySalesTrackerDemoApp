using eBay.Service.Core.Soap;
using eBay.Service.Call;
using EbaySalesTracker.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Configuration;
using eBay.Services;
using eBay.Services.Finding;
using EbaySalesTracker.Models;

namespace EbaySalesTracker.Repository
{
    public class SearchEngine
    {
        public SuggestedCategoryTypeCollection GetSuggestedCategories(string searchTerm, string userToken)
        {
            var context = RequestBuilder.CreateNewApiCall(userToken);
            var getSuggestedCategories = new GetSuggestedCategoriesCall(context);
            getSuggestedCategories.Query = searchTerm;
            getSuggestedCategories.Execute();
            //if(getSuggestedCategories.ApiResponse.Ack ==)
            var categoryCount = getSuggestedCategories.ApiResponse.CategoryCount;
            var suggestedCategories = getSuggestedCategories.ApiResponse.SuggestedCategoryArray.SuggestedCategory;    
     
            return suggestedCategories;
        }

        public ICollection<Models.SearchItem> FindItemsAdvanced(EbayItemSearchFilter searchFilter)
        {
            var maxSearchResults = 100;
            string appName = ConfigurationManager.AppSettings["ApplicationApiCredential"];
            // Initialize client site configuration
            ClientConfig config = new ClientConfig();
            // Initialize service end-point configration
            config.EndPointAddress = "http://svcs.ebay.com/services/search/FindingService/v1";
            // set eBay developer accoutn AppID
            config.ApplicationId = appName;


            FindingServicePortTypeClient client = FindingServiceClientFactory.getServiceClient(config);

            var request = new FindItemsAdvancedRequest();
            //FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();
            request.keywords = searchFilter.SearchTerm;
            request.sortOrder = SortOrderType.StartTimeNewest;
            request.categoryId = new string[] { searchFilter.CategoryId };

            //var itemFilter = new ItemFilter();
            //itemFilter.name = ItemFilterType.BestOfferOnly;
            //itemFilter.value = new string[] { "true" };

            var maxPriceItemFilter = new ItemFilter();
            maxPriceItemFilter.name = ItemFilterType.MaxPrice;
            maxPriceItemFilter.value = new string[] { searchFilter.PriceLimit };


            request.itemFilter = new ItemFilter[] { maxPriceItemFilter };


            PaginationInput paginationInput = new PaginationInput();
            var items = new List<Models.SearchItem>();
            var maxEntriesPerPage = 100;
            var maxIterations = maxSearchResults / maxEntriesPerPage;
            paginationInput.entriesPerPage = maxEntriesPerPage;

            for (var i = 0; i < maxIterations; i++)
            {
                paginationInput.pageNumber = i;

                request.paginationInput = paginationInput;

                FindItemsAdvancedResponse response = client.findItemsAdvanced(request);
                //FindItemsByKeywordsResponse response = client.findItemsByKeywords(request);

                if (response?.searchResult?.item == null || response.ack.ToString() != "Success")
                    return items;

                var results = response.searchResult.item;

                foreach (var result in results)
                {
                    
                    
                    var listing = MapResultToSearchItem(result);
                    items.Add(listing);
                    
                }

                if (response.searchResult.count < maxEntriesPerPage)
                    break;
            }
            return items.OrderByDescending(l => l.ListingInfo.startTime).ToList();
        }

        public ICollection<Models.SearchItem> SearchByKeywordAndCategory(string keyword, string categoryId, int maxSearchResults)
        {
            string appName = ConfigurationManager.AppSettings["ApplicationApiCredential"];
            // Initialize client site configuration
            ClientConfig config = new ClientConfig();
            // Initialize service end-point configration
            config.EndPointAddress = "http://svcs.ebay.com/services/search/FindingService/v1";
            // set eBay developer accoutn AppID
            config.ApplicationId = appName;


            FindingServicePortTypeClient client = FindingServiceClientFactory.getServiceClient(config);

            

                MessageHeader header = MessageHeader.CreateHeader("CustomHeader", "", "");

               
                    FindCompletedItemsRequest request = new FindCompletedItemsRequest();
                    //FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();
                    request.keywords = keyword;
                    request.categoryId = new string[] { categoryId };

                    //var itemFilter = new ItemFilter();
                    //itemFilter.name = ItemFilterType.Condition;
                    //itemFilter.value = new string[] { "1000" };
                    //request.itemFilter = new ItemFilter[] { itemFilter };


                    PaginationInput paginationInput = new PaginationInput();
                    var items = new List<Models.SearchItem>();
                    var maxEntriesPerPage = 100;
                    var maxIterations = maxSearchResults / maxEntriesPerPage;
                    paginationInput.entriesPerPage = maxEntriesPerPage;

                    for (var i = 0; i < maxIterations; i++)
                    {
                        paginationInput.pageNumber = i;

                        request.paginationInput = paginationInput;

                        FindCompletedItemsResponse response = client.findCompletedItems(request);
                        //FindItemsByKeywordsResponse response = client.findItemsByKeywords(request);

                        if (response?.searchResult?.item == null || response.ack.ToString() != "Success")
                            return items;

                        var results = response.searchResult.item;

                        foreach (var result in results)
                        {
                            var listing = MapResultToSearchItem(result);
                            items.Add(listing);
                        }

                        if (response.searchResult.count < maxEntriesPerPage)
                            break;
                    }
                    return items;
                
            
        }
        private Models.SearchItem MapResultToSearchItem(eBay.Services.Finding.SearchItem result)
        {
            var searchItem = new Models.SearchItem();
            
            searchItem.CurrentPrice = result.sellingStatus.currentPrice.Value;
            searchItem.ItemId = result.itemId;
            searchItem.Title = result.title;
            searchItem.PrimaryCategory = result.primaryCategory;
            searchItem.SecondaryCategory = result.secondaryCategory;
            searchItem.ListingInfo = result.listingInfo;
            searchItem.SellingStatus = result.sellingStatus;
            searchItem.Condition = result.condition;
            searchItem.GalleryImageUrl = result.galleryURL;
            searchItem.ViewItemUrl = result.viewItemURL;

            return searchItem;
        }
    }
}
