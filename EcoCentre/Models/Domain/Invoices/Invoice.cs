using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using EcoCentre.Models.Domain.Payments;

namespace EcoCentre.Models.Domain.Invoices
{
	[BsonIgnoreExtraElements]
	public class Invoice : Entity
	{
		public Invoice()
		{
			Materials = new List<InvoiceMaterial>();
			Attachments = new List<InvoiceAttachment>();
			GiveawayItems = new List<InvoiceGiveawayItem>();
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string ClientId { get; set; }
		public int SequentialNo { get; set; }
		public string EmployeeName { get; set; }
		public DateTime CreatedAt { get; set; }
		public IList<InvoiceMaterial> Materials { get; set; }
		public IList<InvoiceAttachment> Attachments { get; set; }
		public IList<InvoiceGiveawayItem> GiveawayItems { get; set; }

		public ClientAddress Address { get; set; }

		public void AddAttachment(string id, string name)
		{
			Attachments.Add(new InvoiceAttachment
			{
				Id = id,
				Name = name
			});

		}
		public string Comment { get; set; }

		public CenterIdentification Center { get; set; }
		
		public string InvoiceNo { get; set; }

		public InvoiceCreator CreatedBy { get; set; }
		public IList<Tax> Taxes { get; set; }
		public decimal Amount { get; set; }
		public decimal AmountIncludingTaxes { get; set; }
		public int? VisitLimit { get; set; }
		public int VisitNumber { get; set; }
		public Payment Payment { get; set; }

		public static Invoice Import(Client client, DateTime createdAt, int invoiceId)
		{
			if (client == null) throw new ArgumentNullException(nameof(client));
			var result = new Invoice
			{
				ClientId = client.Id,
				CreatedAt = createdAt,
				SequentialNo = invoiceId
			};
			return result;
		}

		public void AddMaterial(Material material, decimal qty, double weight)
		{
			var im = new InvoiceMaterial
			{
				MaterialId = material.Id,
				Quantity = qty,
				Weight = weight
			};
			Materials.Add(im);
		}

	}

	public class InvoiceGiveawayItem
	{
		public decimal Price { get; set; }
		public string Title { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
	}
}