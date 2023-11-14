using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace EcoCentre.Models.Import
{
    public class ExcelWrapper : IDisposable
    {
        public bool IsProduction { get; set; }
        private ExcelPackage _pkg;
        public ExcelWrapper()
        {
            var file = new FileInfo("customers.xlsx");
            if (!file.Exists)
                throw new Exception("File not found:" + file.FullName);
            _pkg = new ExcelPackage(file);
        }

        public ExcelWrapper(HttpPostedFileBase baseFile)
        {
            _pkg = new ExcelPackage(baseFile.InputStream);
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

                    string postal = "";
                    string street = "";
                    string civicNo = "";

                    string name = "";
                    string city = "";


                    var hub = Hubs.FirstOrDefault(h => h.Id == HubId);
                    if (hub != null)
                    {
                        if (hub.Name.ToLower().Contains("gore"))
                        {
                            civicNo = sht.GetValue<string>(ri, 14);
                            postal = sht.GetValue<string>(ri, 6);
                            street = sht.GetValue<string>(ri, 11) + " " + sht.GetValue<string>(ri, 13);

                            //string ctmp = sht.GetValue<string>(ri, 5);
                            //if (!String.IsNullOrWhiteSpace(ctmp)) 
                            //{
                            //    if (ctmp.Contains(","))
                            //    {
                            //        city = ctmp.Split(',')[0];
                            //    }
                            //    else
                            //    {
                            //        city = ctmp;
                            //    }
                            //}
                            //else
                            //{
                                city = "Gore";
                          //  }

                            name = sht.GetValue<string>(ri, 2);

                        }
                        else if (hub.Name.ToLower().Contains("milee"))
                        {
                            civicNo = sht.GetValue<string>(ri, 9);
                            postal = sht.GetValue<string>(ri, 6);
                            street = sht.GetValue<string>(ri, 7) + " " + sht.GetValue<string>(ri, 13);

                            //string ctmp = sht.GetValue<string>(ri, 5);
                            //if (!String.IsNullOrWhiteSpace(ctmp))
                            //{
                            //    if (ctmp.Contains(","))
                            //    {
                            //        city = ctmp.Split(',')[0];
                            //    }
                            //    else
                            //    {
                            //        city = ctmp;
                            //    }
                            //}
                            //else
                            //{
                            city = "Mille-Isles";
                           // }

                            name = sht.GetValue<string>(ri, 2);
                        }
                        else if (hub.Name.ToLower().Contains("wentworth"))
                        {
                            civicNo = sht.GetValue<string>(ri, 7);
                           // postal = sht.GetValue<string>(ri, 6);
                            string mstreet = sht.GetValue<string>(ri, 5);
                            if (String.IsNullOrWhiteSpace(mstreet))
                            {
                                street = sht.GetValue<string>(ri, 4) + " " + sht.GetValue<string>(ri, 6);
                            }
                            else
                            {
                                street = sht.GetValue<string>(ri, 4) + " " + mstreet + " " + sht.GetValue<string>(ri, 6);
                            }

                            //string ctmp = sht.GetValue<string>(ri, 5);
                            //if (!String.IsNullOrWhiteSpace(ctmp))
                            //{
                            //    if (ctmp.Contains(","))
                            //    {
                            //        city = ctmp.Split(',')[0];
                            //    }
                            //    else
                            //    {
                            //        city = ctmp;
                            //    }
                            //}
                            city = "Wentworth";
                            name = sht.GetValue<string>(ri, 2);
                        }
                    }
                  

                    if (!String.IsNullOrWhiteSpace(name))
                        name = name.Trim();

                    if (!String.IsNullOrWhiteSpace(civicNo))
                        civicNo = civicNo.Trim();

                    if (!String.IsNullOrWhiteSpace(street))
                        street = street.Trim();

                    if (!String.IsNullOrWhiteSpace(postal))
                        postal = postal.Trim();

                    if (!String.IsNullOrWhiteSpace(city))
                        city = city.Trim();

                    yield return new ExcelRow
                    {
                        //RefId = sht.GetValue<string>(ri, 1).Trim(),
                        Name = name,
                        Street = street,
                        CivicNo = civicNo,
                        PostalCode = postal.Trim(),
                        City = city

                    };
                    ri++;
                }

            }
        }

        public void Dispose()
        {
            _pkg.Dispose();
        }

        public IEnumerable<Domain.Hubs.Hub> Hubs { get; set; }

        public string HubId { get; set; }
    }
}