using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Import
{
    public class ExcelRow
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
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