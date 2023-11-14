using System;
using System.Linq;
using System.ServiceModel.Configuration;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
	public class JournalReportQuery
	{
		private readonly Repository<InvoiceJournal> _journalRepository;
		private readonly Repository<Invoice> _invoiceRepository;
		private const int PageSize = 20;

		public JournalReportQuery(Repository<InvoiceJournal> journalRepository,
			Repository<Invoice> invoiceRepository)
		{
			_journalRepository = journalRepository;
			_invoiceRepository = invoiceRepository;
		}

		public JournalReportQueryResult Execute(JournalReportQueryParams @params)
		{

			var query = _journalRepository.Query;

			if (@params.From.HasValue)
				query = query.Where(x => x.InvoiceDate >= @params.From.Value);

			if (@params.To.HasValue)
				query = query.Where(x => x.InvoiceDate <= @params.To.Value.AddDays(1));

			if (!string.IsNullOrEmpty(@params.City) && @params.City.Length == 24)
				query = query.Where(x => x.CityId == @params.City);

			if (!string.IsNullOrEmpty(@params.Material))
				query = query.Where(x => x.Materials.Any(m => m.NameLower.Contains(@params.Material.ToLower())));

			if (!string.IsNullOrEmpty(@params.HubId))
				query = query.Where(x => x.HubId == @params.HubId);

			var count = query.Count();

			//summary
			
			var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
			var pageSize = @params.PageSize ?? PageSize;

			if (skip < 0) skip = 0;
			if (!@params.Xls && pageSize > 0)
			{
				query = query.Skip(skip).Take(pageSize);
			}
			switch (@params.OrderBy)
			{
				case JournalReportSortBy.City:
					query = (@params.OrderDir == SortDir.Asc
								 ? query.OrderBy(x => x.City)
								 : query.OrderByDescending(x => x.City))
						.ThenBy(x => x.ClientLastName).ThenBy(x => x.ClientFirstName);
					break;
				case JournalReportSortBy.ClientName:
					query = (@params.OrderDir == SortDir.Asc
								 ? query.OrderBy(x => x.ClientLastName)
								 : query.OrderByDescending(x => x.ClientLastName))
						.ThenBy(x => x.ClientFirstName);
					break;
				case JournalReportSortBy.Date:
					query = (@params.OrderDir == SortDir.Asc
								 ? query.OrderBy(x => x.InvoiceDate)
								 : query.OrderByDescending(x => x.InvoiceDate))
						.ThenBy(x => x.ClientLastName).ThenBy(x => x.ClientFirstName);
					break;
			}
			var rows = query.ToList();
			for(int i=0; i< rows.Count; i++)
			{
				var invoice = _invoiceRepository.FindOne(rows[i].InvoiceId);
				decimal amount = 0;
				foreach (var material in invoice.Materials)
				{
					
					amount += material.Amount;
					
				}
				if (invoice.Taxes.Count > 0)
				{
					for (int x = 0; x < invoice.Taxes.Count; x++)
					{
						amount += invoice.Taxes[x].Amount;
					}
				}
				rows[i].AmountIncludingTaxes = amount;
			}

			var summary = new JournalReportSummaryResult(rows);

			// payments are not stored with journal, because invoice is created, and payment is added later
			// (this can change).
			// We will need to look up them from invoices.
			var invoiceIds = rows.Select(x => x.InvoiceId).Distinct();

			var payments = _invoiceRepository.Query
				.Where(x => invoiceIds.Contains(x.Id))
				.ToDictionary(x=>x.Id, x=>x.Payment);
			
			var items = rows
				.Select(x => new JournalReportResultRow
				{
					Id = x.Id,
					InvoiceNo = x.InvoiceNo,
					InvoiceId = x.InvoiceId,
					Date = x.InvoiceDate.ToLocalTime().ToString("yyyy-MM-dd"),
					Type = x.Type,
					Address = x.CivicNumber + x.AptNumber.FormatIfNotEmpty(" - {0}") +", " + x.Street,
					City = x.City,
					ClientId = x.ClientId,
					ClientFirstName = x.ClientFirstName,
					ClientLastName = x.ClientLastName,
					ClientName = x.ClientLastName + " " + x.ClientFirstName,
					Materials = x.Materials.Select(m=>m.Name).JoinBy(", "),
					AmountIncludingTaxes = x.AmountIncludingTaxes,
					PaymentMethod = payments[x.InvoiceId]?.PaymentMethod.GetDescription(),
					IsTestPayment = payments[x.InvoiceId]?.IsTestPayment
				})
				.ToList();

			var resultItems = new PagedCollection<JournalReportResultRow>(items, pageSize, count, @params.Page);
			
			return new JournalReportQueryResult
			{
				Report = resultItems,
				Summary = summary
			};
		}
	}
}