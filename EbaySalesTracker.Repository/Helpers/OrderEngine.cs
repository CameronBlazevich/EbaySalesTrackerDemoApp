using eBay.Service.Call;
using eBay.Service.Core.Soap;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository.Helpers
{
    public class OrderEngine : Engine, IOrderEngine
    {
        public Order GetOrderByOrderIdFromEbay(long listingId, string orderId, string userToken)
        {
            var context = RequestBuilder.CreateNewApiCall(userToken);
            GetOrdersCall getOrdersCall = new GetOrdersCall(context);
            getOrdersCall.OrderIDList = new StringCollection() { orderId };
            getOrdersCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            getOrdersCall.Execute();
            if (getOrdersCall.ApiResponse.OrderArray.Count > 0)
            {
                var mappedOrder = MapResultToOrder(getOrdersCall.ApiResponse.OrderArray[0]);                
                return mappedOrder;
                }
            //need to have plan for when no order is returned from ebay
            return new Order();
        }

        public IEnumerable<Order> GetOrdersByModTimeFromEbay(DateTime modTimeFrom, DateTime modTimeTo, string userToken)
        {
            var context = RequestBuilder.CreateNewApiCall(userToken);
            var getOrdersCall = new GetOrdersCall(context);
            getOrdersCall.ModTimeFrom = modTimeFrom;
            getOrdersCall.ModTimeTo = modTimeTo;
            getOrdersCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            //getOrdersCall.OutputSelector(new string[] { "OrderArray" })
            getOrdersCall.Execute();
            var orders = new List<Order>();           
            if (getOrdersCall.ApiResponse.Ack == AckCodeType.Success)
            {
                var ordersFromCall = getOrdersCall.ApiResponse.OrderArray;
                if (ordersFromCall.Count > 0)
                {
                    
                    foreach (OrderType orderFromCall in ordersFromCall)
                    {
                        orders.Add(MapResultToOrder(orderFromCall));
                    }

                }
            }
            return orders;
        }

        private Order MapResultToOrder(OrderType result)
        {
            var orderId = result.OrderID;

            var order = new Order();
            order.OrderId = result.OrderID;
            order.OrderStatus = result.OrderStatus;
            order.SalesPrice = result.Subtotal.Value;
            order.TotalCost = result.Total.Value;            
            if (result.MonetaryDetails?.Refunds?.Refund?.Count > 0)
            {
                order.RefundAmount = result.MonetaryDetails.Refunds.Refund[0].RefundAmount.Value;
                order.RefundStatus = result.MonetaryDetails.Refunds.Refund[0].RefundStatus;
                order.RefundTime = result.MonetaryDetails.Refunds.Refund[0].RefundTime;

            }

            if (result.PaidTime == DateTime.MinValue)
            {
                order.PaidTime = null;
            }
            else {
                order.PaidTime = result.PaidTime;
            }

            if (result.ShippedTime == DateTime.MinValue)
            {
                order.ShippedTime = null;
            }
            else {
                order.ShippedTime = result.ShippedTime;
            }

            var transactionsFromCall = result.TransactionArray;
            if (transactionsFromCall.Count > 0)
            {
                var listingTransEngine = new ListingTransactionEngine();
                var transactions = new List<ListingTransaction>();
                foreach (TransactionType transFromCall in transactionsFromCall)
                {
                    transactions.Add(listingTransEngine.MapResultToListingTransaction(transFromCall));
                }
                order.Transactions = transactions;
            }
            
            return order;
        }
    }
}
