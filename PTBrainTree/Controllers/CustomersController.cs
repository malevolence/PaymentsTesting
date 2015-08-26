using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Braintree;
using PTBrainTree.Utilities;
using Braintree.Exceptions;

namespace PTBrainTree.Controllers
{
    public class CustomersController : Controller
    {
		private BraintreeGateway gateway;

		public CustomersController()
		{
			this.gateway = Common.GetGateway();
		}

        // GET: Customers
        public ActionResult Index()
        {
			var customers = gateway.Customer.All();
            return View(customers);
        }

		public ActionResult Details(string id)
		{
			try
			{
				var customer = gateway.Customer.Find(id);
				ViewBag.Token = Common.GetGateway().ClientToken.generate();
				return View(customer);
			}
			catch (NotFoundException)
			{
				TempData["error"] = "No customer found with Id = " + id;
			}
			catch (Exception exc)
			{
				TempData["error"] = exc.Message;
			}

			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(CustomerRequest customerRequest)
		{
			if (ModelState.IsValid)
			{
				var result = gateway.Customer.Create(customerRequest);
				if (result.IsSuccess())
				{
					TempData["success"] = "New customer saved successfully";
					return RedirectToAction("Index");
				}
				else
				{
					TempData["error"] = "Failed to save new customer. Message: " + result.Message;
				}
			}

			return View(customerRequest);
		}

		public ActionResult Edit(string id)
		{
			try
			{
				var customer = gateway.Customer.Find(id);
				return View(customer);
			}
			catch (NotFoundException)
			{
				TempData["error"] = "No customer found with Id = " + id;
			}
			catch (Exception exc)
			{
				TempData["error"] = exc.Message;
			}

			return View();
		}

		[HttpPost]
		public ActionResult Edit(string id, CustomerRequest request)
		{
			try
			{
				var result = gateway.Customer.Update(id, request);

				if (result.IsSuccess())
					TempData["success"] = "Customer updated successfully.";
				else
					TempData["error"] = "Customer not updated. Error: " + result.Message;
			}
			catch (NotFoundException)
			{
				TempData["error"] = "No customer found with Id = " + id;
			}
			catch (Exception exc)
			{
				TempData["error"] = exc.Message;
			}

			return RedirectToAction("Edit", new { id });
		}

		[HttpPost]
		public ActionResult AddPaymentMethod(string id, string payment_method_nonce)
		{
			var request = new PaymentMethodRequest
			{
				CustomerId = id,
				PaymentMethodNonce = payment_method_nonce
			};

			var result = gateway.PaymentMethod.Create(request);
			if (result.IsSuccess())
				TempData["success"] = "New payment method saved successfully";
			else
				TempData["error"] = "Payment method not saved. Error: " + result.Message;

			return RedirectToAction("Details", new { id });
		}

		[HttpPost]
		public ActionResult Charge(string id, string token, decimal amount)
		{
			var request = new TransactionRequest
			{
				Amount = amount,
				PaymentMethodToken = token,
				CustomerId = id,
				Options = new TransactionOptionsRequest
				{
					SubmitForSettlement = true
				}
			};

			var result = gateway.Transaction.Sale(request);

			if (result.IsSuccess())
				TempData["success"] = string.Format("Charge completed for {0:c}", amount);
			else
				TempData["error"] = "There was a problem completing the transaction. Error: " + result.Message;

			return RedirectToAction("Details", new { id });
		}

		[HttpPost]
		public ActionResult Signup(string token, string plan)
		{
			var request = new SubscriptionRequest
			{
				PaymentMethodToken = token,
				PlanId = plan
			};

			var result = gateway.Subscription.Create(request);

			if (result.IsSuccess())
				TempData["success"] = "Subscription created successfully";
			else
				TempData["error"] = "Failed to create subscription. Error: " + result.Message;

			return RedirectToAction("Index");
		}    }
}