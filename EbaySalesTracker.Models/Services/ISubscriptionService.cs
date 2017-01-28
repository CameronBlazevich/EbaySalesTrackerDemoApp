using Microsoft.AspNet.Identity;
using System;
namespace EbaySalesTracker.Models
{
    public interface ISubscriptionService
    {
        void Create(string userName, SubscriptionPlan plan, string stripeToken);
        Stripe.StripeCustomerService StripeCustomerService { get; }
        Stripe.StripeSubscriptionService StripeSubscriptionService { get; }
        UserManager<ApplicationUser> UserManager { get; }
        void CancelSubscription(string stripeUserId);
        void ReactivateSubscription(string stripeCustomerId);
    }
}
