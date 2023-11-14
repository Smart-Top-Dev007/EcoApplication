using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.CompleteImport
{
	public class Address
	{
		[JsonProperty(PropertyName = "_id")]
		public string _id { get; set; }
		[JsonProperty(PropertyName = "City")]
		public string City { get; set; }
		[JsonProperty(PropertyName = "CityId")]
		public string CityId { get; set; }
		[JsonProperty(PropertyName = "Street")]
		public string Street { get; set; }
		[JsonProperty(PropertyName = "StreetLower")]
		public string StreetLower { get; set; }
		[JsonProperty(PropertyName = "CivicNumber")]
		public string CivicNumber { get; set; }
		[JsonProperty(PropertyName = "PostalCode")]
		public string PostalCode { get; set; }
		[JsonProperty(PropertyName = "ExternalId")]
		public int ExternalId { get; set; }
		[JsonProperty(PropertyName = "AptNumber")]
		public string AptNumber { get; set; }

		public Address()
		{
			_id = ObjectId.GenerateNewId().ToString();
			ExternalId = 0;
		}
	}
}