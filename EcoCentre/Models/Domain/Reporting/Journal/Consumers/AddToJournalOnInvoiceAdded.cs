using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using EcoCentre.Models.Domain.Materials;
using MassTransit;
using NLog;

namespace EcoCentre.Models.Domain.Reporting.Journal.Consumers
{
	public class AddToJournalOnInvoiceAdded : Consumes<InvoiceAddedEvent>.All
	{
		private readonly Repository<InvoiceJournal> _journalRepository;
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<Material> _materialRepository;

		public AddToJournalOnInvoiceAdded(Repository<InvoiceJournal> journalRepository, Repository<Invoice> invoiceRepository,
										  Repository<Client> clientRepository, Repository<Material> materialRepository)
		{
			_journalRepository = journalRepository;
			_invoiceRepository = invoiceRepository;
			_clientRepository = clientRepository;
			_materialRepository = materialRepository;
		}

		public void Consume(InvoiceAddedEvent message)
		{
			var existing = _journalRepository.Query.SingleOrDefault(x => x.InvoiceId == message.InvoiceId);
			if (existing != null)
				_journalRepository.Remove(existing);
			var invoice = _invoiceRepository.FindOne(message.InvoiceId);


			var client = _clientRepository.FindOne(invoice.ClientId);
			var mIds = invoice.Materials.Select(x => x.MaterialId).ToArray();
			var materials = _materialRepository.Query.Where(x => mIds.Contains(x.Id)).ToList();

			var materialList = materials
				.Select(m => new InvoiceJournalMaterial { Name = m.Name, NameLower = m.NameLower })
				.ToList();

			if (client == null) return;

			var journal = new InvoiceJournal
			{
				City = client.Address.City,
				CityId = client.Address.CityId,
				CivicNumber = client.Address.CivicNumber,
				AptNumber = client.Address.AptNumber,
				ClientFirstName = client.FirstName,
				ClientLastName = client.LastName,
				ClientId = client.Id,
				InvoiceDate = invoice.CreatedAt,
				HubId = client.Hub?.Id,
				InvoiceId = invoice.Id,
				InvoiceNo = invoice.InvoiceNo,
				PostalCode = client.Address.PostalCode,
				Materials = materialList,
				Street = client.Address.Street,
				Type = client.Category,
				AmountIncludingTaxes = invoice.AmountIncludingTaxes				
			};
			_journalRepository.Insert(journal);
		}
	}
}
