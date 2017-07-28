using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Models
{
    public class EbayItemSearchFilter
    {
        public string SearchTerm { get; set; }
        public string PriceLimit { get; set; }
        public string CategoryId { get; set; }
    }
}
