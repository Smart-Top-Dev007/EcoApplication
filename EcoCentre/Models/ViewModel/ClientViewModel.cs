using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.OBNLReinvestments.Queries;

namespace EcoCentre.Models.ViewModel
{
    public class ClientViewModel
    {
        public ClientViewModel()
        {
        }

        public ClientViewModel(Client client, IList<InvoiceDetails> invoices = null,
            IList<OBNLReinvestmentDetails> obnlReinvestments = null)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            OBNLNumber = client.OBNLNumber;
            OBNLNumbers = client.OBNLNumbers;
            Category = client.Category;
            CategoryCustom = client.CategoryCustom;
            Email = client.Email;
            PhoneNumber = client.PhoneNumber;
	        MobilePhoneNumber = client.MobilePhoneNumber;
            Hub = client.Hub;
            Comments = client.Comments;
            Status = client.Status;
            Verified = client.Verified;
	        AllowCredit = client.AllowCredit;
	        CreditAcountNumber = client.CreditAcountNumber;
	        RefId = client.RefId;
			CitizenCard = client.CitizenCard;
			LastChange = client.LastChange.ToString();

			Address = new ClientAddressViewModel(client.Address);
            Invoices = new List<InvoiceClientDetails>();
            if (invoices != null)
            {
                foreach (var invoice in invoices)
                {
                    Invoices.Add(new InvoiceClientDetails(invoice));
                }
            }
            PersonalVisitsLimit = client.PersonalVisitsLimit ?? 0;

            OBNLReinvestments = new List<OBNLReinvestmentClientDetails>();
            if (obnlReinvestments != null)
            {
                foreach (var obnlReinvestment in obnlReinvestments)
                {
                    OBNLReinvestments.Add(new OBNLReinvestmentClientDetails(obnlReinvestment));
                }

                if (obnlReinvestments.Any())
                {
                    LastOBNLVisit = obnlReinvestments.Min(r => r.CreatedAt);
                }
                else
                {
                    LastOBNLVisit = null;
                }
            }
        }

	    public string RefId { get; set; }

	    public string MobilePhoneNumber { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OBNLNumber { get; set; }
        public IList<string> OBNLNumbers { get; set; }
        public string Category { get; set; }
        public string CategoryCustom { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Hub Hub { get; set; }
        public string Comments { get; set; }
        public ClientStatus Status { get; set; }
        public bool Verified { get; set; }
        public ClientAddressViewModel Address { get; set; }
        public IList<InvoiceClientDetails> Invoices { get; protected set; }
        public IList<OBNLReinvestmentClientDetails> OBNLReinvestments { get; protected set; }
        public DateTime? LastOBNLVisit { get; private set; }
        public bool AllowAddressCreation { get; set; }
        public int PersonalVisitsLimit { get; set; }
	    public bool IsRegisteredInCurrentHub { get; set; } = true;
	    public bool AllowCredit { get; set; }
	    public string CreditAcountNumber { get; set; }
		public string CitizenCard { get; set; }
		public string LastChange { get; set; }
}
}