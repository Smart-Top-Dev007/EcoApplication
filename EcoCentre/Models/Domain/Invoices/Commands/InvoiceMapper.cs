using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Giveaway;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Domain.User;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
	public class InvoiceMapper
	{
		private readonly Repository<Client> _clientRepository;
		private readonly AuthenticationContext _authenticationContext;
		private readonly CenterIdentification _centerIdentification;
		private readonly Repository<Material> _materialRepository;
		private readonly GlobalSettingsQuery _globalSettingsQuery;
		private readonly InvoiceFormatter _formatter;
		private readonly CompleteInvoicesListQuery _completeInvoicesListQuery;
		private readonly Repository<GiveawayItem> _giveawayRepository;
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<MaterialBrought> _materialBroughtRepository;

		public InvoiceMapper(
			Repository<Client> clientRepository,
			AuthenticationContext authenticationContext,
			CenterIdentification centerIdentification,
			Repository<Material> materialRepository,
			GlobalSettingsQuery globalSettingsQuery,
			InvoiceFormatter formatter,
			CompleteInvoicesListQuery completeInvoicesListQuery,
			Repository<GiveawayItem> giveawayRepository,
			Repository<Invoice> invoiceRepository,
			Repository<MaterialBrought> materialBroughtRepository)
		{
			_clientRepository = clientRepository;
			_authenticationContext = authenticationContext;
			_centerIdentification = centerIdentification;
			_materialRepository = materialRepository;
			_globalSettingsQuery = globalSettingsQuery;
			_formatter = formatter;
			_completeInvoicesListQuery = completeInvoicesListQuery;
			_giveawayRepository = giveawayRepository;
			_invoiceRepository = invoiceRepository;
			_materialBroughtRepository = materialBroughtRepository;
		}
		public Invoice CreateInvoice(InvoiceViewModel vm, int sequentialNo,bool isNeedSaving)
		{
			var client = _clientRepository.FindOne(vm.ClientId);

			var settings = _globalSettingsQuery.Execute();

			var createdAt = DateTime.UtcNow;


			var visitNumber = GetNumberOfVisits(client) + 1;
			var visitLimit = GetVisitsLimit(client);

			var userName = _authenticationContext.User.Name;
			if (string.IsNullOrWhiteSpace(userName))
			{
				userName = _authenticationContext.User.Login;
			}

			var invoice = new Invoice
			{
				CreatedAt = createdAt,
				CreatedBy = new InvoiceCreator
				{
					UserId = _authenticationContext.User.Id,
					UserName = userName
				},
				Comment = vm.Comment,
				ClientId = vm.ClientId,
				Address = client.Address,
				SequentialNo = sequentialNo,
				Center = _centerIdentification,
				Taxes = new List<Tax>(),
				InvoiceNo = _formatter.FormatInvoiceNo(createdAt.Year, sequentialNo),
				VisitNumber = visitNumber,
				VisitLimit = visitLimit
			};

			invoice.Materials = CreateInvoiceMaterials(vm, _authenticationContext, client);
			invoice.GiveawayItems = CreateInvoiceGiveawayItems(vm);

			invoice.Amount = invoice.Materials.Sum(x => x.Amount) +
							 invoice.GiveawayItems.Sum(x => x.Price);

			if (settings.GstTaxRate > 0)
			{
				invoice.Taxes.Add(new Tax
				{
					Name = settings.GstTaxLine,
					Rate = settings.GstTaxRate,
					Amount = TaxCalculator.GetTaxAmount(invoice.Amount, settings.GstTaxRate)
				});
			}

			if (settings.QstTaxRate > 0)
			{
				invoice.Taxes.Add(new Tax
				{
					Name = settings.QstTaxLine,
					Rate = settings.QstTaxRate,
					Amount = TaxCalculator.GetTaxAmount(invoice.Amount, settings.QstTaxRate)
				});
			}
			invoice.AmountIncludingTaxes = invoice.Amount + invoice.Taxes.Sum(x => x.Amount);
			if (isNeedSaving)
			{
				_invoiceRepository.Save(invoice);

				foreach (var material in invoice.Materials)
				{
					var materialBrought = CreateMaterialBrought(invoice, client, material);
					_materialBroughtRepository.Save(materialBrought);
				}
			}

			foreach (var material in invoice.Materials)
			{
				var materialBrought = CreateMaterialBrought(invoice, client, material);
				_materialBroughtRepository.Save(materialBrought);
			}

			return invoice;
		}

		private MaterialBrought CreateMaterialBrought(Invoice invoice, Client client, InvoiceMaterial material)
		{
			var _material = _materialRepository.Query.FirstOrDefault(x => x.Id == material.MaterialId);
			if (_material == null)
			{
				return null;
			}

			var materialBrought = new MaterialBrought()
			{
				Amount = material.Quantity,
				AmountPaid = material.Amount,
				Center = invoice.Center,
				CityId = client.Address.CityId,
				CityName = client.Address.City,
				ClientCategory = client.Category,
				ClientId = invoice.ClientId,
				Date = DateTime.Now,
				InvoiceId = invoice.Id,
				InvoiceNumber = invoice.InvoiceNo,
				IsExcludedInvoice = _material.IsExcluded,
				MaterialId = material.MaterialId,
				MaterialName = _material.Name,
				MaterialNameLower = _material.NameLower,
				Unit = _material.Unit
			};

			return materialBrought;
		}

		private IList<InvoiceMaterial> CreateInvoiceMaterials(InvoiceViewModel vm, AuthenticationContext authenticationContext, Client client)
		{
			if (vm.Materials == null || !vm.Materials.Any())
			{
				return new List<InvoiceMaterial>();
			}
			var materialIds = vm.Materials
				.Select(m => m.Id)
				.Distinct();

			var materials = _materialRepository.Query
				.Where(x => materialIds.Contains(x.Id))
				.ToDictionary(x => x.Id);

			var result = new List<InvoiceMaterial>();
			var hubId = authenticationContext.Hub.Id;
			foreach (var material in vm.Materials)
			{
				var quantity = material.Quantity;

				var materialModel = materials[material.Id];
				
				var settings = materialModel.GetHubSettings(hubId, client.Address.CityId);
				if (settings.FreeAmount > 0)
				{
					var appliedFreeAmountSoFar = GetAppliedFreeAmountSoFar(material.Id, client.Id, hubId);
					var appliedFreeAmount = Math.Min(settings.FreeAmount.Value - appliedFreeAmountSoFar, quantity);
					if (appliedFreeAmount > 0)
					{
						result.Add(new InvoiceMaterial
						{
							MaterialId = material.Id,
							Quantity = appliedFreeAmount,
							Price = 0,
							Amount = 0,
							IsUsingFreeAmount = true
						});

						quantity -= appliedFreeAmount;
					}
				}

				if (quantity > 0)
				{
					var price = materialModel.Price;
					result.Add(new InvoiceMaterial
					{
						MaterialId = material.Id,
						Quantity = quantity,
						Price = price,
						Amount = Math.Round(price * quantity, 2),
						Container = material.Container
					});
				}
			}
			return result;
		}

		private decimal GetAppliedFreeAmountSoFar(string materialId, string clientId, string hubId)
		{
			var thisYear = new DateTime(DateTime.Now.Year, 1, 1);

			var invoices = _invoiceRepository.Query
				.Where(x =>
					x.ClientId == clientId &&
					x.CreatedAt > thisYear &&
					x.Center.Id == hubId)
				.ToList();

			return invoices
				.SelectMany(x => x.Materials.Where(m => m.IsUsingFreeAmount && m.MaterialId == materialId))
				.Sum(m => m.Quantity);
		}

		private List<InvoiceGiveawayItem> CreateInvoiceGiveawayItems(InvoiceViewModel vm)
		{
			if (vm.GiveawayItems == null)
			{
				return new List<InvoiceGiveawayItem>();
			}

			var giveawayItems = GetGiveawayItems(vm);

			var result = new List<InvoiceGiveawayItem>();
			foreach (var giveawayItem in vm.GiveawayItems)
			{
				var item = giveawayItems[giveawayItem.Id];
				result.Add(new InvoiceGiveawayItem
				{
					Id = item.Id,
					Price = item.Price,
					Title = "Ton surplus... mon bonheur"
				});
			}
			return result;
		}

		private Dictionary<string, GiveawayItem> GetGiveawayItems(InvoiceViewModel vm)
		{
			var giveawayIds = vm.GiveawayItems
				.Select(m => m.Id)
				.Distinct();

			var giveawayItems = _giveawayRepository.Query
				.Where(x => giveawayIds.Contains(x.Id))
				.ToDictionary(x => x.Id);
			return giveawayItems;
		}

		private int? GetVisitsLimit(Client client)
		{
			if (client.PersonalVisitsLimit != null && client.PersonalVisitsLimit > 0)
			{
				return client.PersonalVisitsLimit.Value;
			}
			var globalVisitsLimit = _globalSettingsQuery.Execute().MaxYearlyClientVisits;
			if (globalVisitsLimit > 0)
			{
				return globalVisitsLimit;
			}
			return null;
		}

		private int GetNumberOfVisits(Client client)
		{
			var queryParams = new CompleteInvoicesListQueryParams { ClientId = client.Id };
			return _completeInvoicesListQuery.Execute(queryParams).Count;
		}
	}
}