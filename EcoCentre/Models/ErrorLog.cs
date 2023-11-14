using System;
using System.Web;
using Elmah;

namespace EcoCentre.Models
{
    public class ErrorLog
    {
        public static void Log(Exception e)
        {
	        if (HttpContext.Current == null)
	        {
		        Elmah.ErrorLog.GetDefault(null).Log(new Error(e));
	        }
	        else
	        {
		        ErrorSignal.FromCurrentContext().Raise(e);
	        }
        }
    }
}