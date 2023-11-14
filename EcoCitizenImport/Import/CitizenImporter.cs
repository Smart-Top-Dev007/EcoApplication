using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Infrastructure;
using EcoCitizenImport.Tools;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Hubs.Queries;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.ViewModel;
using EcoCitizenImport.Exceptions;

namespace EcoCitizenImport.Import
{
	internal class CitizenImporter
	{
		protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly string _fileName;
		private readonly string _dbConnectionString;
		private readonly List<int> _sheets;
		private readonly UnitOfWork _uow;
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<ClientAddress> _addressRepository;
		private readonly Repository<Municipality> _municipalityRepository;
		private readonly Repository<Hub> _hubRepository;
		private readonly ClientAddressQuery _clientAddressQuery;
		private readonly HubDetailsQuery _hubDetailsQuery;

		internal CitizenImporter(string fileName, string dbConnectionString, List<int> sheets)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			_fileName = fileName;
			_dbConnectionString = dbConnectionString;
			_sheets = sheets;
			_uow = new UnitOfWork(dbConnectionString);
			_clientRepository = new Repository<Client>(_uow);
			_addressRepository = new Repository<ClientAddress>(_uow);
			_municipalityRepository = new Repository<Municipality>(_uow);
			_hubRepository = new Repository<Hub>(_uow);
			_clientAddressQuery = new ClientAddressQuery(_addressRepository);
			_hubDetailsQuery = new HubDetailsQuery(_hubRepository);
		}

		internal bool Import()
		{
			try
			{
				Logger.Info($"Import from {_fileName} to {_dbConnectionString} started");
				var filePath = Path.GetFullPath(_fileName);
				if (!File.Exists(filePath)) throw new FileNotFoundException("Couldn't find input file", filePath);

				using (var ep = new ExcelPackage(new FileInfo(filePath)))
				{
					ExcelWorksheet[] worksheets;
					if (_sheets.Count == 0)
					{
						worksheets = ep.Workbook.Worksheets.ToArray();
					}
					else
					{
						worksheets = ep.Workbook.Worksheets.Where((ws, i) => _sheets.Contains(i)).ToArray();
					}

					Logger.Trace($"Selected sheets: {string.Join(",", worksheets.Select(w => w.Name))}");
					foreach (var worksheet in worksheets)
					{
						var mapping = MapWorksheet(worksheet);
						if (mapping.Count == 0) throw new ImportException("Failed to find any columns");
						ReadAndImport(worksheet, mapping);
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Failed to import:{Environment.NewLine}{ex.FullErrorMessage()}");
				return false;
			}
		}

		private DataMapping MapWorksheet(ExcelWorksheet worksheet)
		{
			Logger.Trace($"Mapping worksheet {worksheet.Name}");
			var mapping = new DataMapping();
			var read = true;
			var column = 1;
			while (read)
			{
				var columnName = worksheet.GetValue<string>(1, column);
				if (string.IsNullOrWhiteSpace(columnName))
				{
					read = false;
				}
				else if (CitizenExcelColumnNames.Names.Contains(columnName))
				{
					mapping.SetColumn(columnName, column);
				}

				column++;
			}
			Logger.Trace($"Mapping worksheet {worksheet.Name} done: {mapping}");
			return mapping;
		}

		private void ReadAndImport(ExcelWorksheet worksheet, DataMapping mapping)
		{
			Logger.Trace($"ReadAndImport() for worksheet {worksheet.Name}");
			var props = mapping.Properties;
			for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
			{
				var dataRow = new WorksheetDataRow();
				for (var i = 0; i < props.Length; i++)
				{
					var propMapping = props[i];
					dataRow.Add(propMapping.ColumnName, new PropertyValue(propMapping.ColumnName, worksheet.Cells[row, propMapping.Column].Value));
				}

				if (row % 1000 == 0)
				{
					Logger.Info($"Parsed {row}/{worksheet.Dimension.End.Row} rows for worksheet {worksheet.Name}");
				}
				Logger.Trace($"Parsing worksheet {worksheet.Name} row {row}: {dataRow}");
				ImportToDatabase(dataRow);
			}
		}

		private void ImportToDatabase(WorksheetDataRow row)
		{
			var command = new ImportClientCommand(_clientRepository, _municipalityRepository, _clientAddressQuery,
				_hubDetailsQuery);
			var hub = _hubRepository.Query.FirstOrDefault();
			if (hub == null) throw new ImportException($"No Hub found");
			var address = new ClientAddressImportModel
			{
				AptNumber = row.GetStringColumnValue(CitizenExcelColumnNames.AptNumber),
				City = row.GetStringColumnValue(CitizenExcelColumnNames.City),
				CivicNumber = row.GetStringColumnValue(CitizenExcelColumnNames.CivicNumber),
				PostalCode = row.GetStringColumnValue(CitizenExcelColumnNames.PostalCode),
				Street = row.GetStringColumnValue(CitizenExcelColumnNames.Street)
			};
			var clientModel = new ClientImportModel
			{
				Category = ClientCategory.Resident.ToString(),
				HubId = hub.Id,
				Address = address,
				Email = row.GetStringColumnValue(CitizenExcelColumnNames.Email),
				FirstName = row.GetStringColumnValue(CitizenExcelColumnNames.FirstName),
				LastName = row.GetStringColumnValue(CitizenExcelColumnNames.LastName),
				PhoneNumber = row.GetStringColumnValue(CitizenExcelColumnNames.PhoneNumber),
				CitizenCard = row.GetStringColumnValue(CitizenExcelColumnNames.CitizenCard),
				DateFinMember = row.GetDateTimeColumnValue(CitizenExcelColumnNames.DateFinMember)
			};
			var genderInt = row.GetIntColumnValue(CitizenExcelColumnNames.Gender);
			switch (genderInt)
			{
				case 0:
					clientModel.Gender = Genders.Female;
					break;
				case 1:
					clientModel.Gender = Genders.Male;
					break;
				default:
					throw new IncorrectFieldDataException(nameof(clientModel.Gender), genderInt);
			}
			clientModel.Status = clientModel.DateFinMember < DateTime.Now ? ClientStatus.Inactive : ClientStatus.Active;
			command.Execute(clientModel, true);
		}
	}
}
