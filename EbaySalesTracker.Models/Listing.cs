using eBay.Service.Core.Soap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public double CurrentPrice { get; set; }
        [Display(Name = "Quantity Sold")]
        public int QuantitySold { get; set; }
        [Display(Name = "Listing Status")]
        public ListingStatusCodeType ListingStatus {get; set;}
        //public ListingStatus ListingStatus { get; set; }
        [Display(Name = "Total Net Fees")]
        public double TotalNetFees { get; set; }
        public double TotalGrossFees { get; set; }
        public List<ListingDetail> ListingDetails { get; set; }
        //public virtual ApplicationUser User { get; set; }
        [Required]    
        //[ForeignKey("User")] 
        public string UserId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public int? InventoryItemId { get; set;}
        public double Profit { get; set; }



    }
}