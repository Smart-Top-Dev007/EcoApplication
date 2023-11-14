using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoCitizenImport.Tools
{
	public static class ExceptionTools
	{
		public static string FullErrorMessage(this Exception ex)
		{
			var sb = new StringBuilder();
			while (ex != null)
			{
				sb.AppendLine(ex.Message);
				ex = ex.InnerException;
			}
			return sb.ToString();
		}

		public static string FullErrorStackTrace(this Exception ex)
		{
			var sb = new StringBuilder();
			while (ex != null)
			{
				sb.AppendLine(ex.StackTrace);
				ex = ex.InnerException;
			}
			return sb.ToString();
		}
	}
}
