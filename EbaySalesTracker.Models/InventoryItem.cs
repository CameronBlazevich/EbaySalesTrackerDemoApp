using System.ComponentModel.DataAnnotations;

namespace EbaySalesTracker.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name ="Product Cost")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double Cost { get; set; }
        public double Quantity { get; set; }
        [Display(Name = "Average Sales Price")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double? AverageSalesPrice { get; set; }
        [Display(Name = "Average Profit")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double? AverageProfit { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Quantity Sold")]
        public int QuantitySold { get; set; }
    }
}
