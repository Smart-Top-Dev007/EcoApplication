using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.Infrastructure;
using OfficeOpenXml;

namespace EcoCompoImport
{
	internal class Program
	{
		private static void Main()
		{

			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

			var addressFile = ConfigurationManager.AppSettings.Get("AddressFile");
			var databaseConnectionString = ConfigurationManager.ConnectionStrings["EcoDatabase"].ConnectionString;


			var uow = new UnitOfWork(databaseConnectionString);
			
			var addressRepo = new Repository<ClientAddress>(uow);
			var municipalityRepo = new Repository<Municipality>(uow);
			var clientRepo = new Repository<Client>(uow);
			var hubRepo = new Repository<Hub>(uow);

			var addresses = addressRepo.Query.ToLookup(x => x.ExternalId);
			var clients = clientRepo.Query.ToLookup(x => x.RefId ?? x.Address.ExternalId.ToString());
			var municipalities = municipalityRepo.Query.ToLookup(x => x.Name);
			var hubs = hubRepo.Query.ToLookup(x => x.Name);

			var postalCode = ConfigurationManager.AppSettings.Get("DefaultPostalCode");
			var i = 0;
			using (var excel = new ExcelWrapper(addressFile))
			{
				foreach (var row in excel.Rows)
				{
					var municipality = municipalities[row.Grouping].FirstOrDefault();

					if (municipality == null)
					{
						municipality = new Municipality();
						municipality.UpdateName(row.Grouping);
						municipalityRepo.Insert(municipality);
						municipalities = municipalityRepo.Query.ToLookup(x => x.Name);
					}

					var hub = hubs[row.Grouping].FirstOrDefault();

					if (hub == null)
					{
						hub = Hub.Create(row.Grouping);
						hubRepo.Insert(hub);
						hubs = hubRepo.Query.ToLookup(x => x.Name);
					}

					ClientAddress clientAddress;
					if (addresses.Contains(row.ExternalId))
					{
						clientAddress = addresses[row.ExternalId].First();
					}
					else
					{
						clientAddress = new ClientAddress
						{
							ExternalId = row.ExternalId
						};
						clientAddress.UpdateDetails(municipality, row.Street, row.CivicNumber, postalCode, "");

						addressRepo.Insert(clientAddress);
					}

					Client client;
					var clientRefId = row.ExternalId.ToString();
					var exists = false;
					if (clients.Contains(clientRefId))
					{
						client = clients[clientRefId].First();
						exists = true;
					}
					else
					{
						client = new Client();
					}
					client.UpdateName(row.FirstName, row.LastName);
					client.UpdateContact(null, row.PhoneNumber, row.MobilePhoneNumber);
					client.UpdateAddress(clientAddress);
					client.Category = GetClientCategory(row);
					client.RefId = clientRefId;
					client.Hub = hub;
					client.Verify();

					if (exists)
					{
						clientRepo.Save(client);
					}
					else
					{
						clientRepo.Insert(client);
					}

					i++;

					if (i%100 == 0)
					{
						Console.WriteLine(i);
					}
				}
			}
		}

		private static string GetClientCategory(ExcelRow row)
		{
			if (row.Category == "Résident") return ClientCategory.Resident.ToString();
			if (row.Category == "Commerce") return ClientCategory.Commerce.ToString();
			if (row.Category == "Institution") return ClientCategory.Institution.ToString();
			return ClientCategory.Other.ToString();
		}

		internal class ExcelWrapper : IDisposable
		{
			private readonly ExcelPackage _package;

			public ExcelWrapper(string fileName)
			{
				var file = new FileInfo(fileName);
				if (!file.Exists)
					throw new Exception("File not found:" + file.FullName);
				_package = new ExcelPackage(file);
			}

			public IEnumerable<ExcelRow> Rows
			{
				get
				{
					var ri = 2;
					var sht = _package.Workbook.Worksheets[1];
					while (true)
					{
						var id = sht.GetValue<string>(ri, 1)?.Trim();
						if (string.IsNullOrEmpty(id)) break;

						yield return new ExcelRow
						{
							ExternalId = int.Parse(id),
							CivicNumber = sht.GetValue<string>(ri, 2)?.Trim(),
							Street = sht.GetValue<string>(ri, 3)?.Trim(),
							PostalCode = sht.GetValue<string>(ri, 4)?.Trim(),
							FirstName = sht.GetValue<string>(ri, 5)?.Trim(),
							LastName = sht.GetValue<string>(ri, 6)?.Trim(),
							PhoneNumber = sht.GetValue<string>(ri, 7)?.Trim(),
							MobilePhoneNumber = sht.GetValue<string>(ri, 8)?.Trim(),
							Grouping = sht.GetValue<string>(ri, 9).Trim(),
							City = sht.GetValue<string>(ri, 10).Trim(),
							Category = sht.GetValue<string>(ri, 11).Trim(),
						};
						ri++;
					}

				}
			}

			public void Dispose()
			{
				_package.Dispose();
			}
		}

		internal class ExcelRow
		{
			public int ExternalId { get; set; }
			public string Grouping { get; set; }
			public string Street { get; set; }
			public string CivicNumber { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string PhoneNumber { get; set; }
			public string MobilePhoneNumber { get; set; }
			public string City { get; set; }
			public string Category { get; set; }
			public string PostalCode { get; set; }
		}

	}
}
