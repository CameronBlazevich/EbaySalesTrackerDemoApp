using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EbaySalesTracker.ViewModels.Subscription
{
    public class IndexViewModel
    {
        public IList<SubscriptionPlan> Plans { get; set; }
    }

    public class BillingViewModel
    {
        public SubscriptionPlan Plan { get; set; }
        public string StripePublishableKey {
            get
            {
                return ConfigurationManager.AppSettings["stripePublishableKeyProd"];
            }

        }

        public string StripeToken { get; set; }
    }

}

