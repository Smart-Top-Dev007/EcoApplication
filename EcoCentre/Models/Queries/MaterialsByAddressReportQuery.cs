using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Municipalities;
using NLog;
using Nuclex.Cloning;

namespace EcoCentre.Models.Queries
{
	using Domain.GlobalSettings;
	using Domain.GlobalSettings.Queries;

	public class MaterialsByAddressReportQuery
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Repository<Client> _clientsRepository;
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<Material> _materialRepository;
		private readonly Repository<Municipality> _municipalitiesRepository;
		private readonly Repository<MaterialBrought> _materialBroughtRepository;
		private readonly Repository<CachedMaterialsByAddressQuery> _cachedMaterialsByAddressBundle;
		private readonly Repository<MaterialByAddress> _cachedMaterialsByAddress;
		private readonly MaterialByAddressCachingSettings _cachingSettings;
		private readonly GlobalSettings _globalSettings;

		public MaterialsByAddressReportQuery(GlobalSettingsQuery globalSettingsQuery,
			Repository<Client> clientsRepository, Repository<Invoice> invoiceRepository, Repository<Material> materialRepository,
			Repository<Municipality> municipalitiesRepository, Repository<MaterialBrought> materialBroughtRepository,
			Repository<CachedMaterialsByAddressQuery> cachedMaterialsByAddressBundle, Repository<MaterialByAddress> cachedMaterialsByAddress,
			MaterialByAddressCachingSettings cachingSettings)
		{
			_clientsRepository = clientsRepository;
			_invoiceRepository = invoiceRepository;
			_materialRepository = materialRepository;
			_municipalitiesRepository = municipalitiesRepository;
			_materialBroughtRepository = materialBroughtRepository;
			_cachedMaterialsByAddressBundle = cachedMaterialsByAddressBundle;
			_cachedMaterialsByAddress = cachedMaterialsByAddress;
			_cachingSettings = cachingSettings;


			_globalSettings = globalSettingsQuery.Execute();
		}

		private const int PageSize = 20;

