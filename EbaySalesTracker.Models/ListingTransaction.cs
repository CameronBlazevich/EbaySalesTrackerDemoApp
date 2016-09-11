using System.ComponentModel.DataAnnotations.Schema;

namespace EbaySalesTracker.Models
{
    public class ListingTransaction
    {
        public int Id { get; set; }
        public long ListingId { get; set; }
        [ForeignKey("ListingId")]
        public Listing Listing { get; set; }
        public string TransactionId { get; set; }

    }
}
