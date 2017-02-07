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

        public IEnumerable<Order> GetAllOrdersSinceDateFromEbay(DateTime startDate,string userId)
        {
            List<Order> orders = new List<Order>();
            DateTime startDateFrom = startDate;
            DateTime startDateTo = startDate.AddDays(29);

            for (DateTime start = startDateFrom; start <= DateTime.Now; start = start.AddDays(29))
            {
                orders = orders.Concat(GetOrdersByModTimeFromEbay(start, startDateTo, userId)).ToList();
                startDateTo = startDateTo.AddDays(29);
            }
            return orders;
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

        public IEnumerable<Order> GetOrdersByModTimeFromEbay(DateTime modTimeFrom, DateTime modTimeTo, string userId)
        {
            var userContext = new ApplicationDbContext();
            var user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();

            var orders = _OrderEngine.GetOrdersByModTimeFromEbay(modTimeFrom, modTimeTo, user.UserToken);
            foreach (var order in orders)
            {
                DataContext.Orders.AddOrUpdate(order);
                if (order.Transactions != null && order.Transactions.Count > 0)
                {
                    foreach (var trans in order.Transactions)
                    {
                        DataContext.ListingTransactions.AddOrUpdate(trans);
                    }
                }
            }
            DataContext.SaveChanges();
            return orders;
        }

    }
}