		private void RemoveCachedMaterialByAddressList(List<string> idsList)
		{
			if (idsList.Count == 0) return;

			var cachedMaterialsByAddressToDeleteList = _cachedMaterialsByAddress.Query.Where(x => idsList.Contains(x.Id)).ToList();
			foreach (var materialByAddress in cachedMaterialsByAddressToDeleteList)
			{
				_cachedMaterialsByAddress.Remove(materialByAddress);
			}
		}
		private List<MaterialByAddress> _LoadCachedResult(MaterialsByAddressReportQueryParams @params)
		{
			var curCachedResultsList = _cachedMaterialsByAddressBundle.Query.ToList();
			var curCachedResult = curCachedResultsList.FirstOrDefault(x => x.Query == @params);

			if (null == curCachedResult)
			{
				return null;
			}
			if ((DateTime.Now - curCachedResult.CreatedAt).Days > _cachingSettings.MaxCachedRequestAgeInDays)
			{
				RemoveCachedMaterialByAddressList(curCachedResult.CachedMaterialByAddressIds);
				_cachedMaterialsByAddressBundle.Remove(curCachedResult);
				return null;
			}

			return _cachedMaterialsByAddress.Query.Where(x => curCachedResult.CachedMaterialByAddressIds.Contains(x.Id)).ToList();
		}
		private void _CacheSearchResult(MaterialsByAddressReportQueryParams @params, List<MaterialByAddress> result)
		{
			if (result.Count == 0) return;

			var curCachedResultsList = _cachedMaterialsByAddressBundle.Query.OrderBy(x => x.CreatedAt).ToList();
			if (curCachedResultsList.Count > _cachingSettings.MaxCachedRequestsNumber)
			{
				var oldestCachedResults = curCachedResultsList.LastOrDefault();
				if (null != oldestCachedResults)
				{
					RemoveCachedMaterialByAddressList(oldestCachedResults.CachedMaterialByAddressIds);
					_cachedMaterialsByAddressBundle.Remove(oldestCachedResults);
				}
			}

			_cachedMaterialsByAddress.InsertBatch(result);

			_cachedMaterialsByAddressBundle.Save(new CachedMaterialsByAddressQuery
			{
				Query = new MaterialsByAddressReportQueryParams
				{
					FilterFirstName = @params.FilterFirstName,
					FilterLastName = @params.FilterLastName,
					FilterCivicNumber = @params.FilterCivicNumber,
					FilterStreet = @params.FilterStreet,
					FromDate = @params.FromDate,
					ToDate = @params.ToDate,
					SearchTerm = @params.SearchTerm,
					SearchType = @params.SearchType
				},
				CachedMaterialByAddressIds = result.Select(x => x.Id).ToList(),
				CreatedAt = DateTime.Now
			});
		}
		private List<MaterialByAddress> _FetchNewResultsList(MaterialsByAddressReportQueryParams @params)
		{
			// ToList().OrderBy(...).ToList() construction here is used to overcome Mongo's 32mb in-memory sort limit without
			// completely re-indexing the database and using ensureIndex, by moving the sorts to IIS memory.
			// It should be fixed at some point, of course, but it just takes too much time.
			var materialsBroughtQuery = _materialBroughtRepository.Query;

			if (@params.FromDate != null)
				materialsBroughtQuery = materialsBroughtQuery.Where(x => x.Date >= @params.FromDate.Value);
			if (@params.ToDate != null)
				materialsBroughtQuery = materialsBroughtQuery.Where(x => x.Date < @params.ToDate.Value.AddDays(1));

			if (!String.IsNullOrEmpty(@params.CenterName) && @params.CenterName.ToLower() != "tous")
			{
				materialsBroughtQuery =
					materialsBroughtQuery.Where(x => x.Center != null && x.Center.Name == @params.CenterName);
			}

			var materialsBroughtQueryList = materialsBroughtQuery.ToList().OrderBy(x => x.ClientId).ToList();

			var materialsBroughtClientIds = materialsBroughtQueryList.Select(x => x.ClientId);

			var clientsQuery = _clientsRepository.Query;
			if (!@params.AllClients)
			{
				clientsQuery = clientsQuery.Where(x => materialsBroughtClientIds.Contains(x.Id));
			}

			clientsQuery = ApplySearchTerm(@params, clientsQuery);

			if (@params.PersonalVisitsLimitHigherThenGlobalOnly)
			{
				clientsQuery = clientsQuery
					.Where(x =>
						x.PersonalVisitsLimit != null &&
						x.PersonalVisitsLimit > _globalSettings.MaxYearlyClientVisits
					);
			}

			var clientsList = clientsQuery.ToList().OrderBy(x => x.Id).ToList();
			var clientIds = clientsList.Select(x => x.Id).ToList();

			materialsBroughtQueryList = materialsBroughtQueryList
				.Where(x => clientIds.Contains(x.ClientId))
				.OrderBy(x => x.ClientId)
				.ToList();

			var result = clientsList.Select(x => new MaterialByAddress
			{
				ClientId = x.Id,
				Name = !String.IsNullOrEmpty(x.FirstName) || !String.IsNullOrEmpty(x.LastName)
					? x.FirstName + " " + x.LastName
					: "",
				FirstName = x.FirstName,
				FirstNameLower = x.FirstNameLower,
				LastName = x.LastName,
				LastNameLower = x.LastNameLower,
				City = x.Address.City,
				Street = x.Address.Street,
				StreetLower = x.Address.StreetLower,
				CivicNumber = x.Address.CivicNumber,
				PostalCode = x.Address.PostalCode,
				PersonalVisitsLimit = x.PersonalVisitsLimit ?? 0,
				AptNumber = x.Address.AptNumber
			}).OrderBy(x => x.ClientId).ToList();

			List<MaterialByAddressMaterial> materialsList = _materialRepository.Query
					.Where(i => i.Active)
					.Select(i => new MaterialByAddressMaterial
					{
						Name = i.Name,
						Tag = i.Tag,
						Id = i.Id,
						Unit = i.Unit
					}).ToList();

			MaterialBrought curMaterialBrought;
			foreach (var matByAddr in result)
			{
				matByAddr.Invoices = new List<MaterialByAddressInvoice>();
				matByAddr.IncludedInvoices = new List<MaterialByAddressInvoice>();
				matByAddr.ExcludedInvoices = new List<MaterialByAddressInvoice>();
				matByAddr.Materials = ReflectionCloner.DeepFieldClone(materialsList);

				while (materialsBroughtQueryList.Count > 0 && materialsBroughtQueryList[0].ClientId == matByAddr.ClientId) //both are order the same way
				{
					curMaterialBrought = materialsBroughtQueryList[0];
					var resultInvoice = matByAddr.Invoices.FirstOrDefault(x => x.Id == curMaterialBrought.InvoiceId);
					if (resultInvoice == null)
					{
						resultInvoice = new MaterialByAddressInvoice
						{
							Id = curMaterialBrought.InvoiceId,
							CreatedAt = curMaterialBrought.Date,
							CenterName = curMaterialBrought.Center != null ? curMaterialBrought.Center.Name : "",
							CenterUrl = curMaterialBrought.Center != null ? curMaterialBrought.Center.Url : "",
							InvoiceNo = curMaterialBrought.InvoiceNumber,
							IsExcluded = curMaterialBrought.IsExcludedInvoice
						};

						matByAddr.Invoices.Add(resultInvoice);
						if (curMaterialBrought.IsExcludedInvoice)
						{
							matByAddr.ExcludedInvoices.Add(resultInvoice);
							matByAddr.TotalExcludedInvoices++;
						}
						else
						{
							matByAddr.IncludedInvoices.Add(resultInvoice);
							matByAddr.TotalIncludedInvoices++;
						}
						matByAddr.TotalInvoices++;
					}
					
					var resultMaterial = matByAddr.Materials.FirstOrDefault(x => x.Id == curMaterialBrought.MaterialId);
					if (resultMaterial != null)
					{
						var invoice = _invoiceRepository.Query.FirstOrDefault(x => x.Id == curMaterialBrought.InvoiceId);
						if (invoice != null)
						{
							var material = invoice.Materials.FirstOrDefault(x => x.MaterialId == curMaterialBrought.MaterialId);
							if (material != null)
							{
								resultMaterial.Total += material.Quantity;
							}
							else
							{
								resultMaterial.Total += 0;
							}
						}
						else
						{
							resultMaterial.Total += 0;
						}
					}
					materialsBroughtQueryList.RemoveAt(0);
				}
			}

			var ids = result.SelectMany(x => x.Invoices.Select(i => i.Id)).Distinct();
			var invoices = _invoiceRepository.Query.Where(x => ids.Contains(x.Id)).ToDictionary(x=>x.Id);

			List<string> missingInvoices = new List<string>();
			foreach (var row in result)
			{
				var rowInvoices = row.Invoices
					.Select(x =>
					{
						if (x.Id == null || !invoices.ContainsKey(x.Id))
						{
							missingInvoices.Add(x.Id);
							return null;
						}
						return invoices[x.Id];
					})
					.Where(x=>x != null)
					.ToList();
				row.Amount = rowInvoices.Sum(x=>x.Amount);
				row.AmountIncludingTaxes = rowInvoices.Sum(x => x.AmountIncludingTaxes);
			}

			if (missingInvoices.Any())
			{
				Logger.Warn($"Could not find invoices with the following ids: {string.Join(", ", missingInvoices)}");
			}

			return result;
		}
		private List<MaterialByAddress> _PrepareResultsList(MaterialsByAddressReportQueryParams @params)
		{
			if (@params.FromDate != null) @params.FromDate = @params.FromDate.Value.Date;
			if (@params.ToDate != null) @params.ToDate = @params.ToDate.Value.Date;

			var result = _LoadCachedResult(@params);
			if (null == result)
			{
				result = _FetchNewResultsList(@params);
				_CacheSearchResult(@params, result);
			}
			return result;
		}
		public PagedCollection<MaterialsByAddressReportQueryResultRow> Execute(MaterialsByAddressReportQueryParams @params)
		{
			var materialsByAddressList = _PrepareResultsList(@params);
			int count = materialsByAddressList.Count;

			var sortedMaterials = ApplySortToList(@params, materialsByAddressList);

			if (@params.Page < 1) @params.Page = 1;
			var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
			var pageSize = @params.PageSize ?? PageSize;

			if (pageSize > 0)
			{
				sortedMaterials = sortedMaterials.Skip(skip).Take(pageSize);
			}

			var result = new List<MaterialsByAddressReportQueryResultRow>();

			foreach (var item in sortedMaterials)
			{
				var row = new MaterialsByAddressReportQueryResultRow
				{
					ClientId = item.ClientId,
					Name = item.Name,
					City = item.City,
					Street = item.Street,
					CivicNumber = item.CivicNumber,
					AptNumber = item.AptNumber,
					PostalCode = item.PostalCode,
					Invoices = item.Invoices,
					ExcludedInvoices = item.ExcludedInvoices,
					IncludedInvoices = item.IncludedInvoices,
					Materials = item.Materials,
					PersonalVisitsLimit = item.PersonalVisitsLimit,
					Amount = item.Amount,
					AmountIncludingTaxes = item.AmountIncludingTaxes,

				};
				result.Add(row);
			}

			return new PagedCollection<MaterialsByAddressReportQueryResultRow>(result, pageSize, count, @params.Page);
		}

