using System;
using eBay.Service.Core.Soap;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EbaySalesTracker.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        public string OrderId { get; set; }
        public double TotalCost { get; set;}
        public double Shipping { get; set; }
        public double Handling { get; set; }
        public double SalesPrice { get; set; }
        public DateTime? PaidTime { get; set; }
        public DateTime? ShippedTime { get; set; }
        public double TotalTaxAmount { get; set; }
        public OrderStatusCodeType OrderStatus { get; set; }
        public long ListingId { get; set; }
        [ForeignKey("ListingId")]
        public Listing listing { get; set; }
        public double? RefundAmount { get; set; }
        public DateTime? RefundTime { get; set; }
        public PaymentTransactionStatusCodeType? RefundStatus { get; set; }

    }
}
