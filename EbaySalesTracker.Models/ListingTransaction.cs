using eBay.Service.Core.Soap;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbaySalesTracker.Models
{
    public class ListingTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        public string TransactionId { get; set; }
        public long ListingId { get; set; }
        [ForeignKey("ListingId")]
        public Listing Listing { get; set; }
        // public int QuantityListed { get; set; }
        //public int QuantitySold { get; set; }
        public double TotalAmountPaid { get; set; }
        public double UnitPrice { get; set; }
        public double BuyerShippingCost { get; set; }
        public double BuyerHandlingCost { get; set; }
        public int QuantitySold { get; set; }
        public string OrderId { get; set; }
        public DateTime CreatedDate {get;set;}
        public CompleteStatusCodeType Status { get; set; }
    }
}
