using System.Collections.Generic;
using EcoCentre.Models.Queries;
using System.Text;
using OfficeOpenXml;
using System.CodeDom.Compiler;
using System.IO;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialByAddressXlsFormatter
    {
        public byte[] Generate(IList<MaterialsByAddressReportQueryResultRow> data)
        {
            // CSV export - commented for good, but won't be deleted yet
            //CsvExport csv = new CsvExport();

            //foreach (var item in data)
            //{
            //    StringBuilder invoicesList = new StringBuilder();
            //    foreach (var invoice in item.Invoices)
            //    {
            //        if (invoicesList.Length > 0) invoicesList.Append(", ");
            //        invoicesList.Append(invoice.InvoiceNo);
            //    }

            //    csv.AddRow();

            //    csv["Nom"] = item.Name;
            //    csv["Ville"] = item.City;
            //    csv["CodePostal"] = item.PostalCode;
            //    csv["Adresse"] = String.Format("{0} {1}", item.CivicNumber, item.Street);
            //    csv["Factures"] = invoicesList;

            //    foreach (var material in item.Materials)
            //    {
            //        csv[string.Format("{0} ({1}), {2}", material.Tag, material.Name, material.Unit)] = material.Total;
            //    }
            //}

            //return csv.ExportToBytes();
            using (var pck = new ExcelPackage())
            {
                var sht = pck.Workbook.Worksheets.Add("Matériaux par adresse");
                BuildHeader(sht, data[0]);
                var row = 2;
                foreach (var item in data)
                {
                    var cell = 1;
                    sht.SetValue(row, cell++, item.Name);
                    sht.SetValue(row, cell++, item.City);
                    sht.SetValue(row, cell++, item.PostalCode);
                    sht.SetValue(row, cell++, $"{item.CivicNumber}{item.AptNumber.FormatIfNotEmpty(" - {0}")} {item.Street}");
                    sht.SetValue(row, cell++, item.PersonalVisitsLimit);
                    sht.SetValue(row, cell++, item.IncludedInvoices.Count);
                    sht.SetValue(row, cell++, item.ExcludedInvoices.Count);
                    sht.SetValue(row, cell++, item.Invoices.Count);
                    sht.SetValue(row, cell++, item.Amount);
                    sht.SetValue(row, cell++, item.AmountIncludingTaxes);

                    StringBuilder invoicesList = new StringBuilder();
                    foreach (var invoice in item.Invoices)
                    {
                        if (invoicesList.Length > 0) invoicesList.Append(", ");
                        invoicesList.Append(invoice.InvoiceNo);
                    }
                    sht.SetValue(row, cell++, invoicesList);
                    foreach (var material in item.Materials)
                    {
                        sht.SetValue(row, cell++, material.Total);
                    }
                    row++;
                }
                //return pck.GetAsByteArray();
                // [ObjectDisposedException: Store must be open for this operation.] workaround

                using (var tempFiles = new TempFileCollection())
                {
                    string tmpPath = tempFiles.AddExtension("xlsx");

                    Stream stream = File.Create(tmpPath);
                    pck.SaveAs(stream);
                    stream.Close();

                    return File.ReadAllBytes(tmpPath);
                }
            }       
        }
        private void BuildHeader(ExcelWorksheet sht, MaterialsByAddressReportQueryResultRow firstItem)
        {
            var cell = 1;
            sht.SetValue(1, cell++, "Nom");
            sht.SetValue(1, cell++, "Ville");
            sht.SetValue(1, cell++, "CodePostal");
            sht.SetValue(1, cell++, "Adresse");
            sht.SetValue(1, cell++, "Limite des visites personnelles");
            sht.SetValue(1, cell++, "Nombre de factures inclus");
            sht.SetValue(1, cell++, "Nombre de factures exclus");
            sht.SetValue(1, cell++, "Nombre de factures totales");
	        sht.SetValue(1, cell++, "!!! Amount");
	        sht.SetValue(1, cell++, "!!! AmountIncludingTaxes");
			sht.SetValue(1, cell++, "Factures");
            foreach (var material in firstItem.Materials)
            {
                sht.SetValue(1, cell++, string.Format("{0} ({1}), {2}", material.Tag, material.Name, material.Unit));
            }
        }
    }
}