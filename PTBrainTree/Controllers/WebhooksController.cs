using Braintree;
using PTBrainTree.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTBrainTree.Controllers
{
    public class WebhooksController : Controller
    {
		private readonly BraintreeGateway gateway;

		public WebhooksController()
		{
			this.gateway = Common.GetGateway();
		}

        // Combined
        public ActionResult Subscriptions()
        {
			if (Request.HttpMethod == "POST")
			{
				// parse the data
				// save the signature and payload for reference - WebHookNotifications table
				var notification = gateway.WebhookNotification.Parse(Request.Form["bt_signature"], Request.Form["bt_payload"]);

				string message = string.Format("[Webhook Received {0}] | Kind: {1} | Subscription: {2}", notification.Timestamp.Value, notification.Kind, notification.Subscription.Id);

				Console.WriteLine(message);
				return new HttpStatusCodeResult(200);
			}
			else
			{
				return Content(gateway.WebhookNotification.Verify(Request.QueryString["bt_challenge"]));
			}
        }

		[HttpGet]
		public ActionResult Subscriptions(string bt_challenge)
		{
			return Content(gateway.WebhookNotification.Verify(bt_challenge));
		}

		[HttpPost]
		public ActionResult Subscriptions(string bt_signature, string bt_payload)
		{
			// parse the data
			// save the signature and payload for reference - WebHookNotifications table
			var notification = gateway.WebhookNotification.Parse(bt_signature, bt_payload);

			// do whatever with this info
			string message = string.Format("[Webhook Received {0}] | Kind: {1} | Subscription: {2}", notification.Timestamp.Value, notification.Kind, notification.Subscription.Id);

			System.Console.WriteLine(message);
			return new HttpStatusCodeResult(200);
		}

		public ActionResult Test()
		{
			var sample = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_CANCELED, "4fmt4b");
			var notification = gateway.WebhookNotification.Parse(sample["bt_signature"], sample["bt_payload"]);

			return View(notification);
		}
    }
}