using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using EbaySalesTracker.ViewModels.Subscription;
using Microsoft.AspNet.Identity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EbaySalesTracker.Controllers
{
    public class SubscriptionController : Controller
    {
        private IPlanService planService;
        private ISubscriptionService subscriptionService;
        private IUserRepository userRepository;     
        ApplicationDbContext ApplicationDbContext;
        UserManager<ApplicationUser> UserManager;
        public ISubscriptionService SubscriptionService
        {
            get
            {
                return subscriptionService ?? new SubscriptionService();
            }
            private set
            {
                subscriptionService = value;
            }
        }

        public SubscriptionController(IPlanService planService, ISubscriptionService subscriptionService, IUserRepository userRepo)
        {
            this.planService = planService;
            this.subscriptionService = subscriptionService;
            this.userRepository = userRepo;
        }
        public IPlanService PlanService
        {
            get
            {
                return planService ?? new PlanService();
            }
            private set
            {
                planService = value;
            }           
        }
        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ?? ModelContainer.Instance.Resolve<IUserRepository>();
            }
            private set
            {
                userRepository = value;
            }
        }

        public SubscriptionController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }
        // GET: Subscription
        public ActionResult Index()
        {
            var viewModel = new ViewModels.Subscription.BillingViewModel() { Plan = PlanService.List().FirstOrDefault() };
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Billing(BillingViewModel billingViewModel)
        {            
            try
            {
                SubscriptionService.Create(User.Identity.Name, PlanService.Find(billingViewModel.Plan.Id), billingViewModel.StripeToken);
            }
            catch (StripeException stripeException)
            {
                ModelState.AddModelError(string.Empty, stripeException.Message);
                billingViewModel.Plan = PlanService.Find(billingViewModel.Plan.Id);
                return View("Index", billingViewModel);
            }
            return RedirectToAction("Index", "Listings");
        }
        [HttpPost]
        public ActionResult CancelSubscription(string cancellationReason)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            UserRepository.SetUserCancelReason(user,cancellationReason);
            var stripeUserId = user.StripeCustomerId;
            SubscriptionService.CancelSubscription(stripeUserId);

            return RedirectToAction("Index", "Manage", new { message = 7 });
        }
    }
}