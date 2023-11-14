using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Tasks
{
    public class AddAddressesToOldOBNLReinvestmentsTask : AsyncAdminTask 
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<ClientAddress> _addressRepository;

        public AddAddressesToOldOBNLReinvestmentsTask(Repository<OBNLReinvestment> invoiceRepository, Repository<Client> clientRepository, 
            Repository<ClientAddress> addressRepository)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _addressRepository = addressRepository;
        }

        protected override void DoWork()
        {
            var invoices = _invoiceRepository.Query.Where(x => x.Address == null).ToList();
            var invoiceCount = invoices.Count;
            var invoiceIndex = 0;
            foreach(var invoice in invoices)
            {
                var client = _clientRepository.FindOne(invoice.ClientId);
                var address = client.AddressIdAt(invoice.CreatedAt);
                invoice.Address = _addressRepository.FindOne(address);
                _invoiceRepository.Save(invoice);
                invoiceIndex++;
                Progress = (decimal)invoiceIndex/invoiceCount;

            }
        }
    }
}