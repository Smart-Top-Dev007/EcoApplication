using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using EcoCentre.Models.Domain.Hubs;
using MongoDB.Bson;

namespace EcoCentre.Models.Domain.Clients
{
    [BsonIgnoreExtraElements]
    public class Client : Entity, ICloneable
    {
        public Client()
        {
            Hub = new Hub();
            Address = new ClientAddress();
            PreviousAddresses = new List<ClientAddressLog>();
        }

        public string FirstName { get; set; }
        public string FirstNameLower { get; set; }
        public string LastName { get; set; }
        public string LastNameLower { get; set; }
        public string OBNLNumber { get; set; }
        public IList<string> OBNLNumbers { get; set; }
        public string Category { get; set; }
        public string CategoryCustom { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }

        public string Email { get; set; }
        public string EmailLower { get; set; }
		public string PostalCode { get; set; }

		public ClientAddress Address { get; /*protected*/ set; }
        public IList<ClientAddressLog> PreviousAddresses { get; set; }

        public Hub Hub { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string MunicipalityId { get; set; }
        public ClientStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastChange { get; set; }
        public bool Verified { get; set; }
        public string Comments { get; set; }

        public bool VisitsLimitExceeded { get; set; }
        public int? PersonalVisitsLimit { get; set; }

        public string RefId { get; set; }
	    public bool AllowCredit { get; set; }
	    public string CreditAcountNumber { get; set; }
		public string CitizenCard { get; set; }
		public string Gender { get; set; }
		public DateTime? DateFinMember { get; set; }

		public void UpdateName(string firstName, string lastName)
        {
            firstName = firstName ?? string.Empty;
            lastName = lastName ?? string.Empty;
            FirstName = firstName.Trim();
            FirstNameLower = FirstName.ToLower();
            LastName = lastName.Trim();
            LastNameLower = LastName.ToLower();
            LastChange = DateTime.UtcNow;
        }
        public void UpdateContact(string email, string phone, string mobilePhoneNumber)
        {
            email = email ?? string.Empty;
            phone = phone ?? string.Empty;
            mobilePhoneNumber = mobilePhoneNumber ?? string.Empty;
            PhoneNumber = phone.Trim();
	        MobilePhoneNumber = mobilePhoneNumber.Trim();
	        Email = email.Trim();
	        EmailLower = Email.ToLower();
	        LastChange = DateTime.UtcNow;
        }

        public void UpdateAddress(ClientAddress address)
        {
            if (address != Address)
            {
                if (Address.Id != null && Address.Id.Length == 24)
                {
                    if (PreviousAddresses == null)
                    {
                        PreviousAddresses = new List<ClientAddressLog>();
                    }
                    PreviousAddresses.Add(new ClientAddressLog { AddressId = Address.Id, ChangedAt = DateTime.UtcNow });
                }
                
                Address = address;
                
                LastChange = DateTime.UtcNow;
            }
            else if (address.Id != Address.Id) // update the Id
            {
                Address = address;
            }
        }

        public void Verify()
        {
            Verified = true;
            LastChange = DateTime.UtcNow;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string AddressIdAt(DateTime createdAt)
        {
            if (PreviousAddresses == null) return Address.Id;
            if (!PreviousAddresses.Any(x => x.ChangedAt > createdAt)) return Address.Id;
            return PreviousAddresses.OrderBy(x => x.ChangedAt).First(x => x.ChangedAt > createdAt).AddressId;
        }

        public int SetPersonalVisitsLimit(int limit)
        {
            PersonalVisitsLimit = limit;
            return limit;
		}

        private const int CitizenCardLength = 9;

        public void UpdateCitizenCard()
        {
	        if (string.IsNullOrWhiteSpace(CitizenCard)) return;
	        var lengthDifference = CitizenCardLength - CitizenCard.Length;
	        if (lengthDifference > 0)
	        {
		        CitizenCard = $"{string.Join("", Enumerable.Repeat("0", lengthDifference))}{CitizenCard}";
	        }
        }
	}
}