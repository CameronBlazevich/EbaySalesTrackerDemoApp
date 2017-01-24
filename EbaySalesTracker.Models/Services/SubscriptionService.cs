using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Stripe;
using EbaySalesTracker;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EbaySalesTracker.Models
{
    public class SubscriptionService : ISubscriptionService
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext ApplicationDbContext;

        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                
                return userManager ?? new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
            }
            private set
            {
                userManager = value;
            }
        }
        private StripeCustomerService customerService;
        public StripeCustomerService StripeCustomerService
        {
            get
            {
                return customerService ?? new StripeCustomerService();
            }
            private set
            {
                customerService = value;
            }
        }

        private StripeSubscriptionService subscriptionService;
        public StripeSubscriptionService StripeSubscriptionService
        {
            get
            {
                return subscriptionService ?? new StripeSubscriptionService();
            }
            private set
            {
                subscriptionService = value;
            }
        }

        public SubscriptionService()
        {
            ApplicationDbContext = new ApplicationDbContext();
        }

        public SubscriptionService(UserManager<ApplicationUser> userManager, StripeCustomerService customerService, StripeSubscriptionService subscriptionService)
        {
            this.userManager = userManager;
            this.customerService = customerService;
            this.subscriptionService = subscriptionService;
        }

        public void Create(string userName, SubscriptionPlan plan, string stripeToken)
        {
            var user = UserManager.FindByName(userName);

            if (String.IsNullOrEmpty(user.StripeCustomerId))  //first time customer
            {
                //create customer which will create subscription if plan is set and cc info via token is provided
                var customer = new StripeCustomerCreateOptions();

                customer.Email = user.Email;
                customer.SourceToken = stripeToken;
                customer.PlanId = plan.ExternalId; //externalid is stripe plan.id
                

                StripeCustomer stripeCustomer = StripeCustomerService.Create(customer);

                
                using (ApplicationDbContext)
                {
                    var dbUser = ApplicationDbContext.Users.Where(u => u.Email == userName).FirstOrDefault();
                    dbUser.StripeCustomerId = stripeCustomer.Id;
                    dbUser.StripeActiveUntil = DateTime.Now.AddDays((double)plan.TrialPeriodDays);
                    ApplicationDbContext.SaveChanges();
                }
                    

            }
            else
            {
                var stripeSubscription = StripeSubscriptionService.Create(user.StripeCustomerId, plan.ExternalId);
                using (ApplicationDbContext)
                {
                    var dbUser = ApplicationDbContext.Users.Where(u => u.Email == userName).FirstOrDefault();
                    dbUser.StripeActiveUntil = DateTime.Now.AddDays((double)plan.TrialPeriodDays);
                }
                //UserManager.Update(user);
            }

        }
        public void CancelSubscription(string stripeUserId)
        {
            var usersSubscriptions = StripeSubscriptionService.List(stripeUserId);
            var subscriptionId = usersSubscriptions.First().Id;
            StripeSubscriptionService.Cancel(stripeUserId, subscriptionId, cancelAtPeriodEnd: true);
        }
    }
}
