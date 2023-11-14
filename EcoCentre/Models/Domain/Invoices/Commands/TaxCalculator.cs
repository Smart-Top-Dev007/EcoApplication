using System;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
	public static class TaxCalculator
	{
		public static decimal GetAmount(decimal quantity, decimal price)
		{
			return Math.Round(price * quantity, 2);
		}

		public static decimal GetAmountIncludingTaxes(decimal quantity, decimal price, decimal taxRate)
		{
			return Math.Round(price * quantity * (1 + taxRate / 100), 2);
		}
		public static decimal GetTaxAmount(decimal amount, decimal taxRate)
		{
			return Math.Round(amount * taxRate / 100, 2);
		}
	}
}