using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Payments;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
	public class InvoiceDetails
	{
		public InvoiceDetails(Invoice invoice, Client client)
		{
			Id = invoice.Id;
			CreatedBy = invoice.CreatedBy;
			InvoiceNo = invoice.InvoiceNo;
			CreatedAt = invoice.CreatedAt.ToLocalTime();
			Client = client;
			Comment = invoice.Comment;
			Address = invoice.Address;
			Center = invoice.Center;

			Taxes = invoice.Taxes
				.EmptyIfNull()
				.Select(x => new InvoiceTax
				{
					Name = x.Name,
					Amount = x.Amount
				})
				.ToList();
			
			Materials = new List<InvoiceDetailsMaterial>();
			Attachments = new List<InvoiceDetailsAttachment>();
			VisitLimit = invoice.VisitLimit;
			VisitNumber = invoice.VisitNumber;
			Amount = invoice.Amount;
			AmountIncludingTaxes = invoice.AmountIncludingTaxes;
			GiveawayItems = invoice.GiveawayItems;
			Payment = invoice.Payment;
		}


		public int VisitNumber { get; set; }
		public int? VisitLimit { get; set; }
		public IList<InvoiceTax> Taxes { get; set; }
		public InvoiceCreator CreatedBy { get; set; }
		public ClientAddress Address { get; set; }
		public CenterIdentification Center { get; set; }
		public string Id { get; set; }
		public string InvoiceNo { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Comment { get; set; }
		public Client Client { get; set; }
		public bool IsOBNL { get; set; }
		public IList<InvoiceDetailsMaterial> Materials { get; set; }
		public IList<InvoiceDetailsAttachment> Attachments { get; set; }
		public IList<InvoiceGiveawayItem> GiveawayItems { get; set; }
		public Payment Payment { get; set; }
		public bool IsExcluded { get; set; }
		public decimal Amount { get; set; }
		public decimal AmountIncludingTaxes { get; set; }

		public void AddAttachment(string id, string name)
		{
			Attachments.Add(new InvoiceDetailsAttachment
			{
				Id = id,
				Name = name
			});
		}
	}
}