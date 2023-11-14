using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace EcoCentre.Models.Domain.Materials
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Material : Entity
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public string NameLower { get; set; }
        public string Unit { get; set; }
        public decimal MaxYearlyAmount { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double Weight { get; set; }
        public bool IsExcluded { get; set; }
	    public decimal Price { get; set; }
	    public List<HubMaterialSettings> HubSettings { get; set; }

	    public HubMaterialSettings GetHubSettings(string hubId, string municipalityId = null)
	    {
			var municipalitySettings = HubSettings?.FirstOrDefault(x => x.HubId == hubId && x.MunicipalityId == municipalityId);
		    if (municipalitySettings !=null)
		    {
			    return municipalitySettings;
		    }
			return HubSettings?.FirstOrDefault(x => x.HubId == hubId);
	    }

	    public static Material Create(string tag, string name, string unit)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            var date = DateTime.UtcNow;
            var result = new Material
                {

                    Tag = tag.Trim().ToUpper(),
                    Name = name.Trim(),
                    Unit = unit.Trim().ToLower(),
                    MaxYearlyAmount = 100,
                    CreatedAt = date,
                    UpdatedAt = date,
                    Active = true
                    
                };
            result.NameLower = result.Name.ToLower();
            return result;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Material p = obj as Material;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            //return (x == p.x) && (y == p.y);
            return p.NameLower == NameLower && p.Unit == Unit && p.Tag == Tag;
        }

        public bool Equals(Material p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            return p.NameLower == NameLower && p.Unit == Unit && p.Tag == Tag;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public static bool operator ==(Material a, Material b)
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
            return a.NameLower == b.NameLower && a.Unit == b.Unit && a.Tag == b.Tag;
        }

        public static bool operator !=(Material a, Material b)
        {
            return !(a == b);
        }

	    public bool IsAllowedToPutToContainer(string hubId)
	    {
			var settings = GetHubSettings(hubId);
		    return settings?.HasContainer ?? false;
		}
    }


	[BsonIgnoreExtraElements]
	public class HubMaterialSettings
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string HubId { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string MunicipalityId { get; set; }
		public decimal? MaxAmountPerVisit { get; set; }
		public decimal? MaxVisits { get; set; }
		public bool RequireProofOfResidence { get; set; }
		public bool HasContainer { get; set; }
		public bool IsActive { get; set; }
		public decimal? FreeAmount { get; set; }
	}
}