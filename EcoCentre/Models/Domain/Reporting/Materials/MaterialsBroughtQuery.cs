using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialsBroughtQuery
    {
        private readonly Repository<MaterialBrought> _materialBroughtRepository;
		private readonly Repository<Invoice> _invoiceRepository;

		public MaterialsBroughtQuery(Repository<MaterialBrought> materialBroughtRepository, Repository<Invoice> invoiceRepository)
		{
			_materialBroughtRepository = materialBroughtRepository;
			_invoiceRepository = invoiceRepository;
		}

		public List<MaterialBroughtQueryResultRow> Execute(MaterialsBroughtQueryParams param)
        {

            var query = _materialBroughtRepository.Query;

	        if (param.From != null)
	        {
		        query = query.Where(x => x.Date >= param.From.Value);
	        }

	        if (param.To != null)
	        {
		        query = query.Where(x => x.Date < param.To.Value.AddDays(1));
	        }

	        if (param.HubId.IsNotEmpty())
	        {
		        query = query.Where(x => x.Center.Id == param.HubId);
	        }

	        if (param.MunicipalityId.IsNotEmpty())
	        {
		        query = query.Where(x => x.CityId == param.MunicipalityId);
	        }

	        var items = query.ToList();
            var result = new List<MaterialBroughtQueryResultRow>();

			var a = items.Where(x => x.MaterialId == "5eaae252559aae10d0f550d6").ToList();

			foreach (var item in items)
            {
                var resultItem = result.FirstOrDefault(x => x.MaterialId == item.MaterialId);
                if (resultItem == null)
                {
                    resultItem = new MaterialBroughtQueryResultRow
                        {
                            Name = item.MaterialName,
                            MaterialId = item.MaterialId,
                            Quantity = 0,
                            Unit = item.Unit,
                            Invoices = 0
                        };
                    result.Add(resultItem);
                }
				var invoice = _invoiceRepository.Query.FirstOrDefault(x => x.Id == item.InvoiceId);
				var material = invoice != null ? invoice.Materials.FirstOrDefault(x => x.MaterialId == item.MaterialId) : null;
				var quantity = material != null ? material.Quantity : 0;

				resultItem.Quantity += quantity;
				resultItem.Invoices++;
            }

            var sortReverse = param.SortDir == SortDir.Desc;
            Func<MaterialBroughtQueryResultRow, MaterialBroughtQueryResultRow, int> sortExpression = null;
            switch (param.SortBy)
            {
                case MaterialsBroughtBySortBy.Quantity:
                    sortExpression = (x, y) => x.Quantity.CompareTo(y.Quantity);
                    break;
                case MaterialsBroughtBySortBy.Unit:
                    sortExpression = (x, y) => String.Compare(x.Unit, y.Unit, StringComparison.Ordinal);
                    break;
                case MaterialsBroughtBySortBy.Invoices:
                    sortExpression = (x, y) => x.Invoices.CompareTo(y.Invoices);
                    break;
                default:
                    sortExpression = (x, y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal);
                    break;
            }
            var comparer = new LambdaComparer<MaterialBroughtQueryResultRow>(sortExpression, sortReverse);
            result.Sort(comparer);
            return result;
        }
    }
}