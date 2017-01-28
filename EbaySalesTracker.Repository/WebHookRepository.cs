using System;
using EbaySalesTracker.Models;
using System.Linq;

namespace EbaySalesTracker.Repository
{
    public class WebHookRepository : RepositoryBase<EbaySalesTrackerContext>, IWebHookRepository
    {
        public bool HasBeenProcessed(string id)
        {
            return DataContext.WebHookLogs.Where(wh => wh.EventId == id).Any();
        }

        public void LogWebHookEvent(WebhookLog stripeEvent)
        {
                DataContext.WebHookLogs.Add(stripeEvent);
                DataContext.SaveChanges();
        }
    }
}
