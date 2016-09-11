using EbaySalesTracker.Bll;
using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using EbaySalesTracker.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EbaySalesTracker.Controllers
{
    [Authorize]
    public class ListingsController : Controller
    {
        //private EbaySalesTrackerContext db = new EbaySalesTrackerContext();
        IListingRepository _ListingRepository;
        IListingDetailRepository _ListingDetailRepository;
        IInventoryRepository _InventoryRepository;
        IInventoryBll _InventoryBll;
        ApplicationDbContext ApplicationDbContext;
        UserManager<ApplicationUser> UserManager;

        

        public ListingsController() : this(null,null,null,null)
        {                
        }

        public ListingsController(IListingRepository listingRepo, IListingDetailRepository listingDetailRepo, IInventoryRepository inventoryRepo, IInventoryBll inventoryBll)
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _ListingDetailRepository = listingDetailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
            _InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>();
            _InventoryBll = inventoryBll ?? new InventoryBll();
           
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        

        // GET: Listings
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userId = user.Id;
            var listingsViewModel = new ListingsViewModel();
            listingsViewModel.Listings = _ListingRepository.GetAllListingsByUser(userId);
           
            var items = _InventoryRepository.GetInventoryItemsByUser(userId).ToList();
            listingsViewModel.Items = items;
            
            foreach (var listing in listingsViewModel.Listings)
            {
                listing.Profit = _InventoryBll.CalculateProfitPerListing(listing.ItemId, user.Id);
                if (listing.InventoryItemId != null)
                {
                    listing.InventoryItem = items.Where(x => x.Id == listing.InventoryItemId).First();
                    //listingsViewModel.InventoryItems = new SelectList(listingsViewModel.Items, "Id", "Description", listing.InventoryItemId);
                }
            }
           

            return View(listingsViewModel);
        }

        // GET: Listings/Details/5
        public ActionResult Details(long id)
        {
            Listing listing = _ListingRepository.GetListingById(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        // GET: Listings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,StartDate,EndDate,Title,CurrentPrice,QuantitySold,ListingStatus")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                _ListingRepository.AddListing(listing);
                return RedirectToAction("Index");
            }

            return View(listing);
        }

        //// GET: Listings/Edit/5
        //public ActionResult Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Listing listing = db.Listings.Find(id);
        //    if (listing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(listing);
        //}

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ItemId,StartDate,EndDate,Title,CurrentPrice,QuantitySold,ListingStatus")] Listing listing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(listing).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(listing);
        //}

        // GET: Listings/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Listing listing = db.Listings.Find(id);
        //    if (listing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(listing);
        //}

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            _ListingRepository.DeleteListing(id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateFeesById(long id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userToken = user.UserToken;
            _ListingRepository.UpdateFeesById(id, userToken);
            return RedirectToAction("Index");
        }

        public ActionResult GetAllListings()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            string userId = user.Id;
            _ListingRepository.GetAllListingsSinceDateFromEbay(new DateTime(2015,01,01), userId);
            //_ListingDetailRepository.GetAllListingDetailsFromEbay(db.Listings.Select(i => i.ItemId).ToList());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AssociateInventoryItemToListing(string inventoryItemId, string listingItemId)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var listingId = Convert.ToInt64(listingItemId);
            if (inventoryItemId == "")
            {
                _ListingRepository.DissociateInventoryItem(listingId);
            }
            else {
                //_ListingRepository.AssociateInventoryItem(listingId, Convert.ToInt32(inventoryItemId));
                _InventoryBll.AssociateInventoryItemToListing(listingId, Convert.ToInt32(inventoryItemId), user.Id);
            }

            var profit = _InventoryBll.CalculateProfitPerListing(listingId, user.Id);
            
            return Json(new { profit = profit });
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
