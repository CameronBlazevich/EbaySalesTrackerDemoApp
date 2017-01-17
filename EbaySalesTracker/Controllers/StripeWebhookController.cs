using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EbaySalesTracker.Controllers
{
    public class StripeWebhookController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Hello world everything is 200 OK");
        }
    }
}