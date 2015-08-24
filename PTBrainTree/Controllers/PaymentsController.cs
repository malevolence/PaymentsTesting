using Braintree;
using PTBrainTree.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTBrainTree.Controllers
{
    public class PaymentsController : Controller
    {
        // GET: Payments
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Create()
		{
			ViewBag.Token = Common.GetGateway().ClientToken.generate();
			return View();
		}

		[HttpPost]
		public ActionResult Create(decimal amount, string payment_method_nonce)
		{
			if (amount > 0)
			{
				TempData["info"] = payment_method_nonce;

				var request = new TransactionRequest
				{
					Amount = amount,
					PaymentMethodNonce = payment_method_nonce
				};

				var result = Common.GetGateway().Transaction.Sale(request);
				if (result.IsSuccess())
				{
					TempData["success"] = "Sale completed";
				}
				else
				{
					TempData["error"] = result.Message;
				}


				return RedirectToAction("Index");
			}

			return View(amount);
		}
    }
}