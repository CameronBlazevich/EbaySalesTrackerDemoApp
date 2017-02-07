using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository.Helpers
{
    public interface IOrderEngine
    {
        Order GetOrderByOrderIdFromEbay(long listingId,string orderId, string userToken);
        IEnumerable<Order> GetOrdersByModTimeFromEbay(DateTime modTimeFrom, DateTime modTimeTo, string userToken);
    }
}
