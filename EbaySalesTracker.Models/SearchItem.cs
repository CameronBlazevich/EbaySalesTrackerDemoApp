using eBay.Services.Finding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EbaySalesTracker.Models.FindingServiceReference;

namespace EbaySalesTracker.Models
{
    public class SearchItem
    {

            public string ItemId { get; set; }
            public string Title { get; set; }
            public double CurrentPrice { get; set; }
            public Category PrimaryCategory { get; set; }
            public Category SecondaryCategory { get; set; }
            public Condition Condition { get; set; }
            public ListingInfo ListingInfo { get; set; }
            public SellingStatus SellingStatus { get; set; }

    }
}
