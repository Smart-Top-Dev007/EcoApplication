using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class DeletedOBNLReinvestmentDetails
    {
        public DeletedOBNLReinvestmentDetails(DeletedOBNLReinvestment invoice, Client client)
        {
            Id = invoice.Id;
            InvoiceNo = invoice.OBNLReinvestment.OBNLReinvestmentNo;
            CreatedAt = invoice.OBNLReinvestment.CreatedAt.ToLocalTime();
            Client = client;
            Comment = invoice.OBNLReinvestment.Comment;
            Materials = new List<OBNLReinvestmentDetailsMaterial>();
            Attachments = new List<OBNLReinvestmentDetailsAttachment>();
        }
        public string Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public Client Client { get; set; }
        public IList<OBNLReinvestmentDetailsMaterial> Materials { get; set; }
        public IList<OBNLReinvestmentDetailsAttachment> Attachments { get; set; }

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