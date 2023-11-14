using System;
using EcoCentre.Models.Domain.Municipalities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Clients
{

	public class ClientAddress : Entity
	{
		public string City { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string CityId { get; set; }
		public string Street { get; set; }
		public string StreetLower { get; set; }
		public string CivicNumber { get; set; }
		public string PostalCode { get; set; }
		public int ExternalId { get; set; }
		public string AptNumber { get; set; }

		public void UpdateDetails(Municipality city, string street, string civicNumber, string postalCode, string aptNumber)
		{
			street = street ?? string.Empty;
			civicNumber = civicNumber ?? string.Empty;
			postalCode = postalCode ?? string.Empty;
			if (city == null) throw new ArgumentNullException("city");
			City = city.Name;
			CityId = city.Id;
			Street = street.Trim();
			StreetLower = Street.ToLower();
			CivicNumber = civicNumber.Trim().ToUpper();
			PostalCode = postalCode.Trim().ToUpper();
			AptNumber = aptNumber;
		}

		public void UpdateMunicipality(Municipality city)
		{
			if (city == null) throw new ArgumentNullException("city");
			City = city.Name;
			CityId = city.Id;
		}

		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			ClientAddress p = obj as ClientAddress;
			if ((System.Object)p == null)
			{
				return false;
			}

			// Return true if the fields match:
			var fixedBPostalCode = p.PostalCode;
			var fixedAPostalCode = PostalCode;

			if (!String.IsNullOrEmpty(fixedBPostalCode) && !fixedBPostalCode.Contains("-") && fixedBPostalCode.Length > 3)
			{
				fixedBPostalCode = fixedBPostalCode.Trim().Insert(3, "-");
			}
			if (!String.IsNullOrEmpty(fixedAPostalCode) && !fixedAPostalCode.Contains("-") && fixedAPostalCode.Length > 3)
			{
				fixedAPostalCode = fixedAPostalCode.Trim().Insert(3, "-");
			}

			bool fPostalCodesComparisonResult = true;
			if (!string.IsNullOrEmpty(fixedAPostalCode) && !string.IsNullOrEmpty(fixedBPostalCode))
			{
				fPostalCodesComparisonResult = fixedAPostalCode == fixedBPostalCode;
			}

			return StreetLower == p.StreetLower &&
				Street.ToLower() == p.Street.ToLower() &&
				CivicNumber == p.CivicNumber &&
				AptNumber == p.AptNumber &&
				City.ToLower() == p.City.ToLower() &&
				fPostalCodesComparisonResult;
			//return p.StreetLower == StreetLower && p.CivicNumber == CivicNumber && p.City.ToLower() == City.ToLower() && p.PostalCode == PostalCode;
		}

		public bool Equals(ClientAddress p)
		{
			// If parameter is null return false:
			if ((object)p == null)
			{
				return false;
			}

			var fixedBPostalCode = p.PostalCode;
			var fixedAPostalCode = PostalCode;

			if (!String.IsNullOrEmpty(fixedBPostalCode) && !fixedBPostalCode.Contains("-") && fixedBPostalCode.Length > 3)
			{
				fixedBPostalCode = fixedBPostalCode.Trim().Insert(3, "-");
			}
			if (!String.IsNullOrEmpty(fixedAPostalCode) && !fixedAPostalCode.Contains("-") && fixedAPostalCode.Length > 3)
			{
				fixedAPostalCode = fixedAPostalCode.Trim().Insert(3, "-");
			}

			bool fPostalCodesComparisonResult = true;
			if (!string.IsNullOrEmpty(fixedAPostalCode) && !string.IsNullOrEmpty(fixedBPostalCode))
			{
				fPostalCodesComparisonResult = fixedAPostalCode == fixedBPostalCode;
			}

			return StreetLower == p.StreetLower &&
			       Street.ToLower() == p.Street.ToLower() &&
			       CivicNumber == p.CivicNumber &&
			       AptNumber == p.AptNumber &&
			       City.ToLower() == p.City.ToLower() &&
			       fPostalCodesComparisonResult;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
		public static bool operator ==(ClientAddress a, ClientAddress b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}

			// Return true if the fields match:
			var fixedBPostalCode = b.PostalCode;
			var fixedAPostalCode = a.PostalCode;

			if (!String.IsNullOrEmpty(fixedBPostalCode) && !fixedBPostalCode.Contains("-") && fixedBPostalCode.Length > 3)
			{
				fixedBPostalCode = fixedBPostalCode.Trim().Insert(3, "-");
			}
			if (!String.IsNullOrEmpty(fixedAPostalCode) && !fixedAPostalCode.Contains("-") && fixedAPostalCode.Length > 3)
			{
				fixedAPostalCode = fixedAPostalCode.Trim().Insert(3, "-");
			}

			bool fPostalCodesComparisonResult = true;
			if (!string.IsNullOrEmpty(fixedAPostalCode) && !string.IsNullOrEmpty(fixedBPostalCode))
			{
				fPostalCodesComparisonResult = fixedAPostalCode == fixedBPostalCode;
			}

			return a.StreetLower == b.StreetLower &&
			       a.Street.ToLower() == b.Street.ToLower() &&
			       a.CivicNumber == b.CivicNumber &&
			       a.AptNumber == b.AptNumber &&
			       a.City.ToLower() == b.City.ToLower() &&
			       fPostalCodesComparisonResult;
			//return a.StreetLower == b.StreetLower && a.CivicNumber == b.CivicNumber && a.City.ToLower() == b.City.ToLower() && a.PostalCode == b.PostalCode;
		}

		public static bool operator !=(ClientAddress a, ClientAddress b)
		{
			return !(a == b);
		}
	}
}