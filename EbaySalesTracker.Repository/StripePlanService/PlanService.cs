using EbaySalesTracker.Models;
using System.Collections.Generic;
using System.Linq;
using Stripe;


namespace EbaySalesTracker.Repository
{
    public class PlanService : RepositoryBase<EbaySalesTrackerContext>, IPlanService
    {
        private StripePlanService stripePlanService;
        public PlanService(StripePlanService stripePlanService)
        {
            this.stripePlanService = stripePlanService;
        }
        public PlanService() : this(new StripePlanService())
        {

        }
        public SubscriptionPlan Find(int id)
        {
            var plan = new SubscriptionPlan();
            using (DataContext)
            {
                plan = DataContext.SubscriptionPlans.Where(p => p.Id == id).SingleOrDefault();
            }
            var stripePlan = stripePlanService.Get(plan.ExternalId);
            StripePlanToSubscriptionPlan(stripePlan, plan);

            return plan;
        }
        public IList<SubscriptionPlan> List()
        {
            var plans = new List<SubscriptionPlan>();
            using (DataContext)
            {
                plans = DataContext.SubscriptionPlans.ToList();
            }

            var stripePlans = (from p in stripePlanService.List() select p).ToList();

            foreach (var plan in plans)
            {
                var stripePlan = stripePlans.Single(p => p.Id == plan.ExternalId);
                StripePlanToSubscriptionPlan(stripePlan, plan);
            }

            return plans;

        }
        private static void StripePlanToSubscriptionPlan(StripePlan stripePlan, SubscriptionPlan plan)
        {
            plan.Name = stripePlan.Name;
            plan.AmountInCents = stripePlan.Amount;
            plan.Currency = stripePlan.Currency;
            plan.Interval = stripePlan.Interval;
            plan.TrialPeriodDays = stripePlan.TrialPeriodDays;
        }
    }
}
