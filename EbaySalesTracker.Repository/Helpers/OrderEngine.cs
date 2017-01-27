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
        public Order GetOrderByOrderIdFromEbay(long listingId,string orderId, string userToken)
        {
            var context = RequestBuilder.CreateNewApiCall(userToken);
            GetOrdersCall getOrdersCall = new GetOrdersCall(context);
            getOrdersCall.OrderIDList = new StringCollection() { orderId };
            getOrdersCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            getOrdersCall.Execute();
            if (getOrdersCall.ApiResponse.OrderArray.Count > 0)
                return MapResultToOrder(getOrdersCall.ApiResponse.OrderArray[0], listingId);
            //need to have plan for when no order is returned from ebay
            return new Order();
        }
        private Order MapResultToOrder(OrderType result, long listingId)
        {
            var orderId = result.OrderID;
            string[] orderIdSplit = orderId.Split('-');

            var order = new Order();
            order.OrderId = result.OrderID;
            order.OrderStatus = result.OrderStatus;
            order.SalesPrice = result.Subtotal.Value;
            order.TotalCost = result.Total.Value;
            order.ListingId = listingId;
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

            double? totalTaxAmount = result.TransactionArray[0]?.Taxes?.TotalTaxAmount?.Value;
            double? shipping = result.TransactionArray[0]?.ActualShippingCost?.Value;
            double? handling = result.TransactionArray[0]?.ActualHandlingCost?.Value;

            order.TotalTaxAmount = totalTaxAmount == null ?  0 : result.TransactionArray[0].Taxes.TotalTaxAmount.Value;
            order.Shipping = shipping == null ? 0 : result.TransactionArray[0].ActualShippingCost.Value;
            order.Handling = handling == null ? 0 : result.TransactionArray[0].ActualHandlingCost.Value;
            

            return order;
        }
    }
}
