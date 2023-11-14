using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class InvoiceDetailsQuery
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;

        public InvoiceDetailsQuery(Repository<Invoice> invoiceRepository, Repository<Client> clientRepository, Repository<Material> materialRepository )
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
        }

        public InvoiceDetails Execute(string id)
        {
            var invoice = _invoiceRepository.FindOne(id);
	        if (invoice == null)
	        {
		        throw new NotFoundException(typeof(Invoice), id);
	        }
            var client = _clientRepository.FindOne(invoice.ClientId);
            var materialIds = invoice.Materials.Select(x => x.MaterialId);
            var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id));

            var result = new InvoiceDetails(invoice, client);
            result.IsExcluded = true;
            foreach (var material in invoice.Materials)
            {
                var materialDetails = new InvoiceDetailsMaterial();
                var originalMaterial = materials.First(x => x.Id == material.MaterialId);
                materialDetails.Name = originalMaterial.Name;
                materialDetails.Quantity = material.Quantity;
                materialDetails.Weight = material.Weight;
                materialDetails.Unit = originalMaterial.Unit;
	            materialDetails.Price = material.Price;
	            materialDetails.Amount = material.Amount;
	            materialDetails.IsUsingFreeAmount = material.IsUsingFreeAmount;
                result.Materials.Add(materialDetails);
                result.IsExcluded &= originalMaterial.IsExcluded;
            }
			result.Amount = result.Materials.Sum(x => x.Amount);
			result.AmountIncludingTaxes = result.Amount + result.Taxes.Sum(x => x.Amount);

            foreach (var attachment in invoice.Attachments)
            {
                result.AddAttachment(attachment.Id, attachment.Name);
            }
            result.IsOBNL = client.Category == "OBNL" ? true : false;
			return result;

        }
    }
}