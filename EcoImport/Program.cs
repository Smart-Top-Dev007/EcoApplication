using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.ViewModel;
using OfficeOpenXml;
using EcoCentre.Models.Domain.Hubs.Queries;
using EcoCentre.Models.Domain.Hubs;

namespace EcoImport
{
    class Program
    {
        static void Main(string[] args)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var uow = new UnitOfWork("mongodb://localhost/eco");

            var clientRepo = new Repository<Client>(uow);
            var hubRepo = new Repository<Hub>(uow);
            var municipalityRepo = new Repository<Municipality>(uow);
            var addressRepo = new Repository<ClientAddress>(uow);
            var addressQuery = new ClientAddressQuery(addressRepo);
            var hubQuery = new HubDetailsQuery(hubRepo);

            using (var excel = new ExcelWrapper())
            {
                foreach (var row in excel.Rows)
                {
                    Console.WriteLine("Imprting: " + row);
                    var cmd = new ImportClientCommand(clientRepo, municipalityRepo, addressQuery, hubQuery);
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

                        });

                }

            }
        }
    }

    class ExcelWrapper : IDisposable
    {
        private ExcelPackage _pkg;
        public ExcelWrapper()
        {
            var file = new FileInfo("MILLE-ISLES.XLSx");
            if (!file.Exists)
                throw new Exception("File not found:" + file.FullName);
            _pkg = new ExcelPackage(file);
        }

        public IEnumerable<ExcelRow> Rows
        {
            get
            {
                var ri = 1;
                var sht = _pkg.Workbook.Worksheets[1];
                while (true)
                {
                    var id = sht.GetValue<string>(ri, 1);
                    if (string.IsNullOrEmpty(id)) break;

                    var postal = sht.GetValue<string>(ri, 6).Trim();
                    if (postal.Length == 6)
                        postal = postal.Insert(3, "-");
                    var street = string.Format("{0} {1} {2}",
                                               sht.GetValue<string>(ri, 11).Trim(),
                                               sht.GetValue<string>(ri, 12).Trim(),
                                               sht.GetValue<string>(ri, 13).Trim());
                    yield return new ExcelRow
                        {
                            RefId =  sht.GetValue<string>(ri,1).Trim(),
                            Name = sht.GetValue<string>(ri, 2).Trim(),
                            Street = street,
                            CivicNo = sht.GetValue<string>(ri,14).Trim(),
                            PostalCode = postal.Trim(),
                            City = "GORE"

                        };
                    ri++;
                }

            }
        }

        public void Dispose()
        {
            _pkg.Dispose();
        }
    }

    class ExcelRow
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { 
                _name = value;
                if (_name.EndsWith(" INC.") || _name.EndsWith(" INC"))
                {
                    LastName = _name;
                    return;
                }
                var tokens = _name.Split(' ');
                if (tokens.Length < 1) return;
                if (tokens.Length == 1)
                {
                    LastName = tokens[0];
                    return;
                }
                if (tokens.Length > 1)
                {
                    var fn = "";
                    for (var ti = 1; ti < tokens.Length; ti++)
                    {
                        fn += tokens[ti] + " ";
                    }
                    FirstName = fn.Trim();
                    LastName = tokens[0];
                }

            }
        }
        public string RefId { get; set; }
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string PostalCode { get; set; }

        public string Street { get; set; }

        public string CivicNo { get; set; }

        public string City { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, RefId: {1}, FirstName: {2}, LastName: {3}, PostalCode: {4}, Street: {5}, CivicNo: {6}", Name, RefId, FirstName, LastName, PostalCode, Street, CivicNo);
        }
    }

}
