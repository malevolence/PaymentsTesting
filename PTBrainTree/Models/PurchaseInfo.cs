using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTBrainTree.Models
{
	public class PurchaseInfo
	{
		[DataType(DataType.Currency)]
		public decimal Amount { get; set; }
		public bool SavePaymentMethod { get; set; }
	}
}