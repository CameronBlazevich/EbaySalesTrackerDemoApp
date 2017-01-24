using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbaySalesTracker.Models
{
    public class SubscriptionPlan
    {
        [Required, Key]
        public int Id { get; set; }
        [MaxLength(50), Required]
        public string ExternalId { get; set; }
        [MaxLength(500), Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public int AmountInCents { get; set; }
        [NotMapped]
        public string Currency { get; set; }
        [NotMapped]
        public string Interval { get; set; }
        [NotMapped]
        public int? TrialPeriodDays { get; set; }
        [NotMapped]
        public int AmountInDollars
        {
            get
            {
                return (int)Math.Floor((decimal)this.AmountInCents / 100);
            }
        }

    }
}
