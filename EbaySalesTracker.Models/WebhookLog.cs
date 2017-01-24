using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Models
{
    public class WebhookLog
    {
        
        [Key]
        public int Id { get; set; }
        public string EventId { get; set; }
        public string StripeUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string EventType { get; set; }
        public DateTime LoggedDate { get; set; }
        
    }
}
