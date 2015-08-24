using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTBrainTree.Utilities
{
	public static class Common
	{
		public static BraintreeGateway GetGateway()
		{
			return new BraintreeGateway
			{
				Environment = Braintree.Environment.SANDBOX,
				MerchantId = "8fjbtryc9n3srp9j",
				PublicKey = "py5gvkf5q64f27n5",
				PrivateKey = "7cc5dbb6ab2aed59144989b133fa6859"
			};
		}
	}
}