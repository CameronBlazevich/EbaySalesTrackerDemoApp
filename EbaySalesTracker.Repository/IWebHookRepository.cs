using EbaySalesTracker.Models;

namespace EbaySalesTracker.Repository
{
    public interface IWebHookRepository
    {
        void LogWebHookEvent(WebhookLog stripeEvent);
        
    }
}
