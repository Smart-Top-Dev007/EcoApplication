using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLReinvestmentDetails
    {
        public OBNLReinvestmentDetails(OBNLReinvestment invoice, Client client )
        {
            Id = invoice.Id;
            CreatedBy = invoice.CreatedBy;
            OBNLReinvestmentNo = invoice.OBNLReinvestmentNo;
            CreatedAt= invoice.CreatedAt.ToLocalTime();
            Client = client;
            Comment = invoice.Comment;
            Address = invoice.Address;
            Center = invoice.Center;
            Materials = new List<OBNLReinvestmentDetailsMaterial>();
            Attachments = new List<OBNLReinvestmentDetailsAttachment>();
        }

        public OBNLReinvestmentCreator CreatedBy { get; set; }

        public ClientAddress Address { get; set; }
        public CenterIdentification Center { get; set; }

        public string Id { get; set; }
        public string OBNLReinvestmentNo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public Client Client { get; set; }
        public IList<OBNLReinvestmentDetailsMaterial> Materials { get; set; }
        public IList<OBNLReinvestmentDetailsAttachment> Attachments { get; set; }

        public bool IsExcluded { get; set; }

        public void AddAttachment(string id, string name)
        {
            Attachments.Add(new OBNLReinvestmentDetailsAttachment
            {
                    Id = id,
                    Name = name
                });
        }
    }
}