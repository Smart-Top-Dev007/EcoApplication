using EcoCentre.Models.Domain.Giveaway;
using EcoCentre.Models.Domain.Invoices.Events;
using FluentValidation;
using MassTransit;
using EcoCentre.Models.Domain.Clients;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.GlobalSettings.Queries;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
	public class CreateInvoiceCommand
	{
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly InvoiceViewModelValidator _invoiceViewModelValidator;
		private readonly Sequences _sequences;
		private readonly FileRepository _fileRepository;
		private readonly IServiceBus _serviceBus;
		private readonly InvoiceMapper _invoiceMapper;
		private readonly Repository<Client> _clientRepository;
		private readonly UpdateClientCommand _updateClientCommand;
		private readonly GlobalSettingsQuery _globalSettingsQuery;


		public CreateInvoiceCommand(Repository<Client> clientRepository,
			 UpdateClientCommand updateClientCommand,
			 GlobalSettingsQuery globalSettingsQuery,
			Repository<Invoice> invoiceRepository,
			InvoiceViewModelValidator invoiceViewModelValidator,
			Sequences sequences,
			FileRepository fileRepository,
			IServiceBus serviceBus,
			InvoiceMapper invoiceMapper)
		{
			_invoiceRepository = invoiceRepository;
			_invoiceViewModelValidator = invoiceViewModelValidator;
			_sequences = sequences;
			_fileRepository = fileRepository;
			_serviceBus = serviceBus;
			_invoiceMapper = invoiceMapper;
			_clientRepository = clientRepository;
			_updateClientCommand = updateClientCommand;
			_globalSettingsQuery = globalSettingsQuery;
		}

		public Invoice Execute(InvoiceViewModel vm)
		{

			_invoiceViewModelValidator.ValidateAndThrow(vm);
			var sequentialNo = _sequences.NextInvoice();
			
			var invoice = _invoiceMapper.CreateInvoice(vm, sequentialNo, true);

			SaveAttachments(vm, invoice);

			var invoiceAddedEvent = new InvoiceAddedEvent
			{
				InvoiceId = invoice.Id
			};

			_serviceBus.Publish(invoiceAddedEvent);

			var rawClient = _clientRepository.Collection.AsQueryable().SingleOrDefault( x => x.Id == invoice.ClientId);
			rawClient.LastChange = invoice.CreatedAt;
			_clientRepository.Save(rawClient);
			//	client.LastChange = DateTime.UtcNow;
			//client.Verify();
			//_clientRepository.Save(client);


			return invoice;
		}

		private void SaveAttachments(InvoiceViewModel vm, Invoice invoice)
		{
			if (vm.Attachments == null)
			{
				return;
			}

			foreach (var attachment in vm.Attachments)
			{
				var file = _fileRepository.Find(attachment.Id);
				invoice.AddAttachment(file.Id.ToString(), file.Filename);
			}
		}			
	}
}