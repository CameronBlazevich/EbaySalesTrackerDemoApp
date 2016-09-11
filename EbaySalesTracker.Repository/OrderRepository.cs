using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System;
using System.Collections.Generic;
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
        public Order GetOrderByOrderIdFromEbay(string orderId, string userId)
        {
            ApplicationUser user = new ApplicationUser();
            var userContext = new ApplicationDbContext();
            user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();
            
            var order = _OrderEngine.GetOrderByOrderIdFromEbay(orderId, user.UserToken);
            if (order.OrderId != null)
            {
                var existingOrder = DataContext.Orders.SingleOrDefault(o => o.OrderId == order.OrderId);
                if (existingOrder != null)
                {
                    //SHOULD NOT LEAVE THIS LIKE THIS, need to use a delta or something to see if anything changed
                    existingOrder.OrderStatus = order.OrderStatus;
                    existingOrder.SalesPrice =  order.SalesPrice;
                    existingOrder.TotalCost = order.TotalCost;
                    existingOrder.RefundAmount = order.RefundAmount;
                    existingOrder.RefundStatus = order.RefundStatus;
                    existingOrder.RefundTime = order.RefundTime;
                    existingOrder.PaidTime = order.PaidTime;
                    existingOrder.ShippedTime = order.ShippedTime;
                    existingOrder.TotalTaxAmount = order.TotalTaxAmount;
                    existingOrder.Shipping = order.Shipping;
                    existingOrder.Handling = order.Handling;
                                        
                    DataContext.SaveChanges();
                }
                else
                {
                    DataContext.Orders.Add(order);
                    DataContext.SaveChanges();
                }
            }
            return order;


        }
    }
}
