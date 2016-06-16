using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public double Quantity { get; set; }
        public double? AverageSalesPrice { get; set; }
        public double? AverageProfit { get; set; }
        public string UserId { get; set; }
        public int QuantitySold { get; set; }

    }
}
