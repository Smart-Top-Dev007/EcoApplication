using System;
using MassTransit;

namespace EcoCentre.Models.Domain.Invoices.Events
{
	public class InvoiceAddedEvent
	{
		public string InvoiceId { get; set; }
	}
}