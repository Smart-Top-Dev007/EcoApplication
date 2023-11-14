using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using EcoCentre.Models.Domain.Materials;
using MassTransit;

namespace EcoCentre.Models.Domain.Limits.Consumers
{
    public class UpdateLimitsOnInvoiceAdded : Consumes<InvoiceAddedEvent>.All
    {
	    private readonly Repository<LimitStatus> _limitsRepository;
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Material> _materialRepository;

        public UpdateLimitsOnInvoiceAdded(Repository<LimitStatus> limitsRepository, Repository<Invoice> invoiceRepository, 
            Repository<Material> materialRepository )
        {
            _limitsRepository = limitsRepository;
            _invoiceRepository = invoiceRepository;
            _materialRepository = materialRepository;
		}

        public void Consume(InvoiceAddedEvent message)
        {
			var invoice = _invoiceRepository.FindOne(message.InvoiceId);
			var mateiralIds = invoice.Materials.Select(x => x.MaterialId).ToList();
            var materials = _materialRepository.Query.Where(x => mateiralIds.Contains(x.Id)).ToList();
			
            var limits = _limitsRepository.Query.FirstOrDefault(x => x.Address.Id == invoice.Address.Id);
			if (limits == null)
            {
                limits = new LimitStatus
                {
                    Address = invoice.Address,
                    Limits = new List<LimitStatusYear>()
                };
            }
            limits.UpdateLimits(invoice, materials);
            _limitsRepository.Save(limits);
        }

    }
}