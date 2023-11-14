using System.Collections.Generic;
using OfficeOpenXml;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class OBNLTotalXlsFormatter
    {
        public byte[] Generate(IEnumerable<OBNL.OBNLTotal> data)
        {
            using (var pck = new ExcelPackage())
            {
                var sht = pck.Workbook.Worksheets.Add("OBNL Reinvestments");
                BuildHeader(sht);
                var row = 2;
                foreach (var item in data)
                {
                    var cell = 1;
                    sht.SetValue(row,cell++,item.FullName.Trim());
                    sht.SetValue(row,cell++,item.City);
                    sht.SetValue(row,cell++,item.PostalCode);
                    sht.SetValue(row,cell++,item.Address);
                    sht.SetValue(row, cell++, item.Invoices.Count);
                    sht.SetValue(row, cell++, item.TotalWeight);
                    sht.SetValue(row, cell++, item.TotalVisits);
                    sht.SetValue(row, cell++, item.LastVisit);
                    row++;
                }
                return pck.GetAsByteArray();
            }
            
        }

        private void BuildHeader(ExcelWorksheet sht)
        {
            var cell = 1;
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.Name);
            sht.SetValue(1, cell++, Resources.Model.Client.City);
            sht.SetValue(1, cell++, Resources.Model.Client.PostalCode);
            sht.SetValue(1, cell++, Resources.Model.Client.Address);
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.NumberReinvestments);
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.TotalOBNLWeight);
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.VisitsNumber);
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.LastVisit);
        }
    }
}