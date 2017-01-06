using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using EbaySalesTracker.Bll;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Claims;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;

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
        IListingBll _ListingBll;
        IInventoryBll _InventoryBll;
        UserManager<ApplicationUser> UserManager;
       string userId = "";


        public HomeController() : this(null,null,null,null,null)
        {
        }

        public HomeController(IListingRepository listingRepo, IListingDetailRepository listingDetailRepo, IUserRepository userRepo, 
            IInventoryRepository inventoryRepo, IInventoryBll inventoryBll)
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            _ListingRepository = listingRepo ?? ModelContainer.Instance.Resolve<IListingRepository>();
            _ListingDetailRepository = listingDetailRepo ?? ModelContainer.Instance.Resolve<IListingDetailRepository>();
            _UserRepository = userRepo ?? ModelContainer.Instance.Resolve<IUserRepository>();
            _InventoryRepository = inventoryRepo ?? ModelContainer.Instance.Resolve<IInventoryRepository>();
            //_ListingBll = listingBll ?? ModelContainer.Instance.Resolve<IListingBll>();
            //_InventoryBll = inventoryBll ?? ModelContainer.Instance.Resolve<IInventoryBll>();
            _InventoryBll = inventoryBll ?? new InventoryBll();

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
            //var code = this.Request.QueryString["code"]?.ToString();
            //var tempToken = this.Request.QueryString["ebaytkn"]?.ToString();
            //var user = UserManager.FindById(User.Identity.GetUserId());
            //var sessionId = user.SessionId;
            //var userId = user.Id;
            //var userToken = user.UserToken;
            //return Json(_ListingRepository.GetListingByItemIdFromEbay(222106907586), JsonRequestBehavior.AllowGet);

            //return Json(_ListingDetailRepository.GetListingDetailsByItemIdFromEbay(222106907586), JsonRequestBehavior.AllowGet);
            // return Json(_ListingDetailRepository.GetListingDetailsByItemIdFromEbay(222106907586, userToken), JsonRequestBehavior.AllowGet);

            //return Json(_ListingRepository.GetListingsByEndDateFromEbay(new DateTime(2016,03,01), new DateTime(2016,05,20),userToken), JsonRequestBehavior.AllowGet);
            // return Json(_InventoryRepository.CalculateItemProfitByMonth(1), JsonRequestBehavior.AllowGet);

            //return Json(_ListingRepository.GetListingsByStartDateFromEbay(new DateTime(2016,03,01), new DateTime(2016,05,20),userId, userToken), JsonRequestBehavior.AllowGet);

            //return Json(_ListingRepository.GetAllListingsSinceDateFromEbay(new DateTime(2015,03,01)), JsonRequestBehavior.AllowGet);

            //var clientId = ConfigurationManager.AppSettings["clientId"];
            //var ruName = ConfigurationManager.AppSettings["runame"];
            //var responseType = ConfigurationManager.AppSettings["response_type"];
            //var clientSecret = ConfigurationManager.AppSettings["client_secret"];
            //var useOAuth = ConfigurationManager.AppSettings["UseOAuth"];

            //var identity = User.Identity as ClaimsIdentity;
            //var userId = identity.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Select(c => c.Value).FirstOrDefault();

            ViewBag.Title = "About Title";


            //_UserRepository.TestEbayApiStuff(userToken);
            //return Json(_UserRepository.GetSessionId(), JsonRequestBehavior.AllowGet);
            // return Json(_UserRepository.GetSessionId(userId), JsonRequestBehavior.AllowGet);
            //return Json(_UserRepository.GetUserToken(userId,sessionId), JsonRequestBehavior.AllowGet);
            //return Json();

            //return Json(_ListingRepository.GetListingByItemIdFromEbay(222106907586, "AgAAAA**AQAAAA**aAAAAA**9Rc1Vw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AFl4enAJGBpAqdj6x9nY+seQ**JUgDAA**AAMAAA**kpn6ddRezaL9hKw9yeKXpVk4E6vO4fvDpeViYkVIaS0A5J0sP0jAX9ZWfVY34BqCgBgNKdKECco5LmoX1QxWWvD0OaFreyvT5ROo9PepEces++SuKgvAmS2JZBgxdPeAJtOrIZ1sbDSqMvMjxO/733g+iA3VuXw3XLY+y6S9rEGjyVh0AFGvLPvXEDdxnHHbspo4gB1SrHbRPkuz1tVv9KVaPonWw6Pjvrz4+HhRxGOLDmRPpuEZCCNJKj29VuWnLhNKNAzOYo4apXlGbQoTa7y/W6wnI/9Sqb6QfbyYSoqNkQFlZtPgLas4Z6SFhEF86JlhctHzBD+QqYrcE9lU3SJS8TaQcwM18i3PaG0qO2UYcDgygChFtXwSgHKE45q4qpFO16GLvfl3VQoMYasT9xBw65/2f4AWa3s0XZt7zhPo18famPCsTnEr6NvfFLyrCMi3X20faqjvnAlQhtA3RpoLdOLNVjR0ttCBAj08KqXBtEQ31r97ExlU01JBHkwpDvT26SNVvU9h8Pc9bISOskPVYdh31Gfa80XukvZrkL8L3hDJMc7DAK0V4lMQVGHrnJ9cyNVNo/KULkcXRYKkQYGhWdXAR5PuYPATWH7q6FNg4GpdETcgBv7kNdWVm197dCYVFmo4EAB9jA5q1nO26M7Sx3p70ZizFXcX0EqZuB1c7Mhkx2gllApq+2c+IF4SuRfEeRIC6JyxQzx7Yhd4N3wfOan3KfPnXfxXbPqR7MEe89dMHSCLqHSnROhh004Q"),JsonRequestBehavior.AllowGet);

            //return Json(_InventoryRepository.GetBestSellingItem(userId), JsonRequestBehavior.AllowGet);
            //return Json(_InventoryRepository.GetHighestAverageProfitItem(userId), JsonRequestBehavior.AllowGet);
            // return Json(userId, JsonRequestBehavior.AllowGet);

            //return Json(_InventoryBll.GetInventoryItemsByUser(user.Id), JsonRequestBehavior.AllowGet);

            //if (code != null)
            //{
            //    var encodedTempCode = HttpUtility.UrlEncode(code);
            //    ViewBag.Message = PostForm(code);
            //    var response = PostForm(code);

            //    var jsonSerializer = new JavaScriptSerializer();
            //    Dictionary<string, object> responseDictionary = (Dictionary<string, object>)jsonSerializer.DeserializeObject(response);

            //    var authTokenResponse = new OAuthTokenResponse();
                
            //    authTokenResponse.AccessToken = responseDictionary["access_token"].ToString();
            //    authTokenResponse.AccessTokenExpiresIn = (int)(responseDictionary["expires_in"]);
            //    authTokenResponse.TokenType = responseDictionary["token_type"].ToString();
            //    authTokenResponse.RefreshToken = responseDictionary["refresh_token"].ToString();
            //    authTokenResponse.RefreshTokenExpiresIn = (int)responseDictionary["refresh_token_expires_in"];

            //    SetOAuthUserClaim(authTokenResponse);

            //}
            //else if (tempToken != null)
            //{
            //    var sessionId = UserManager.GetClaims(userId).Where(c => c.Type == "SessionId").Select(c => c.Value).FirstOrDefault();
            //    var tokenInfo = _UserRepository.GetUserToken(userId, sessionId);

            //    bool result = _UserRepository.TestUserToken(tokenInfo[0]);


            //    ViewBag.Message = tokenInfo[0] + ", " + tokenInfo[1];
            //}
            //else
            //{
            //    if (useOAuth == "true")
            //    {
            //        var url = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame=Cameron_Blazevi-CameronB-EbayFe-urvcak&oauthparams=%26state%3Dnull%26client_id%3DCameronB-EbayFeeT-PRD-e8a129233-5ff958d9%26redirect_uri%3DCameron_Blazevi-CameronB-EbayFe-urvcak%26response_type%3Dcode%26device_id%3Dnull%26display%3Dnull%26scope%3Dhttps%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.marketing.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.marketing+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.inventory.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.inventory+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.account.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.account+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.fulfillment.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.fulfillment+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.analytics.readonly%26tt%3D1";
            //        return Redirect(url);
            //    }
            //    else
            //    {
            //        string sessionId = _UserRepository.GetSessionId(userId);
            //        sessionId = HttpUtility.UrlEncode(sessionId);

            //        var claimType = "SessionId";
            //        var existingClaim = UserManager.GetClaims(userId).Where(c => c.Type == claimType).FirstOrDefault();
            //        if (existingClaim != null)
            //        {
            //            UserManager.RemoveClaimAsync(userId, existingClaim);
            //        }
            //        UserManager.AddClaimAsync(userId, new Claim(claimType, sessionId));

            //        var url = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame=Cameron_Blazevi-CameronB-EbayFe-urvcak&SessID=" + sessionId;
            //        return Redirect(url);

            //    }
            //}


            return View();
        }

        private void SetOAuthUserClaim(OAuthTokenResponse authTokenResponse)
        {
            var identity = User.Identity as ClaimsIdentity;

            var userId = identity.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Select(c => c.Value).FirstOrDefault();
            var claims = UserManager.GetClaims(userId);

            var claimType = "OAuthToken";

            if(!claims.Where(c => c.Type == claimType).Any())
            {
                UserManager.AddClaimAsync(userId, new Claim(claimType, authTokenResponse.AccessToken));
            }
        }



        private string PostForm(string code)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.ebay.com/identity/v1/oauth2/token");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorization:Basic Q2FtZXJvbkItRWJheUZlZVQtUFJELWU4YTEyOTIzMy01ZmY5NThkOTpQUkQtOGExMjkyMzM4Y2NjLTQ4ZDYtNDM5ZC05MDQxLTI2OTE=");

            string postData = "grant_type=authorization_code&code=" + code + "&redirect_uri=Cameron_Blazevi-CameronB-EbayFe-urvcak";
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
            return result;
        }



        public ActionResult Contact()
        {


            var identity = User.Identity as ClaimsIdentity;
            

            var userId = identity.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Select(c => c.Value).FirstOrDefault();

            var claims = UserManager.GetClaims(userId);

            //UserManager.AddClaimAsync(userId, new Claim("TestClaim", "Test"));


            var claimToRemove = claims.Where(c => c.Type == "TestClaim").FirstOrDefault();

            //UserManager.RemoveClaimAsync(userId, claimToRemove);

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