using eBay.Service.Core.Soap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EbaySalesTracker.Models
{
    public class Listing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        public long ItemId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        //public int ProductId { get; set; }
        //public Product Product { get; set; }
        [Display(Name = "Current Price")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double CurrentPrice { get; set; }
        [Display(Name = "Qty Sold")]
        public int QuantitySold { get; set; }
        [Display(Name = "Status")]
        public ListingStatusCodeType ListingStatus {get; set;}
        //public ListingStatus ListingStatus { get; set; }
        [Display(Name = "Total Fees")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double TotalNetFees { get; set; }
        public double TotalGrossFees { get; set; }
        public List<ListingDetail> ListingDetails { get; set; }
        //public virtual ApplicationUser User { get; set; }
        [Required]    
        //[ForeignKey("User")] 
        public string UserId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public int? InventoryItemId { get; set;}
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double Profit { get; set; }
        public ListingTypeCodeType Type { get; set; }
        public virtual ICollection<ListingTransaction> Transactions { get; set; }
        [NotMapped, Display(Name = "Avg. Sale Price")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double AveragePrice
        {
            get
            {
                return this.CalculateAverageSalesPrice();
            }
        }

        public int CalculateQuantitySold()
        {
            if(Transactions.Count > 0)
                return Transactions.Sum(t => t.QuantitySold);

            return 0;           
        }
        public double CalculateGrossSales()
        {
            if (Transactions.Count > 0)
                return Transactions.Sum(t => t.UnitPrice * t.QuantitySold);

            return 0;
        }
        public double CalculateAverageSalesPrice()
        {
            if (Transactions.Count > 0)
            { 
                var totalSales = Transactions.Sum(x => x.QuantitySold * x.UnitPrice);
                var qtySold = CalculateQuantitySold();
                return totalSales / qtySold;
            }
            return 0;
        }
    }
}