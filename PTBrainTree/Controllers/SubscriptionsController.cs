using Braintree;
using PTBrainTree.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTBrainTree.Controllers
{
    public class SubscriptionsController : Controller
    {
		private BraintreeGateway gateway;

		public SubscriptionsController()
		{
			this.gateway = Common.GetGateway();
		}

        // GET: Subscriptions
        public ActionResult Index()
        {
			var request = new SubscriptionSearchRequest();
			ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

			return View(collection);
        }

		public ActionResult Details(string id)
		{
			var result = gateway.Subscription.Find(id);

			return View(result);
		}

		[HttpPost]
		public ActionResult Cancel(string id)
		{
			var result = gateway.Subscription.Cancel(id);

			if (result.IsSuccess())
				TempData["success"] = "Subscription cancelled";
			else
				TempData["error"] = "Problem cancelling subscription. Error: " + result.Message;

			return RedirectToAction("Index");
		}
    }
}