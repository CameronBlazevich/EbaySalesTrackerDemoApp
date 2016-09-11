using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public interface IOrderRepository
    {
        Order GetOrderByOrderIdFromEbay(string orderId, string userId);
    }
}
