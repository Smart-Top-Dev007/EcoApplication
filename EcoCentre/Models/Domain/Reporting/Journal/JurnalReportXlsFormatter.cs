using EcoCentre.Models.Queries;
using OfficeOpenXml;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
    public class JurnalReportXlsFormatter
    {
        public byte[] Generate(PagedCollection<JournalReportResultRow> data)
        {
            using (var pck = new ExcelPackage())
            {
                var sht = pck.Workbook.Worksheets.Add("Invoices");
                BuildHeader(sht);
                var row = 2;
                foreach (var item in data.Items)
                {
                    var cell = 1;
                    sht.SetValue(row,cell++,item.InvoiceNo);
                    sht.SetValue(row,cell++,item.Date);
                    sht.SetValue(row,cell++,item.ClientFirstName);
                    sht.SetValue(row,cell++,item.ClientLastName);
                    sht.SetValue(row,cell++,item.Materials);
                    sht.SetValue(row,cell++,item.AmountIncludingTaxes);
                    sht.SetValue(row,cell++,item.PaymentMethod);
                    sht.SetValue(row,cell++,item.Type);
                    sht.SetValue(row,cell++,item.Address);
                    sht.SetValue(row,cell++,item.City);
                    row++;
                }
                return pck.GetAsByteArray();
            }
            
        }

        private void BuildHeader(ExcelWorksheet sht)
        {
            var cell = 1;
            sht.SetValue(1, cell++, Resources.Model.Invoice.InvoiceNo);
            sht.SetValue(1, cell++, Resources.Model.Invoice.InvoiceDate);
            sht.SetValue(1, cell++, Resources.Model.Client.FirstName);
            sht.SetValue(1, cell++, Resources.Model.Client.LastName);
            sht.SetValue(1, cell++, "Materials");
            sht.SetValue(1, cell++, "Amount including taxes");
            sht.SetValue(1, cell++, "Payment method");
            sht.SetValue(1, cell++, Resources.Model.Client.Category);
            sht.SetValue(1, cell++, Resources.Model.Client.Address);
            sht.SetValue(1, cell++, Resources.Model.Client.City);
        }
    }
}