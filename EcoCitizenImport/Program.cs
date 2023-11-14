using EcoCitizenImport.Import;
using EcoCitizenImport.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoCitizenImport
{
	class Program
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private const string _allSheetsParameter = "-all";

		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			try
			{
				string fileName = null;
				var sheets = new List<int>();
				string dbConnectionString = null;

				var showHelp = false;
				if (args.Length < 2)
				{
					showHelp = true;
				}
				else
				{
					fileName = args[0];
					dbConnectionString = args[1];
					if (args.Length > 2)
					{
						var sheetsStr = args[2];
						if (sheetsStr != _allSheetsParameter)
						{
							try
							{
								sheets = sheetsStr.Split(',').Select(s => int.Parse(s.Trim())).ToList();
							}
							catch
							{
								Logger.Error($"Sequence {sheetsStr} couldn't be parsed");
								showHelp = true;
							}
						}
					}
				}
				if (showHelp)
				{
					Logger.Info($"EcoCitizenImport usage:");
					Logger.Info($"EcoCitizenImport.exe <path to Excel file> <database connection string> [optional: -all|',' delimited int sequence of shit numbers starting from 0 (0,1,2,5)]");
					Environment.Exit(-1);
				}
				var importer = new CitizenImporter(fileName, dbConnectionString, sheets);
				var result = importer.Import();
				Logger.Info($"Import {(result ? string.Empty : "not ")}successfull");
				Environment.Exit(result ? 0 : -1);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Import failed:{Environment.NewLine}{ex.FullErrorMessage()}");
				Environment.Exit(-1);
			}
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = e.ExceptionObject as Exception;
			var message = $"Unhandled exception: {ex?.FullErrorMessage() ?? e.ExceptionObject?.ToString() ?? "<ExceptionObject is null>"}";
			try
			{
				Console.WriteLine(message);
				Logger.Error(ex, message);
			}
			catch
			{

			}
		}
	}
}
