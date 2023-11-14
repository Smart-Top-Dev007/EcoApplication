using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLReinvestmentDetailsQuery
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;

        public OBNLReinvestmentDetailsQuery(Repository<OBNLReinvestment> invoiceRepository, 
            Repository<Client> clientRepository, 
            Repository<Material> materialRepository )
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
        }

        public OBNLReinvestmentDetails Execute(string id)
        {
            var invoice = _invoiceRepository.FindOne(id);
            var client = _clientRepository.FindOne(invoice.ClientId);
            var materialIds = invoice.Materials.Select(x => x.MaterialId);
            var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id));

            var result = new OBNLReinvestmentDetails(invoice, client);
            result.IsExcluded = true;
            foreach (var material in invoice.Materials)
            {
                var materialDetails = new OBNLReinvestmentDetailsMaterial();
                var originalMaterial = materials.First(x => x.Id == material.MaterialId);
                materialDetails.Name = originalMaterial.Name;
                materialDetails.Weight = material.Weight;
                materialDetails.Unit = originalMaterial.Unit;
                result.Materials.Add(materialDetails);
                result.IsExcluded &= originalMaterial.IsExcluded;
            }
            foreach (var attachment in invoice.Attachments)
            {
                result.AddAttachment(attachment.Id, attachment.Name);
            }
            return result;

        }
    }
}