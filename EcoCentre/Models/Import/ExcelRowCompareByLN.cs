using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Import
{
    public class ExcelRowCompareByLN : IEqualityComparer<ExcelRow>
    {
        public bool Equals(ExcelRow x, ExcelRow y)
        {
            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal. 
            return x.LastName == y.LastName
                && x.CivicNo == y.CivicNo 
                && x.Street == y.Street;
        }

        public int GetHashCode(ExcelRow obj)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;


            //Get hash code for the Code field. 
            int hashLastName = obj.LastName != null ? obj.LastName.GetHashCode() : 0;

            int hashStreet = obj.Street != null ? obj.Street.GetHashCode() : 0;


            int hashCivicNo = obj.CivicNo != null ? obj.CivicNo.GetHashCode() : 0;


            //Calculate the hash code for the product. 
            return  hashLastName ^ hashStreet
                  ^ hashCivicNo ;
        }
    }
}