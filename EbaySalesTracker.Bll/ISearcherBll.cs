using eBay.Service.Core.Soap;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EbaySalesTracker.Bll
{
    public interface ISearcherBll
    {
        SuggestedCategoryTypeCollection GetSuggestedCategories(string searchTerm, string userToken);
        ICollection<SearchItem> SearchByKeywordAndCategory(string keyword, string categoryId, int maxSearchResults);
        ICollection<SearchItem> FindActiveItemsMatchingCriteria(EbayItemSearchFilter searchFilter);
    }
}
