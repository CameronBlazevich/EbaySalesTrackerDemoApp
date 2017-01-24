using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Models
{
    public class SubscriptionFeature
    {
        [Required,Key]      
        public int Id { get; set; }
        [Required, ForeignKey("SubscriptionPlan")]
        public int PlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        [Required,MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
    }
}
