using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoCitizenImport.Exceptions
{
	internal class ImportException : Exception
	{
		internal ImportException(string message, Exception innerException) : base(message, innerException)
		{

		}

		internal ImportException(string message) : base(message)
		{

		}
	}
}
