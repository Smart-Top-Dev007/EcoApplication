using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Containers;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.User;
using MassTransit;

namespace EcoCentre.Models
{
	public class RecalculateCustomerLimitsTask : AsyncAdminTask
	{
		private readonly Repository<LimitStatus> _limitsRepository;
		private readonly Repository<Invoice> _invoiceRepo;
		private readonly Repository<Material> _materialsRepository;

		public RecalculateCustomerLimitsTask(Repository<LimitStatus> limitsRepository, Repository<Invoice> invoiceRepo, Repository<Material> materialsRepository)
		{
			_limitsRepository = limitsRepository;
			_invoiceRepo = invoiceRepo;
			_materialsRepository = materialsRepository;
		}

		protected override void DoWork()
		{
			var limitsList = _limitsRepository.Query.ToList();
			var allMaterials = _materialsRepository.Query.ToList();

			int i = 0;

			var thisYear = new DateTime(DateTime.Now.Year, 1, 1);
			foreach (var limits in limitsList)
			{

				Progress = i*1.0m / limitsList.Count;

				var invoices = _invoiceRepo.Query.Where(x => x.Address.Id == limits.Address.Id && x.CreatedAt >= thisYear )
					.ToList();

				limits.Clear(thisYear.Year);
				foreach (var invoice in invoices)
				{
					limits.UpdateLimits(invoice, allMaterials);
				}
				_limitsRepository.Save(limits);

				i++;
			}
		}
	}
}