using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Microsoft.AspNet.Identity.EntityFramework;
using EbaySalesTracker.Bll;

namespace EbaySalesTracker.Controllers
{
    [Authorize]
    public class InventoryItemsController : Controller
    {
        IListingRepository _ListingRepository;
        IListingDetailRepository _ListingDetailRepository;
        IInventoryRepository _InventoryRepository;
        IInventoryBll _InventoryBll;
        ApplicationDbContext ApplicationDbContext;
        UserManager<ApplicationUser> UserManager;

        public InventoryItemsController(): this(null,null,null,null)
        {

        }

        public InventoryItemsController(IListingRepository listingRepo, IListingDetailRepository listingDetailRepo, IInventoryRepository inventoryRepo, IInventoryBll inventoryBll)
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _ListingDetailRepository = listingDetailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
            _InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>();
            _InventoryBll = inventoryBll ?? new InventoryBll();

            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: InventoryItems
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var inventoryItems = _InventoryBll.GetInventoryItemsByUser(user.Id);
           
            return View(inventoryItems);
        }

        // GET: InventoryItems/Details/5
        public ActionResult Details(int? id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = _InventoryBll.GetInventoryItemById(Convert.ToInt32(id),user.Id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }
        // GET: InventoryItems/Details/5
        public ActionResult DetailsPartial(int? id,string viewName)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = _InventoryBll.GetInventoryItemById(Convert.ToInt32(id),user.Id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            //return Json(inventoryItem, JsonRequestBehavior.AllowGet);

            return PartialView(viewName, inventoryItem);
        }

        // GET: InventoryItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Cost")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                inventoryItem.UserId = user.Id;
                _InventoryRepository.CreateInventoryItem(inventoryItem);
               
                return RedirectToAction("Index");
            }

            return View(inventoryItem);
        }

        // GET: InventoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = _InventoryBll.GetInventoryItemById(Convert.ToInt32(id), user.Id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Cost,Quantity,AverageSalesPrice,AverageProfit")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                inventoryItem.UserId = user.Id;
                inventoryItem = _InventoryRepository.EditInventoryItem(inventoryItem);              
                return RedirectToAction("Index");
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = _InventoryBll.GetInventoryItemById(Convert.ToInt32(id),user.Id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _InventoryRepository.DeleteInventoryItem(id);
           
            return RedirectToAction("Index");
        }              
    }
}
