using System;
using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.ViewModel
{
    public class ClientImportModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Category { get; set; }
        public ClientAddressImportModel Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ClientStatus Status { get; set; }
        public string Comments { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string RefId { get; set; }

        public string HubId { get; set; }

        public string CitizenCard { get; set; }

        public string Gender { get; set; }

        public DateTime? DateFinMember { get; set; }
		public DateTime LastChange { get; set; }
	}
}