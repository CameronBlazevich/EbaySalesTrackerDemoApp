using eBay.Service.Core.Sdk;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserEngine engine = new UserEngine();
        public string GetSessionId(string userId)
        {
            string sessionId = engine.GetSessionId();
            if (sessionId == null) return null;

            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.Where(p => p.Id == userId).FirstOrDefault();
                user.SessionId = sessionId;
                context.SaveChanges();
            }

            return sessionId;
            
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

    }
}
