using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Materials;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.OBNLReinvestments
{
    [BsonIgnoreExtraElements]
    public class OBNLReinvestment : Entity
    {
        public OBNLReinvestment()
        {
            Materials = new List<OBNLReinvestmentMaterial>();
            Attachments = new List<OBNLReinvestmentAttachment>();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }
        public int SequentialNo { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public IList<OBNLReinvestmentMaterial> Materials { get; set; }
        public IList<OBNLReinvestmentAttachment> Attachments { get; set; }

        public ClientAddress Address { get; set; }

        public void AddAttachment(string id, string name)
        {
            Attachments.Add(new OBNLReinvestmentAttachment
            {
                    Id = id,
                    Name = name
                });

        }
        public string Comment { get; set; }

        public CenterIdentification Center { get; set; }
        [BsonIgnore]
        public string OBNLReinvestmentNo => $"{CreatedAt.Year}-OBNL-{SequentialNo:00000}";

        public OBNLReinvestmentCreator CreatedBy { get; set; }

        public static Invoice Import(Client client, DateTime createdAt, int invoiceId)
        {
            if (client == null) throw new ArgumentNullException("client");
            var result = new Invoice
                {
                    ClientId = client.Id,
                    CreatedAt = createdAt,
                    SequentialNo = invoiceId
                };
            return result;
        }

        public void AddMaterial(Material material, double weight)
        {
            var im = new OBNLReinvestmentMaterial
            {
                    MaterialId = material.Id,
                    Weight = weight
                };
            Materials.Add(im);
        }

    }
}