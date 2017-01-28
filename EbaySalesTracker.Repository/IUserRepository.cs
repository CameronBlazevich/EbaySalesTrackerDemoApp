using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public interface IUserRepository
    {
        string GetSessionId(string userId);
        List<string> GetUserToken(string userId, string sessionId);
        bool TestUserToken(string userToken);
        ApplicationUser GetUserByStripeId(string stripeUserId);
        void SetNewActiveUntilDate(ApplicationUser user, DateTime newEndDate);
        void CancelUser(ApplicationUser user, string cancellationReason);
        void ReactivateUser(ApplicationUser user);
    }
}
