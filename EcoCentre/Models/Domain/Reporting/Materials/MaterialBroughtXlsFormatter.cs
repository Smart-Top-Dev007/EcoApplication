using System.Collections.Generic;
using OfficeOpenXml;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialBroughtXlsFormatter
    {
        public byte[] Generate(IList<MaterialBroughtQueryResultRow> data)
        {
            using (var pck = new ExcelPackage())
            {
                var sht = pck.Workbook.Worksheets.Add("Materials");
                BuildHeader(sht);
                var row = 2;
                foreach (var item in data)
                {
                    var cell = 1;
                    sht.SetValue(row,cell++,item.Name);
                    sht.SetValue(row,cell++,item.Invoices);
                    sht.SetValue(row,cell++,item.Quantity);
                    sht.SetValue(row,cell++,item.Unit);
                    row++;
                }
                return pck.GetAsByteArray();
            }
            
        }

        private void BuildHeader(ExcelWorksheet sht)
        {
            var cell = 1;
            sht.SetValue(1, cell++, Resources.Model.Materials.MaterialName);
            sht.SetValue(1, cell++, Resources.Model.Materials.FacturesNumber);
            sht.SetValue(1, cell++, Resources.Model.Materials.Quantity);
            sht.SetValue(1, cell++, Resources.Model.Materials.Unit);
        }
    }
}