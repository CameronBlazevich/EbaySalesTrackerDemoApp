using eBay.Service.Core.Sdk;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public class UserRepository : IUserRepository
    {
        string sessionId = "";
        private UserEngine engine = new UserEngine();
        public string GetSessionId(string userId)
        {
            using (var userContext = new ApplicationDbContext())
            {
                //sessionId = userContext.Users.Where(x => x.Id == userId).Select(u => u.SessionId).FirstOrDefault();

                //if (sessionId == "" || sessionId == null)
                //{
                    sessionId = engine.GetSessionId();
                    //if (sessionId == null) return null;

                    //var user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();
                    //user.SessionId = sessionId;
                    //userContext.SaveChanges();
                //}                
            }

            return sessionId;
            
        }

        public ApplicationUser GetUserByStripeId(string stripeUserId)
        {
            using (var context = new ApplicationDbContext())
            { 
                return context.Users.Where(u => u.StripeCustomerId == stripeUserId).FirstOrDefault();
            }
        }

        public List<string> GetUserToken(string userId, string sessionId)
        {
            var result = engine.GetUserToken(sessionId);
            if (result == null) return null;

            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.Where(p => p.Id == userId).FirstOrDefault();
                user.UserToken = result[0];
                user.TokenExpDate = Convert.ToDateTime(result[1]);
                context.SaveChanges();
            }

            return result;
        }
        public bool TestUserToken(string userToken)
        {
           return engine.GetEbayOfficialTime(userToken);
        }

        public void SetNewActiveUntilDate(ApplicationUser user, DateTime newEndDate)
        {
            using (var context = new ApplicationDbContext())
            {
                user.StripeActiveUntil = newEndDate;
                context.Users.AddOrUpdate(user);
                context.SaveChanges();
            }
        }

        public void SetUserCancelReason(ApplicationUser user, string cancellationReason)
        {
            using (var context = new ApplicationDbContext())
            {
                user.CancelReason = cancellationReason;
                context.Users.AddOrUpdate(user);
                context.SaveChanges();
            }
        }
    }
}
