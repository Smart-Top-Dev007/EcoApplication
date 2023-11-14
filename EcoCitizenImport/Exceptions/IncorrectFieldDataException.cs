using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoCitizenImport.Exceptions
{
	internal class IncorrectFieldDataException : ImportException
	{
		internal IncorrectFieldDataException(string propertyName, object propertyValue) : base(
			$"Wrong {propertyName} value: {propertyValue}")
		{

		}
	}
}
