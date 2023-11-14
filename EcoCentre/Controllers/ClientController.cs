using System;
using System.Linq;
using System.Web.Mvc;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.Clients.Queries;
using EcoCentre.Models.ViewModel;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Commands;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using EcoCentre.Models.Domain.OBNLReinvestments.Queries;
using MongoDB.Driver;

namespace EcoCentre.Controllers
{
    public class ClientController : Controller
    {
        private readonly Repository<Client> _clientRepository;
        private readonly UpdateClientCommand _updateClientCommand;
        private readonly CreateClientCommand _createClientCommand;
        private readonly ClientListQuery _clientListQuery;
        private readonly SuggestClientNameQuery _suggestClientNameQuery;
		private readonly SuggestOBNLNumberQuery _suggestOBNLNumberQuery;
        private readonly SuggestAddressQuery _suggestAddressQuery;
        private readonly CompleteInvoicesListQuery _completeInvoicesListQuery;
        private readonly CompleteOBNLReinvestmentsListQuery _completeOBNLReinvestmentsListQuery;
        private readonly MergeClientsCommand _mergeClientsCommand;
        private readonly GlobalSettingsQuery _globalSettingsQuery;
        private readonly DeleteClientCommand _deleteClientCommand;

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public ClientController(Repository<Client> clientRepository, 
            UpdateClientCommand updateClientCommand, 
            CreateClientCommand createClientCommand,
            ClientListQuery clientListQuery, 
            SuggestClientNameQuery suggestClientNameQuery,
			SuggestOBNLNumberQuery suggestOBNLNumberQuery,
            CompleteInvoicesListQuery completeInvoicesListQuery, 
            CompleteOBNLReinvestmentsListQuery completeOBNLReinvestmentsListQuery,
            MergeClientsCommand mergeClientsCommand, 
            GlobalSettingsQuery globalSettingsQuery,
            SuggestAddressQuery suggestAddressQuery, 
            DeleteClientCommand deleteClientCommand)
        {
            _clientRepository = clientRepository;
            _updateClientCommand = updateClientCommand;
            _createClientCommand = createClientCommand;
            _clientListQuery = clientListQuery;
            _suggestClientNameQuery = suggestClientNameQuery;
			_suggestOBNLNumberQuery = suggestOBNLNumberQuery;
            _suggestAddressQuery = suggestAddressQuery;
            _completeInvoicesListQuery = completeInvoicesListQuery;
            _completeOBNLReinvestmentsListQuery = completeOBNLReinvestmentsListQuery;
            _mergeClientsCommand = mergeClientsCommand;
            _globalSettingsQuery = globalSettingsQuery;
            _deleteClientCommand = deleteClientCommand;
        }

        //
        // GET: /Client/
        public ActionResult Index(ClientListQueryParams @params)
        {

            if (string.IsNullOrEmpty(@params.Id))
            {

                return Json(_clientListQuery.Execute(@params), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var rawClient = _clientRepository.Collection.AsQueryable().SingleOrDefault(x => x.Id == @params.Id);
                var data = new ClientViewModel(rawClient,
                    _completeInvoicesListQuery.Execute(new CompleteInvoicesListQueryParams {Client = rawClient}),
                    _completeOBNLReinvestmentsListQuery.Execute(new CompleteOBNLReinvestmentsListQueryParams()
                    {
                        Client = rawClient
                    }));
				data.LastChange = data.LastChange.ToString();

				return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Suggest(string term, bool isFirstName = false)
        {
            var data = _suggestClientNameQuery.Execute(term, isFirstName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

		public ActionResult Suggest1(string term, bool isFirstName = false)
		{
			var data = _suggestClientNameQuery.Execute1(term, isFirstName);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult SuggestOBNL(string term)
        {
            var data = _suggestOBNLNumberQuery.Execute(term);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
		public ActionResult SuggestOBNL1(string term)
		{
			var data = _suggestOBNLNumberQuery.Execute1(term);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult SuggestCivicCard(string card)
		{
			var data = _suggestOBNLNumberQuery.Execute(card, true);
			return Json(data, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SuggestCivicCard1(string card)
		{
			var data = _suggestOBNLNumberQuery.Execute1(card, true);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult SuggestStreet(string number, string streetName, string postalCode, string cityId, string hubId)
        {
	        if (hubId == "null")
	        {
		        hubId = null;
	        }
            var data = _suggestAddressQuery.Execute(number, streetName, postalCode, "streetName", cityId, hubId);
			return Json(data, JsonRequestBehavior.AllowGet);
        }
		public ActionResult SuggestStreet1(string number, string streetName, string postalCode, string cityId, string hubId)
		{
			if (hubId == "null")
			{
				hubId = null;
			}
			var data = _suggestAddressQuery.Execute1(number, streetName, postalCode, "streetName", cityId, hubId);
			return Json(data, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SuggestCivicNumber(string number, string streetName, string postalCode)
        {
            var data = _suggestAddressQuery.Execute(number, streetName, postalCode, "number");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
		public ActionResult SuggestCivicNumber1(string number, string streetName, string postalCode)
		{
			var data = _suggestAddressQuery.Execute1(number, streetName, postalCode, "number");
			return Json(data, JsonRequestBehavior.AllowGet);
		}
		public ActionResult SuggestPostalCode(string number, string streetName, string postalCode)
        {
            var data = _suggestAddressQuery.Execute(number, streetName, postalCode, "postalCode");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
		public ActionResult SuggestPostalCode1(string number, string streetName, string postalCode)
		{
			var data = _suggestAddressQuery.Execute1(number, streetName, postalCode, "postalCode");
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult IndexTemplate()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }
        public ActionResult NewTemplate()
        {
            return View();
        }

		public ActionResult Canadian_NewTemplate()
		{
			return View();
		}
		public ActionResult ShowTemplate()
        {
            return View();
        }

        [HandleError()]
        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(ClientViewModel client)
        {
            // replace hyphens
            client.Address.Street = client.Address.Street?.Replace("-", " ").Trim();

            var result = _createClientCommand.Execute(client, Request.Url.Host.ToLower(), _globalSettingsQuery.Execute().AdminNotificationsEmail);

            return Json(result);
        }

		[HandleError()]
		[RequireWriteRightsForPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CanadianIndex(ClientViewModel client)
		{
			// replace hyphens
			client.Address.Street = client.Address.Street?.Replace("-", " ").Trim();

			var result = _createClientCommand.Execute1(client, Request.Url.Host.ToLower(), _globalSettingsQuery.Execute().AdminNotificationsEmail);

			return Json(result);
		}

		[RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Index(ExistingClientViewModel client)
        {
            var result = _updateClientCommand.Execute(client, Request.Url.Host.ToLower(),
                _globalSettingsQuery.Execute().AdminNotificationsEmail);
            return Json(result);
        }


        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Index(string id)
        {
            _deleteClientCommand.Execute(id);
            return Json("Ok");
        }

        public ActionResult ClientInvoices(CompleteInvoicesListQueryParams @param)
        {
            var result = _completeInvoicesListQuery.Execute(param);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MergerTemplate()
        {
            return View();
        }
        [HandleError()]
        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Merge(MergeCommandParams @params)
        {
            var result = _mergeClientsCommand.Execute(@params);
            return Json(result);
        }
    }
}
