using System;
using EbaySalesTracker.Models;
using System.Linq;

namespace EbaySalesTracker.Repository
{
    public class WebHookRepository : RepositoryBase<EbaySalesTrackerContext>, IWebHookRepository
    {
        

        public void LogWebHookEvent(WebhookLog stripeEvent)
        {
                DataContext.WebHookLogs.Add(stripeEvent);
                DataContext.SaveChanges();
        }
    }
}
