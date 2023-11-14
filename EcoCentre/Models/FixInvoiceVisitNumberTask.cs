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
	public class FixInvoiceVisitNumberTask : AsyncAdminTask
	{
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<Invoice> _invoiceRepo;

		public FixInvoiceVisitNumberTask(Repository<Client> clientRepository, Repository<Invoice> invoiceRepo)
		{
			_clientRepository = clientRepository;
			_invoiceRepo = invoiceRepo;
		}

		protected override void DoWork()
		{
			var clients = _clientRepository.Query.Select(x=>x.Id).ToList();

			int i = 0;

			var thisYear = new DateTime(DateTime.Now.Year, 1, 1);
			foreach (var client in clients)
			{

				Progress = i*1.0m / clients.Count;

				var invoices = _invoiceRepo.Query.Where(x => x.ClientId == client && x.CreatedAt >= thisYear )
					.OrderBy(x=>x.CreatedAt)
					.ToList();

				int visitNumber = 0;
				foreach (var invoice in invoices)
				{
					visitNumber++;
					if (invoice.VisitNumber != visitNumber)
					{
						invoice.VisitNumber = visitNumber;
						_invoiceRepo.Save(invoice);
					}
				}

				i++;
			}
		}
	}
}