		private IQueryable<MaterialByAddress> ApplySortToList(MaterialsByAddressReportQueryParams @params, List<MaterialByAddress> list)
		{
			switch (@params.SortBy)
			{
				case MaterialByAddressSortBy.City:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.City).AsQueryable();
					else
						return list.OrderByDescending(x => x.City).AsQueryable();
				case MaterialByAddressSortBy.PostalCode:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.PostalCode).AsQueryable();
					else
						return list.OrderByDescending(x => x.PostalCode).AsQueryable();
				case MaterialByAddressSortBy.Address:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.Street).AsQueryable();
					else
						return list.OrderByDescending(x => x.Street).AsQueryable();
				case MaterialByAddressSortBy.Invoices:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.TotalInvoices).AsQueryable();
					else
						return list.OrderByDescending(x => x.TotalInvoices).AsQueryable();
				case MaterialByAddressSortBy.ExcludedInvoices:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.TotalExcludedInvoices).AsQueryable();
					else
						return list.OrderByDescending(x => x.TotalExcludedInvoices).AsQueryable();
				case MaterialByAddressSortBy.IncludedInvoices:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.TotalIncludedInvoices).AsQueryable();
					else
						return list.OrderByDescending(x => x.TotalIncludedInvoices).AsQueryable();
				case MaterialByAddressSortBy.Materials:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.Materials[@params.SortIndex].Total).AsQueryable();
					else
						return list.OrderByDescending(x => x.Materials[@params.SortIndex].Total).AsQueryable();
				case MaterialByAddressSortBy.PersonalVisitsLimit:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.PersonalVisitsLimit).AsQueryable();
					else
						return list.OrderByDescending(x => x.PersonalVisitsLimit).AsQueryable();
				case MaterialByAddressSortBy.Amount:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.Amount).AsQueryable();
					else
						return list.OrderByDescending(x => x.Amount).AsQueryable();
				case MaterialByAddressSortBy.AmountIncludingTaxes:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.AmountIncludingTaxes).AsQueryable();
					else
						return list.OrderByDescending(x => x.AmountIncludingTaxes).AsQueryable();
				default:
					if (@params.SortDir == SortDir.Asc)
						return list.OrderBy(x => x.Name).AsQueryable();
					else
						return list.OrderByDescending(x => x.Name).AsQueryable();
			}
		}

		private IQueryable<Client> ApplySearchTerm(MaterialsByAddressReportQueryParams @params, IQueryable<Client> query)
		{
			if (string.IsNullOrEmpty(@params.SearchTerm) && (@params.SearchType == MaterialByAddressSearchBy.City || @params.SearchType == MaterialByAddressSearchBy.PostalCode) ||
				(string.IsNullOrEmpty(@params.FilterFirstName) && string.IsNullOrEmpty(@params.FilterLastName)) && @params.SearchType == MaterialByAddressSearchBy.ClientName ||
				(string.IsNullOrEmpty(@params.FilterCivicNumber) && string.IsNullOrEmpty(@params.FilterStreet)) && @params.SearchType == MaterialByAddressSearchBy.Address)
			{
				return query;
			}


			switch (@params.SearchType)
			{
				case MaterialByAddressSearchBy.ClientName:
					@params.FilterFirstName = (@params.FilterFirstName ?? "").Trim().ToLower();
					@params.FilterLastName = (@params.FilterLastName ?? "").Trim().ToLower();
					query = query.Where(x => x.FirstNameLower.Contains(@params.FilterFirstName) && x.LastNameLower.Contains(@params.FilterLastName));
					break;
				case MaterialByAddressSearchBy.Address:
					@params.FilterCivicNumber = (@params.FilterCivicNumber ?? "").Trim().ToUpper();
					@params.FilterStreet = (@params.FilterStreet ?? "").Trim().ToLower();
					var addressRegExp = "(\\s|^)" + Regex.Escape(@params.FilterStreet);
					query = query.Where(x => Regex.IsMatch(x.Address.StreetLower, addressRegExp)); //x.Address.StreetLower.Contains(@params.FilterStreet.ToLower()));
					if (!string.IsNullOrEmpty(@params.FilterCivicNumber))
					{
						query = query.Where(x => x.Address.CivicNumber == @params.FilterCivicNumber);
					}
					break;
				case MaterialByAddressSearchBy.City:
					@params.SearchTerm = (@params.SearchTerm ?? "").Trim().ToLower();

					var matchingCities = _municipalitiesRepository.Query.Where(x => x.NameLower.Contains(@params.SearchTerm));
					var matchingCitiesIds = matchingCities.Select(x => x.Id).ToList();
					query = query.Where(x => matchingCitiesIds.Contains(x.Address.CityId));
					break;
				case MaterialByAddressSearchBy.PostalCode:
					@params.SearchTerm = (@params.SearchTerm ?? "").Trim().ToUpper();

					query = query.Where(x => x.Address.PostalCode.Contains(@params.SearchTerm));
					break;
			}

			return query;
		}

		public List<MaterialsByAddressReportQueryResultRow> ExecuteAll(MaterialsByAddressReportQueryParams @params)
		{
			var materialsByAddressList = _PrepareResultsList(@params);

			var sortedMaterials = ApplySortToList(@params, materialsByAddressList);

			var result = new List<MaterialsByAddressReportQueryResultRow>();
			foreach (var item in sortedMaterials)
			{
				var row = new MaterialsByAddressReportQueryResultRow
				{
					ClientId = item.ClientId,
					Name = item.Name,
					City = item.City,
					Street = item.Street,
					AptNumber = item.AptNumber,
					CivicNumber = item.CivicNumber,
					PostalCode = item.PostalCode,
					Invoices = item.Invoices,
					ExcludedInvoices = item.ExcludedInvoices,
					IncludedInvoices = item.IncludedInvoices,
					Materials = item.Materials,
					PersonalVisitsLimit = item.PersonalVisitsLimit,
					Amount = item.Amount,
					AmountIncludingTaxes = item.AmountIncludingTaxes
				};
				result.Add(row);
			}

			return result;
		}
	}
}