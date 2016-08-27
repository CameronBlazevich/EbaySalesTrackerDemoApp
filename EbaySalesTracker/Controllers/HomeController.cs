
using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace EbaySalesTracker.Controllers
{
    public class HomeController : Controller
    {
        //private EbaySalesTrackerContext db = new EbaySalesTrackerContext();
        IListingRepository _ListingRepository;
        IListingDetailRepository _ListingDetailRepository;
        IUserRepository _UserRepository;
        IInventoryRepository _InventoryRepository;
        ApplicationDbContext ApplicationDbContext;
        UserManager<ApplicationUser> UserManager;
       string userId = "";


        public HomeController() : this(null,null,null,null)
        {
        }

        public HomeController(IListingRepository listingRepo, IListingDetailRepository listingDetailRepo, IUserRepository userRepo, IInventoryRepository inventoryRepo)
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _ListingDetailRepository = listingDetailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
            _UserRepository = userRepo ?? ModelContainer.Instance.Resolve<IUserRepository>();
            _InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>();

            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            ViewData["User"] = user;
            return View();
        }

        public ActionResult About()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var sessionId = user.SessionId;
            var userId = user.Id;
            var userToken = user.UserToken;
            //return Json(_ListingRepository.GetListingByItemIdFromEbay(222106907586), JsonRequestBehavior.AllowGet);

            //return Json(_ListingDetailRepository.GetListingDetailsByItemIdFromEbay(222106907586), JsonRequestBehavior.AllowGet);
           // return Json(_ListingDetailRepository.GetListingDetailsByItemIdFromEbay(222106907586, userToken), JsonRequestBehavior.AllowGet);

            //return Json(_ListingRepository.GetListingsByEndDateFromEbay(new DateTime(2016,03,01), new DateTime(2016,05,20),userToken), JsonRequestBehavior.AllowGet);
           // return Json(_InventoryRepository.CalculateItemProfitByMonth(1), JsonRequestBehavior.AllowGet);

            //return Json(_ListingRepository.GetListingsByStartDateFromEbay(new DateTime(2016,03,01), new DateTime(2016,05,20),userId, userToken), JsonRequestBehavior.AllowGet);

            //return Json(_ListingRepository.GetAllListingsSinceDateFromEbay(new DateTime(2015,03,01)), JsonRequestBehavior.AllowGet);

            //ViewBag.Message = "About Page";
            //_UserRepository.TestEbayApiStuff(userToken);
            //return Json(_UserRepository.GetSessionId(), JsonRequestBehavior.AllowGet);
            // return Json(_UserRepository.GetSessionId(userId), JsonRequestBehavior.AllowGet);
            //return Json(_UserRepository.GetUserToken(userId,sessionId), JsonRequestBehavior.AllowGet);
            //return Json();

            //return Json(_ListingRepository.GetListingByItemIdFromEbay(222106907586, "AgAAAA**AQAAAA**aAAAAA**9Rc1Vw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AFl4enAJGBpAqdj6x9nY+seQ**JUgDAA**AAMAAA**kpn6ddRezaL9hKw9yeKXpVk4E6vO4fvDpeViYkVIaS0A5J0sP0jAX9ZWfVY34BqCgBgNKdKECco5LmoX1QxWWvD0OaFreyvT5ROo9PepEces++SuKgvAmS2JZBgxdPeAJtOrIZ1sbDSqMvMjxO/733g+iA3VuXw3XLY+y6S9rEGjyVh0AFGvLPvXEDdxnHHbspo4gB1SrHbRPkuz1tVv9KVaPonWw6Pjvrz4+HhRxGOLDmRPpuEZCCNJKj29VuWnLhNKNAzOYo4apXlGbQoTa7y/W6wnI/9Sqb6QfbyYSoqNkQFlZtPgLas4Z6SFhEF86JlhctHzBD+QqYrcE9lU3SJS8TaQcwM18i3PaG0qO2UYcDgygChFtXwSgHKE45q4qpFO16GLvfl3VQoMYasT9xBw65/2f4AWa3s0XZt7zhPo18famPCsTnEr6NvfFLyrCMi3X20faqjvnAlQhtA3RpoLdOLNVjR0ttCBAj08KqXBtEQ31r97ExlU01JBHkwpDvT26SNVvU9h8Pc9bISOskPVYdh31Gfa80XukvZrkL8L3hDJMc7DAK0V4lMQVGHrnJ9cyNVNo/KULkcXRYKkQYGhWdXAR5PuYPATWH7q6FNg4GpdETcgBv7kNdWVm197dCYVFmo4EAB9jA5q1nO26M7Sx3p70ZizFXcX0EqZuB1c7Mhkx2gllApq+2c+IF4SuRfEeRIC6JyxQzx7Yhd4N3wfOan3KfPnXfxXbPqR7MEe89dMHSCLqHSnROhh004Q"),JsonRequestBehavior.AllowGet);

            //return Json(_InventoryRepository.GetBestSellingItem(userId), JsonRequestBehavior.AllowGet);
            return Json(_InventoryRepository.GetHighestAverageProfitItem(userId), JsonRequestBehavior.AllowGet);
            //return Json(userId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetUserToken()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            userId = user.Id;
            string sessionId = _UserRepository.GetSessionId(userId);
            sessionId = HttpUtility.UrlEncode(sessionId);
            //sandbox
            //var url = "https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn&runame=Cameron_Blazevi-CameronB-EbayFe-dsbmnl&SessID=" + sessionId;

            //production
            var url = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame=Cameron_Blazevi-CameronB-EbayFe-urvcak&amp;SessID=" + sessionId;

            return Redirect(url);
            //return View();
        }
    }
}