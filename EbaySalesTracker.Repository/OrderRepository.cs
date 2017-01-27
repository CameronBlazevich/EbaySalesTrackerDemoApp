using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public class OrderRepository : RepositoryBase<EbaySalesTrackerContext>, IOrderRepository
    {
        IOrderEngine _OrderEngine;
        public OrderRepository(): this(null) {}
        public OrderRepository(IOrderEngine orderEngine)
        {
            _OrderEngine = orderEngine ?? new OrderEngine();
        }
        public Order GetOrderByOrderIdFromEbay(long listingId, string orderId, string userId)
        {
            ApplicationUser user = new ApplicationUser();
            var userContext = new ApplicationDbContext();
            user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();
            
            var order = _OrderEngine.GetOrderByOrderIdFromEbay(listingId, orderId, user.UserToken);
            if (order.OrderId != null)
            {
                
                    DataContext.Orders.AddOrUpdate(order);
                    DataContext.SaveChanges();
                
            }
            return order;


        }
    }
}
