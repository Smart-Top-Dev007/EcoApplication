using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EcoCitizenImport.Import
{
	internal static class CitizenExcelColumnNames
	{
		internal static readonly string[] CitizenCard = new[] { "NO_CLE_ADR" };
		internal static readonly string[] FirstName = new[] { "PNOM" };
		internal static readonly string[] LastName = new[] { "NOM" };
		internal static readonly string[] Street = new[] { "LUDIK_LUD_T_RUE_VILLE1_NOM_RUE", "LUDIK_LUD_T_RUE_VILLE1.NOM_RUE" };
		internal static readonly string[] CivicNumber = new[] { "NO_CIV" };
		internal static readonly string[] PostalCode = new[] { "CODE_POST" };
		internal static readonly string[] Gender = new[] { "SEXE" };
		internal static readonly string[] PhoneNumber = new[] { "TEL_MAIS" };
		internal static readonly string[] DateFinMember = new[] { "DATE_FIN_MEMBRE" };
		internal static readonly string[] City = new[] { "VILLE" };
		internal static readonly string[] Email = new[] { "ADR_EMAIL" };
		internal static readonly string[] AptNumber = new[] { "APP" };

		internal static readonly string[] Names;

		static CitizenExcelColumnNames()
		{
			Names = typeof(CitizenExcelColumnNames)
				.GetFields(BindingFlags.Static | BindingFlags.NonPublic)
				.Where(fi => fi.IsInitOnly && fi.Name != nameof(Names))
				.SelectMany(e => e.GetValue(null) as string[])
				.Where(s => s != null).ToArray();
		}
	}
}
