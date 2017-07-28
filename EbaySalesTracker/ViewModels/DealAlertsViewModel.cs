using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EbaySalesTracker.ViewModels
{
    public class DealAlertsViewModel
    {
        [DisplayName("Seach Term")]
        public string SearchTerm { get; set; }
        [DisplayName("Price Limit")]
        public string PriceLimit { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        public List<Models.SearchItem> SearchResults { get; set; }
    }
}