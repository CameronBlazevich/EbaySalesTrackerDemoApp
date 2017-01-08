using System.ComponentModel.DataAnnotations;

namespace EbaySalesTracker.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [Required]
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
