using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Materials;
using NLog;

namespace EcoCentre.Models.Domain.Limits
{
	using MongoDB.Bson.Serialization.Attributes;

	[BsonIgnoreExtraElements]
	public class LimitStatus : Entity
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public LimitStatus()
		{
			Address = new ClientAddress();
			Limits = new List<LimitStatusYear>();
		}
		public ClientAddress Address { get; set; }
		public IList<LimitStatusYear> Limits { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public void UpdateLimits(Invoice invoice, IList<Material> materialDetails)
		{
			var year = Limits.FirstOrDefault(x => x.Year == invoice.CreatedAt.Year);
			if (year == null)
			{
				year = new LimitStatusYear
				{
					Materials = new List<LimitStatusMaterial>(),
					Year = invoice.CreatedAt.Year
				};
				Limits.Add(year);
			}
			foreach (var material in invoice.Materials)
			{
				var md = materialDetails.FirstOrDefault(x => x.Id == material.MaterialId);
				if (md == null)
				{
					Logger.Error( "Can't find material {materialId} for invoice {invoiceId}", material.MaterialId, invoice.InvoiceNo);
					continue;
				}
				var limit = year.Materials.FirstOrDefault(x => x.MaterialId == md.Id);
				if (limit == null)
				{
					limit = new LimitStatusMaterial
					{
						MaterialId = md.Id,
						MaterialName = md.Name,
						MaxQuantity = md.MaxYearlyAmount,
						QuantitySoFar = 0,
						IsActive = md.Active,
						Unit = md.Unit,
						IsExcluded = md.IsExcluded
					};

					year.Materials.Add(limit);
				}
				limit.QuantitySoFar += material.Quantity;
			}
			if (UpdatedAt == null || UpdatedAt < invoice.CreatedAt)
				UpdatedAt = invoice.CreatedAt;
		}

		public void RemoveFromLimits(Invoice invoice)
		{
			var year = Limits.FirstOrDefault(x => x.Year == invoice.CreatedAt.Year);
			if (year == null)
			{

				Logger.Error("Can't find limits for year {invoiceYear} for invoice {invoiceId}", invoice.CreatedAt.Year, invoice.InvoiceNo);
				return;
			}

			foreach (var material in invoice.Materials)
			{
				var limit = year.Materials.FirstOrDefault(x => x.MaterialId == material.MaterialId);
				if (limit == null)
				{
					
					var msg = $"Can't find limits for year {invoice.CreatedAt.Year} " +
					          $"for invoice {invoice.InvoiceNo} material {material.MaterialId}";
					Logger.Error(msg);
					return;
				}

				limit.QuantitySoFar -= material.Quantity;

				if (limit.QuantitySoFar == 0)
				{
					year.Materials.Remove(limit);
				}
			}
			

			if (UpdatedAt == null || UpdatedAt < invoice.CreatedAt)
			{
				UpdatedAt = invoice.CreatedAt;
			}
		}

		public void Clear(int year)
		{
			var limit = Limits.FirstOrDefault(x => x.Year == year);
			if (limit != null)
			{
				limit.Materials = new List<LimitStatusMaterial>();
			}
		}
	}
}