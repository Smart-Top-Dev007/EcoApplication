using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Municipalities
{
    public class Municipality : Entity
    {
        public string Name { get; set; }
        public string NameLower { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Enabled { get; private set; }
	    [BsonRepresentation(BsonType.ObjectId)]
		public string HubId { get; set; }

	    public void Enable()
        {
            Enabled = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Disable()
        {
            Enabled = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Municipality Create(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            var municipality = new Municipality();
            municipality.UpdateName(name);
            municipality.UpdatedAt = municipality.CreatedAt = DateTime.UtcNow;
            return municipality;
        }

        public void UpdateName(string name)
        {
            Name = name.Trim();
            NameLower = Name.ToLower();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}