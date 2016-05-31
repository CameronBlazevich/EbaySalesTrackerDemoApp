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

    }
}
