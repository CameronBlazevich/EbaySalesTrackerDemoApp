using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBay.Service;
using eBay.Service.Core.Soap;
using EbaySalesTracker.Repository;
using EbaySalesTracker.Models;

namespace EbaySalesTracker.Bll
{
    public class SearcherBll : ISearcherBll
    {
        private SearchEngine _searchEngine;
        public SearchEngine SearchEngine
        {
            get
            {
                return _searchEngine ?? new SearchEngine();
            }
            private set
            {
                _searchEngine = value;
            }
        }

        public ICollection<SearchItem> FindActiveItemsMatchingCriteria(EbayItemSearchFilter searchFilter)
        {
            var results = SearchEngine.FindItemsAdvanced(searchFilter);
            return results;
        }

        public SuggestedCategoryTypeCollection GetSuggestedCategories(string searchTerm, string userToken)
        {

            var suggestedCategories = SearchEngine.GetSuggestedCategories(searchTerm, userToken);

            return suggestedCategories;
        }

        public ICollection<SearchItem> SearchByKeywordAndCategory(string keyword, string categoryId, int maxSearchResults)
        {
            var results =  SearchEngine.SearchByKeywordAndCategory(keyword, categoryId, maxSearchResults);

            return results;

        }
    }
    }

