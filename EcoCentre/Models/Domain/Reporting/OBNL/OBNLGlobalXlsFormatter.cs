using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Reporting.OBNL;
using OfficeOpenXml;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class OBNLGlobalXlsFormatter
    {
        private IDictionary<string, int> materialMap = new Dictionary<string, int>();

        public byte[] Generate(IEnumerable<OBNL.OBNLGlobal> data)
        {
            using (var pck = new ExcelPackage())
            {
                var sht = pck.Workbook.Worksheets.Add("OBNL Reinvestments");
                BuildHeader(sht, data);
                var row = 2;
                foreach (var item in data)
                {
                    var cell = 1;
                    sht.SetValue(row,cell++,item.Name.Trim());
                    sht.SetValue(row,cell++,item.City);
                    sht.SetValue(row,cell++,item.PostalCode);
                    sht.SetValue(row,cell++,item.Address);
                    sht.SetValue(row, cell++, item.OBNLReinvestments.Count());

                    foreach (var material in item.Materials)
                    {
                        sht.SetValue(row, materialMap[material.Name], material.Weight);
                    }

                    row++;
                }
                return pck.GetAsByteArray();
            }
            
        }

        private void BuildHeader(ExcelWorksheet sht, IEnumerable<OBNLGlobal> data)
        {
            var cell = 1;
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.Name);
            sht.SetValue(1, cell++, Resources.Model.Client.City);
            sht.SetValue(1, cell++, Resources.Model.Client.PostalCode);
            sht.SetValue(1, cell++, Resources.Model.Client.Address);
            sht.SetValue(1, cell++, Resources.Model.OBNLReinvestment.NumberReinvestments);

            var materials = data.SelectMany(x => x.Materials).Select(x => x.Name).Distinct();
            foreach (var material in materials)
            {
                materialMap[material] = cell;
                sht.SetValue(1, cell++, material);
                
            }
        }
    }
}