using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Hubs.Queries;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.Import;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.ViewModel;

namespace EcoCentre.Controllers
{
    public class ImportController : Controller
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Municipality> _municipalityRepository;
        private readonly Repository<ClientAddress> _addressRepository;
		private readonly Repository<ClientAddress1> _addressRepository1;
		private readonly Repository<Hub> _hubRepository;

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

        public ImportController(Repository<Client> clientRepository, Repository<Municipality> municipalityRepository, Repository<ClientAddress> addressRepository, Repository<ClientAddress1> addressRepository1
			, Repository<Hub> hubRepository)
        {
            _clientRepository = clientRepository;
            _municipalityRepository = municipalityRepository;
            _addressRepository = addressRepository;
			_addressRepository1 = addressRepository1;
			_hubRepository = hubRepository;
        }
        //
        // GET: /Import/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportClients()
        {
            ImportClientsModel model = new ImportClientsModel();
            HubListQuery hlquery = new HubListQuery(_hubRepository);
            var hubs = hlquery.Execute();
            if (hubs != null)
            {
                model.Hubs = hubs.Select(h => new SelectListItem()
                {
                    Text = h.Name,
                    Value = h.Id
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ImportClients(HttpPostedFileBase pathToFile, ImportClientsModel model)
        {
            var addressQuery = new ClientAddressQuery(_addressRepository, _addressRepository1);
            var hubQuery = new HubDetailsQuery(_hubRepository);

            HubListQuery hlquery = new HubListQuery(_hubRepository);
            var hubs = hlquery.Execute();

            if (pathToFile != null)
            {
                int totalcount = 0;
                using (var excel = new ExcelWrapper(pathToFile))
                {
                    excel.Hubs = hubs;
                    excel.HubId = model.HubId;
                    List<ExcelRow> rows = excel.Rows.ToList();

                    rows = rows.Where(r => r.CivicNo != null && r.CivicNo != "" && r.CivicNo != "0").ToList();

                    //rows = rows.AsQueryable().Distinct(new ExcelRowCompare()).ToList();

                    //rows = rows.AsQueryable().Distinct(new ExcelRowCompareByLN()).ToList();


                    foreach (var row in rows)
                    {
                        if (String.IsNullOrWhiteSpace(row.CivicNo))
                            continue;

                        Console.WriteLine("Imprting: " + row);
                        var cmd = new ImportClientCommand(_clientRepository, _municipalityRepository, addressQuery, hubQuery);
                        cmd.Execute(new ClientImportModel
                        {
                            Address = new ClientAddressImportModel
                            {
                                City = row.City,
                                CivicNumber = row.CivicNo,
                                PostalCode = row.PostalCode,
                                Street = row.Street
                            },
                            FirstName = row.FirstName,
                            Category = ClientCategory.Resident.ToString(),
                            LastName = row.LastName,
                            RefId = row.RefId,
                            HubId = model.HubId
                        });
                        totalcount++;
                    }

                }
                TempData.Add("SuccessImport", "Import done!!! File: " + pathToFile.FileName + ", total rows count: " + totalcount);

            }
            else
            {
                ModelState.AddModelError("NoFile", "No File selected");
                return View();
            }

            return RedirectToAction("ImportClients");
        }

    }
}
