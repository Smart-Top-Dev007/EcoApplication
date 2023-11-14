using EcoCentre.Helpers;
using EcoCentre.Models.CompleteImport;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCentre.Controllers
{
	public class CompleteImportController : Controller
	{
		private readonly Repository<Models.Domain.Clients.Client> _clientRepository;
		private readonly Repository<Models.Domain.Municipalities.Municipality> _municipalityRepository;
		private readonly Repository<Models.Domain.Clients.ClientAddress> _addressRepository;
		private readonly Repository<Models.Domain.Hubs.Hub> _hubRepository;

		public CompleteImportController(Repository<Models.Domain.Clients.Client> clientRepository, Repository<Models.Domain.Municipalities.Municipality> municipalityRepository, Repository<Models.Domain.Clients.ClientAddress> addressRepository, Repository<Models.Domain.Hubs.Hub> hubRepository)
		{
			_clientRepository = clientRepository;
			_municipalityRepository = municipalityRepository;
			_addressRepository = addressRepository;
			_hubRepository = hubRepository;
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Import()
		{
			var files = Request.Files;
			if (files.Count != 2)
			{
				ViewData["Error"] = "Error. Files were not found!";
				return View("Index");
			}

			string clientsFile, addressesFile;
			if (files[0].FileName == "client.xml")
			{
				clientsFile = new StreamReader(files[0].InputStream).ReadToEnd();
				addressesFile = new StreamReader(files[1].InputStream).ReadToEnd();
			}
			else if (files[0].FileName == "address.xml")
			{
				clientsFile = new StreamReader(files[1].InputStream).ReadToEnd();
				addressesFile = new StreamReader(files[0].InputStream).ReadToEnd();
			}
			else
			{
				ViewData["Error"] = "Incorrect files or file names. The names must be:client.xml and address.xml";
				return View("Index");
			}

			var catalog1 = addressesFile.ParseXML<AddressDataRoot.dataroot>();
			var catalog2 = clientsFile.ParseXML<ClientDataRoot.dataroot>();

			List<Models.CompleteImport.Client> clients = new List<Models.CompleteImport.Client>();
			List<Address> addresses = new List<Address>();
			List<Municipality> municipalities = new List<Municipality>();

			List<Dictionary<string, dynamic>> addressNodes = new List<Dictionary<string, dynamic>>();
			List<Dictionary<string, dynamic>> clientNodes = new List<Dictionary<string, dynamic>>();

			Dictionary<string, Address> creditNumberAddressPairs = new Dictionary<string, Address>();

			foreach (var rluex in catalog1.LUDIK_LUD_C_ADR)
			{
				Dictionary<string, dynamic> fields = new Dictionary<string, dynamic>();
				for (var i = 0; i < rluex.Items.Length; i++)
				{
					fields.Add(rluex.ItemsElementName[i].ToString(), rluex.Items[i].ToString());
				}
				addressNodes.Add(fields);
			}

			foreach (var rluex in catalog2.LUDIK_LUD_C_PERS)
			{
				Dictionary<string, dynamic> fields = new Dictionary<string, dynamic>();
				for (var i = 0; i < rluex.Items.Length; i++)
				{
					fields.Add(rluex.ItemsElementName[i].ToString(), rluex.Items[i].ToString());
				}
				clientNodes.Add(fields);
			}

			foreach (var node in addressNodes)
			{
				Address clientAddress = new Address();

				clientAddress.City = node.ContainsKey("VILLE") ? node["VILLE"] : "";
				clientAddress.PostalCode = node.ContainsKey("CODE_POST") ? node["CODE_POST"] : "";
				clientAddress.Street = node.ContainsKey("NOM_RUE") ? node["NOM_RUE"] : "";
				clientAddress.StreetLower = clientAddress.Street.ToLower();
				clientAddress.AptNumber = node.ContainsKey("TYPE_ADR") ? node["TYPE_ADR"] : "";
				clientAddress.CivicNumber = node.ContainsKey("NO_CIV") ? node["NO_CIV"] : "";

				if (municipalities.Find(x => x.Name == clientAddress.City) == null)
				{
					Municipality municipality = new Municipality();
					municipality.Name = clientAddress.City ?? "";
					municipality.NameLower = municipality?.Name.ToLower() ?? "";
					clientAddress.CityId = municipality._id;
					municipalities.Add(municipality);
				}
				else
				{
					Municipality municipality = municipalities.Find(x => x.Name == clientAddress.City);
					clientAddress.CityId = municipality._id;
				}

				addresses.Add(clientAddress);
				creditNumberAddressPairs.Add(node["NO_CLE_ADR"], clientAddress);
			}

			foreach (var node in clientNodes)
			{
				var client = new Models.CompleteImport.Client();

				client.FirstName = node.ContainsKey("PNOM") ? node["PNOM"] : "";
				client.FirstNameLower = client.FirstName.ToLower();
				client.LastName = node.ContainsKey("NOM") ? node["NOM"] : "";
				client.LastNameLower = client.LastName.ToLower();
				client.Email = node.ContainsKey("ADR_EMAIL") ? node["ADR_EMAIL"] : "";
				client.EmailLower = client.Email.ToLower();
				client.MobilePhoneNumber = node.ContainsKey("TEL_MAIS") ? node["TEL_MAIS"] : "";

				client.Category = (node.ContainsKey("RESID") && node["RESID"] == "1") ? "resident" : "";

				client.LastChange = node.ContainsKey("DATE_MODIF") ? Convert.ToDateTime(node["DATE_MODIF"]) : "";
				client.Municipality = null;
				//client.RegistrationDate = node.ContainsKey("DATE_CREAT") ? node["DATE_CREAT"] : "";
				client.Status = 0;
				client.Hub = null;

				var key = "";
				var value = node["NO_PERS"];
				while (value.ToString().Length + key.Length != 8)
				{
					key += "0";
				}
				key += value;
				client.Address = creditNumberAddressPairs.ContainsKey(key) ? creditNumberAddressPairs[$"{key}"] : null;
				client.AllowCredit = true;
				//client.CitizenCard = node.ContainsKey("NO_PERS") ? node["NO_PERS"] : "";
				client.CitizenCard = key;
				clients.Add(client);
			}

			var listOfClients = new List<Models.Domain.Clients.Client>();
			foreach (var client in clients)
			{
				var clientModel = new Models.Domain.Clients.Client()
				{
					Address = client.Address != null ? new ClientAddress()
					{
						AptNumber = client.Address.AptNumber,
						City = client.Address.City,
						CityId = client.Address.CityId,
						CivicNumber = client.Address.CivicNumber,
						ExternalId = client.Address.ExternalId,
						Id = client.Address._id,
						PostalCode = client.Address.PostalCode,
						Street = client.Address.Street,
						StreetLower = client.Address.StreetLower
					} : null,
					Id = client._id,
					AllowCredit = client.AllowCredit,
					Category = client.Category,
					CategoryCustom = client.CategoryCustom,
					CitizenCard = client.CitizenCard,
					Comments = client.Comments,
					CreditAcountNumber = client.CreditAcountNumber,
					Email = client.Email,
					EmailLower = client.EmailLower,
					FirstName = client.FirstName,
					FirstNameLower = client.FirstNameLower,
					Hub = client.Hub != null ? new Models.Domain.Hubs.Hub()
					{
						Address = client.Hub.Address,
						DefaultGiveawayPrice = client.Hub.DefaultGiveawayPrice,
						EmailForLoginAlerts = client.Hub.EmailForLoginAlerts,
						Id = client.Hub._id,
						InvoiceIdentifier = client.Hub.InvoiceIdentifier,
						Name = client.Hub.Name
					} : null,
					LastChange = client.LastChange,
					LastName = client.LastName,
					LastNameLower = client.LastNameLower,
					MobilePhoneNumber = client.MobilePhoneNumber,
					MunicipalityId = client.MunicipalityId,
					OBNLNumber = client.OBNLNumber,
					OBNLNumbers = null,
					PersonalVisitsLimit = client.PersonalVisitsLimit,
					PhoneNumber = client.PhoneNumber,
					PreviousAddresses = null,
					RefId = client.RefId,
					RegistrationDate = Convert.ToDateTime(client.RegistrationDate),
					Status = client.Status == 0 ? ClientStatus.Active : ClientStatus.Inactive,
					Verified = client.Verified,
					VisitsLimitExceeded = client.VisitsLimitExceeded
				};

				listOfClients.Add(clientModel);
			}

			var listOfMunicipalities = new List<Models.Domain.Municipalities.Municipality>();
			foreach (var municipality in municipalities)
			{
				var municipalityModel = new Models.Domain.Municipalities.Municipality()
				{
					CreatedAt = Convert.ToDateTime(municipality.CreatedAt),
					HubId = municipality.HubId,
					Id = municipality._id,
					Name = municipality.Name,
					NameLower = municipality.NameLower,
					UpdatedAt = Convert.ToDateTime(municipality.UpdatedAt)
				};

				if (municipality.Enabled)
				{
					municipalityModel.Enable();
				}
				else
				{
					municipalityModel.Disable();
				}

				listOfMunicipalities.Add(municipalityModel);
			}

			var listOfClientAddresses = new List<Models.Domain.Clients.ClientAddress>();
			foreach (var address in addresses)
			{
				var addressModel = new Models.Domain.Clients.ClientAddress()
				{
					AptNumber = address.AptNumber,
					StreetLower = address.StreetLower,
					Street = address.Street,
					PostalCode = address.PostalCode,
					City = address.City,
					CityId = address.CityId,
					CivicNumber = address.CivicNumber,
					ExternalId = address.ExternalId,
					Id = address._id
				};

				listOfClientAddresses.Add(addressModel);
			}

			try
			{
				var ids = _municipalityRepository.Query.Select(x => x.Id);
				var municipalityFilter = Builders<Models.Domain.Municipalities.Municipality>.Filter.In(x => x.Id, ids);
				_municipalityRepository.Collection.DeleteMany(municipalityFilter);
				_municipalityRepository.Collection.InsertMany(listOfMunicipalities);


				ids = _addressRepository.Query.Select(x => x.Id);
				var addressFilter = Builders<ClientAddress>.Filter.In(x => x.Id, ids);
				_addressRepository.Collection.DeleteMany(addressFilter);
				_addressRepository.Collection.InsertMany(listOfClientAddresses);

				ids = _clientRepository.Query.Select(x => x.Id);
				var clientFilter = Builders<Models.Domain.Clients.Client>.Filter.In(x => x.Id, ids);
				_clientRepository.Collection.DeleteMany(clientFilter);
				_clientRepository.Collection.InsertMany(listOfClients);
			}
			catch (Exception e)
			{
				ViewData["Error"] = "Import error!";
				return View("Index");
			}

			ViewData["Error"] = "Success!";
			return View("Index");
		}
	}
}