using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Stripe;
using Microsoft.AspNet.Identity;
using EbaySalesTracker.Models;
using EbaySalesTracker.Repository;
using Microsoft.Practices.Unity;

namespace EbaySalesTracker.Controllers
{
    public class StripeWebhookController : Controller
    {
        IWebHookRepository _WebHookRepository;
        IUserRepository _UserRepository;
        public StripeWebhookController() : this(null,null)
        {

        }
        public StripeWebhookController(IWebHookRepository webHookRepo, IUserRepository userRepo)
        {
            _WebHookRepository = webHookRepo ?? ModelContainer.Instance.Resolve<IWebHookRepository>();
            _UserRepository = userRepo ?? ModelContainer.Instance.Resolve<IUserRepository>();
        }

        [HttpPost]
        public ActionResult Index()
        {
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            StripeEvent stripeEvent;
            try
            {
                stripeEvent = StripeEventUtility.ParseEvent(json);
                //Comment this out when using test webhooks -> they dont have real ids
                stripeEvent = VerifyEventSentFromStripe(stripeEvent);
                if (HasEventBeenProcessPreviously(stripeEvent))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Unable to parse incoming event. The following error occurred: {0}", ex.Message));
            }
            if (stripeEvent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Incoming event empty");
            }

            switch (stripeEvent.Type)
            {
                case StripeEvents.CustomerCreated:
                    //HandleCustomerCreatedEvent(stripeEvent);
                    break;
                case StripeEvents.CustomerSubscriptionTrialWillEnd:
                    HandleSubscriptionTrialWillEndEvent(stripeEvent);
                    break;
                case StripeEvents.InvoicePaymentSucceeded:
                    HandleInvoicePaymentSucceededEvent(stripeEvent);
                    break;
                case StripeEvents.InvoicePaymentFailed:
                    HandleInvoicePaymentFailedEvent(stripeEvent);
                    break;
                case StripeEvents.CustomerSubscriptionDeleted:
                    HandleSubscriptionCancellactionEvent(stripeEvent);
                    break;
                case StripeEvents.CustomerSubscriptionUpdated:
                    HandleSubscriptionUpdated(stripeEvent);
                    break;
                case StripeEvents.ChargeRefunded:
                default:
                    break;
            }

            LogEvent(stripeEvent);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void HandleSubscriptionUpdated(StripeEvent stripeEvent)
        {
            if (stripeEvent?.Data?.Object?.cancel_at_period_end?.Value != null)
            {
                if (stripeEvent.Data.Object.cancel_at_period_end.Value)
                { 
                HandleSubscriptionCancellactionEvent(stripeEvent);
                }
                else
                {
                    HandleSubscriptionReactivationEvent(stripeEvent);
                }
            }
        }



        private bool HasEventBeenProcessPreviously(StripeEvent stripeEvent)
        {
           return  _WebHookRepository.HasBeenProcessed(stripeEvent.Id);           
        }

        private static StripeEvent VerifyEventSentFromStripe(StripeEvent stripeEvent)
        {
            var eventService = new StripeEventService();
            stripeEvent = eventService.Get(stripeEvent.Id);
            return stripeEvent;
        }

        public void HandleSubscriptionTrialWillEndEvent(StripeEvent stripeEvent)
        {
            var user = GetUserByStripeId(stripeEvent.Data.Object.customer.Value);
            var toAddress = user.Email;
            var subject = "Your trial will end soon";
            var body = "trial ending soon";
            SendEmail(toAddress, subject, body);
        }

        public void HandleInvoicePaymentSucceededEvent(StripeEvent stripeEvent)
        {
            var user = GetUserByStripeId(stripeEvent.Data.Object.customer.Value);

            var newEndDate = ((DateTime)user.StripeActiveUntil).AddMonths(1);
            _UserRepository.SetNewActiveUntilDate(user, newEndDate);
            
            var toAddress = user.Email;
            var subject = "Payment processed successfully.";
            var body = "Payment processed on: " + stripeEvent.Created;

            SendEmail(toAddress, subject, body);
        }

        public void HandleInvoicePaymentFailedEvent(StripeEvent stripeEvent)
        {
            var user = GetUserByStripeId(stripeEvent.Data.Object.customer.Value);

            var newEndDate = ((DateTime)user.StripeActiveUntil).AddDays(-1);
            _UserRepository.SetNewActiveUntilDate(user, newEndDate);

            var toAddress = user.Email;
            var subject = "Your subscription payment failed.";
            var body = "Payment attempted on: " + stripeEvent.Created + " did not go through, and your account has been suspended. Please contact support.";

            SendEmail(toAddress, subject, body);
        }
        private void HandleSubscriptionCancellactionEvent(StripeEvent stripeEvent)
        {
            var user = GetUserByStripeId(stripeEvent.Data.Object.customer.Value);
            var toAddress = user.Email;
            var subject = "Your eProfitTracker subscription has been cancelled.";
            var body = "At your request, your eProfitTracker subscription has been cancelled as of " +
                stripeEvent.Created + ". You can reactivate your subscription at any time.";

            SendEmail(toAddress, subject, body);
        }

        private void HandleSubscriptionReactivationEvent(StripeEvent stripeEvent)
        {
            ApplicationUser user = GetUserByStripeId(stripeEvent.Data.Object.customer.Value);
                      
            var toAddress = user.Email;
            var subject = "Your eProfitTracker Subscription has been Reactivated";
            var body = "At your request, your eProfitTracker subscription has been reactivated.";
            SendEmail(toAddress, subject, body);
        }
        private void SendEmail(string toAddress, string subject, string body)
        {
            var mailService = new EmailService();
            var message = new IdentityMessage()
            {
                Destination = toAddress,
                Subject = subject,
                Body = body
            };
            mailService.Send(message);
        }

        private ApplicationUser GetUserByStripeId(string stripeUserId)
        {
            return _UserRepository.GetUserByStripeId(stripeUserId);
        }

        private void LogEvent(StripeEvent stripeEvent)
        {
            var eventToLog = new WebhookLog();
            eventToLog.CreatedDate = stripeEvent.Created;
            eventToLog.StripeUserId = stripeEvent?.Data?.Object?.customer?.Value;
            eventToLog.EventType = stripeEvent.Type;
            eventToLog.EventId = stripeEvent.Id;
            eventToLog.LoggedDate = DateTime.Now;

            _WebHookRepository.LogWebHookEvent(eventToLog);

        }
    }
}