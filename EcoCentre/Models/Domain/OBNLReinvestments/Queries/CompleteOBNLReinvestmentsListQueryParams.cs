using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class CompleteOBNLReinvestmentsListQueryParams
    {
        public string ClientId { get; set; }
        public Client Client { get; set; } // will only be used internally
    }
}