using System.Linq;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.ViewModel.Container;
using OfficeOpenXml;

namespace EcoCentre.Models.Domain.Containers
{
    public class ContainerXlsFormatter
	{
        public byte[] Generate(PagedList<ContainerViewModel> data)
        {
            using (var pck = new ExcelPackage())
            {
                var sht = pck.Workbook.Worksheets.Add("Conteneurs");
                BuildHeader(sht);
                var row = 2;
	            if (data.Items != null)
	            {
		            foreach (var item in data.Items)
		            {
			            var cell = 1;
			            sht.SetValue(row, cell++, item.Number);
			            sht.SetValue(row, cell++, item.Capacity);
			            sht.SetValue(row, cell++, item.FillAmount);
			            sht.SetValue(row, cell++, item.AlertAtAmount);
			            sht.SetValue(row, cell++, item.DateAdded.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
			            sht.SetValue(row, cell++, item.DateOfLastAlert?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
			            sht.SetValue(row, cell++, item.Materials.Select(x=>x.Name).JoinBy(","));
			            sht.SetValue(row, cell++, item.HubName);
			            sht.SetValue(row, cell++, item.DateDeleted?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
			            row++;
		            }
	            }

	            return pck.GetAsByteArray();
            }
            
        }

		private void BuildHeader(ExcelWorksheet sht)
        {
            var cell = 1;
            sht.SetValue(1, cell++, "No conteneur");
	        sht.SetValue(1, cell++, "Nb v³");
	        sht.SetValue(1, cell++, "Nb v³ entré");
	        sht.SetValue(1, cell++, "Courriel d'alerte à X v³");
	        sht.SetValue(1, cell++, "Date arrivé");
	        sht.SetValue(1, cell++, "Date dernière alerte");
	        sht.SetValue(1, cell++, "Matériel");
	        sht.SetValue(1, cell++, "Regroupement");
	        sht.SetValue(1, cell++, "Date de sortie");
		}
    }
}
 