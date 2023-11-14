using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class DeletedInvoiceDetails
    {
        public DeletedInvoiceDetails(DeletedInvoice invoice, Client client)
        {
            Id = invoice.Id;
            InvoiceNo = invoice.Invoice.InvoiceNo;
            CreatedAt = invoice.Invoice.CreatedAt.ToLocalTime();
            Client = client;
            Comment = invoice.Invoice.Comment;
            Materials = new List<InvoiceDetailsMaterial>();
            Attachments = new List<InvoiceDetailsAttachment>();
        }
        public string Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public Client Client { get; set; }
        public IList<InvoiceDetailsMaterial> Materials { get; set; }
        public IList<InvoiceDetailsAttachment> Attachments { get; set; }

        public void AddAttachment(string id, string name)
        {
            Attachments.Add(new InvoiceDetailsAttachment
                {
                    Id = id,
                    Name = name
                });
        }
    }
}