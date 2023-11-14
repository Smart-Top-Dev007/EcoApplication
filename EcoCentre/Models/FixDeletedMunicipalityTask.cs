using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Containers;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.User;
using MassTransit;

namespace EcoCentre.Models
{
	public class FixDeletedMunicipalityTask : AsyncAdminTask
	{
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<ClientAddress> _addressRepo;
		private readonly Repository<Invoice> _invoiceRepo;

		public FixDeletedMunicipalityTask(
			Repository<Client> clientRepository,
			Repository<ClientAddress> addressRepo,
			Repository<Invoice> invoiceRepo
		)
		{
			_clientRepository = clientRepository;
			_addressRepo = addressRepo;
			_invoiceRepo = invoiceRepo;
		}

		protected override void DoWork()
		{
			var deletedCityId = "59f2411131a1853f5490971e";
			var activeCityId = "5a7c5c09b0ae610ff8fa7746";

			var oldClients = _clientRepository.Query
				.Where(x => x.Address.CityId == deletedCityId)
				.ToList();
			
			var updatedClients = _clientRepository.Query
				.Where(x => x.Address.CityId == activeCityId && x.RefId != null)
				.ToList();
			
			var i = 0;
			var totalCount = oldClients.Count + updatedClients.Count;

			foreach (var client in oldClients)
			{
				client.Address.CityId = activeCityId;
				_clientRepository.Save(client);

				var address = _addressRepo.FindOne(client.Address.Id);
				address.CityId = activeCityId;

				var invoices = _invoiceRepo.Query.Where(x => x.ClientId == client.Id && x.Address.Id == address.Id).ToList();
				foreach (var invoice in invoices)
				{
					invoice.Address.CityId = activeCityId;
					_invoiceRepo.Save(invoice);
				}


				i++;
				Progress = i * 1.0m / totalCount;
			}


			foreach (var client in updatedClients)
			{
				int externalId  = int.Parse(client.RefId);
				var oldAddress = _addressRepo.Query.First(x => x.ExternalId == externalId);


				var invoices = _invoiceRepo.Query.Where(x => x.ClientId == client.Id && x.Address.Id == oldAddress.Id).ToList();
				foreach (var invoice in invoices)
				{
					invoice.Address = client.Address;
					_invoiceRepo.Save(invoice);
				}


				i++;
				Progress = i * 1.0m / totalCount;
			}
		}
	}
}