using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace EbaySalesTracker.Controllers
{
    public class DataServiceController : Controller
    {
        //Will be used for ajax calls from dashboard page

        IListingRepository _ListingRepository;
        IListingDetailRepository _ListingDetailRepository;
        IInventoryRepository _InventoryRepository;
        ApplicationDbContext ApplicationDbContext;
        UserManager<ApplicationUser> UserManager;

        public DataServiceController() : this(null,null,null)
        {
        }

        public DataServiceController(IListingRepository listingRepo, IListingDetailRepository listingDetailRepo, IInventoryRepository inventoryRepo)
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _ListingDetailRepository = listingDetailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
            _InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>();

            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: DataService
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInventoryItems()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return Json(_InventoryRepository.GetInventoryItemsByUser(user.Id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInventoryItemById(int id)
        {
            //var user = UserManager.FindById(User.Identity.GetUserId());
            return Json(_InventoryRepository.GetInventoryItemById(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemSalesDataByMonth(int id)
        {
            return Json(_InventoryRepository.CalculateItemProfitByMonth(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataForAverageProfitOverTime(int id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var data = _ListingRepository.GetListingDataByInventoryItem(user.Id, id);
            //test = EbaySalesTracker.Builders.DashboardBuilder.AverageProfitOverTime(user.Id, id);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}