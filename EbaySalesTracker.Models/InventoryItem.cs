using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Average Sales Price")]
        public double? AverageSalesPrice { get; set; }
        [Display(Name = "Average Profit")]
        public double? AverageProfit { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Quantity Sold")]
        public int QuantitySold { get; set; }

    }
}
