using EbaySalesTracker.Models;
using System.Data.Entity;

namespace EbaySalesTracker.Repository
{
    public class EbaySalesTrackerContext : DbContext
    {
        public EbaySalesTrackerContext()
            : base("DefaultConnection")
            {
            }
        public virtual DbSet<Listing> Listings { get; set; }
        public virtual DbSet<ListingDetail> ListingDetails { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<InventoryItem> InventoryItems { get; set; }
        public virtual DbSet<ListingTransaction> ListingTransactions { get; set; }

    }
}

