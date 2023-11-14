using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.CompleteImport
{
	public class Client
	{
		[JsonProperty(PropertyName = "_id")]
		public string _id { get; set; }
		[JsonProperty(PropertyName = "FirstName")]
		public string FirstName { get; set; }
		[JsonProperty(PropertyName = "FirstNameLower")]
		public string FirstNameLower { get; set; }
		[JsonProperty(PropertyName = "LastName")]
		public string LastName { get; set; }
		[JsonProperty(PropertyName = "LastNameLower")]
		public string LastNameLower { get; set; }
		[JsonProperty(PropertyName = "OBNLNumber")]
		public string OBNLNumber { get; set; }
		[JsonProperty(PropertyName = "OBNLNumbers")]
		public string OBNLNumbers { get; set; }
		[JsonProperty(PropertyName = "Category")]
		public string Category { get; set; }
		[JsonProperty(PropertyName = "CategoryCustom")]
		public string CategoryCustom { get; set; }
		[JsonProperty(PropertyName = "PhoneNumber")]
		public string PhoneNumber { get; set; }
		[JsonProperty(PropertyName = "MobilePhoneNumber")]
		public string MobilePhoneNumber { get; set; }
		[JsonProperty(PropertyName = "Email")]
		public string Email { get; set; }
		[JsonProperty(PropertyName = "EmailLower")]
		public string EmailLower { get; set; }
		[JsonProperty(PropertyName = "Address")]
		public Address Address { get; set; }
		[JsonProperty(PropertyName = "PreviousAddresses")]
		public string[] PreviousAddresses { get; set; }
		[JsonProperty(PropertyName = "Hub")]
		public Hub Hub { get; set; }

		[JsonProperty(PropertyName = "Municipality")]
		public string Municipality { get; set; }

		[JsonProperty(PropertyName = "MunicipalityId")]
		public string MunicipalityId { get; set; }

		[JsonProperty(PropertyName = "Status")]
		public int Status { get; set; }

		[JsonProperty(PropertyName = "RegistrationDate")]
		public string RegistrationDate { get; set; }

		[JsonProperty(PropertyName = "LastChange")]
		public DateTime LastChange { get; set; }

		[JsonProperty(PropertyName = "Verified")]
		public bool Verified { get; set; }

		[JsonProperty(PropertyName = "Comments")]
		public string Comments { get; set; }

		[JsonProperty(PropertyName = "VisitsLimitExceeded")]
		public bool VisitsLimitExceeded { get; set; }
		[JsonProperty(PropertyName = "PersonalVisitsLimit")]
		public int PersonalVisitsLimit { get; set; }
		[JsonProperty(PropertyName = "RefId")]
		public string RefId { get; set; }
		[JsonProperty(PropertyName = "AllowCredit")]
		public bool AllowCredit { get; set; }
		[JsonProperty(PropertyName = "CreditAcountNumber")]
		public string CreditAcountNumber { get; set; }
		[JsonProperty(PropertyName = "CitizenCard")]
		public string CitizenCard { get; set; }

		public Client()
		{
			_id = ObjectId.GenerateNewId().ToString();
			FirstName = string.Empty;
			FirstNameLower = string.Empty;
			LastName = string.Empty;
			LastNameLower = string.Empty;
			OBNLNumber = null;
			OBNLNumbers = null;
			Category = "resident";
			CategoryCustom = null;
			PhoneNumber = string.Empty;
			MobilePhoneNumber = string.Empty;
			Email = string.Empty;
			EmailLower = string.Empty;
			Address = new Address();
			PreviousAddresses = new string[0];
			Hub = new Hub();
			Municipality = string.Empty;
			MunicipalityId = null;
			Status = 0;
			RegistrationDate = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.032+0000'");
			LastChange = DateTime.Now;
				//.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.032+0000'");
			Verified = true;
			Comments = null;
			VisitsLimitExceeded = false;
			PersonalVisitsLimit = 0;
			RefId = null;
			AllowCredit = false;
			CreditAcountNumber = null;
		}
	}
}