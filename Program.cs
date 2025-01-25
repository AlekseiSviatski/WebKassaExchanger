using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.IO;
using System.Windows.Forms;
using WebKassa;
using WebKassa.Models;

namespace WebKassaExchanger
{
	internal static class Program
	{
		private static string _connectionString = string.Empty;
		public static string sysPath = String.Format(@"{0}\sys.ini", AppDomain.CurrentDomain.BaseDirectory);
		public static IConfiguration? Configuration;

		[STAThread]
		static void Main()
		{
			var builder = new ConfigurationBuilder().AddJsonFile("AppConfig.json", optional: true, reloadOnChange: true);
			Configuration = builder.Build();

			NLog.LogManager.Setup().LoadConfiguration(builder =>
			{
				var config = new NLog.Targets.FileTarget
				{
					Name = "logfile",
					FileName = "${basedir}/Logs/${shortdate}_log.txt",
					Layout = "${longdate} ${level} ${callsite} ${message} \n${exception:format=message,stackTrace:maxInnerExceptionLevel=10:innerFormat=message,stackTrace}"
				};

				builder.ForLogger().FilterMinLevel(LogLevel.Error).WriteTo(config);
			});

			GetConnectionStringFromUdl();

			ApplicationConfiguration.Initialize();

			Application.Run(new Exchanger(new WebKassaCore(
				new Uri(Configuration.GetSection("Account").GetSection("BaseUri").Value!),
				Configuration.GetSection("Account").GetSection("APIKey").Value!,
				new DBWork(_connectionString),
				Configuration.GetSection("PPS").Get<PPSConfig>()!)));
		}

		private static void GetConnectionStringFromUdl()
		{
			string connectionStr = string.Empty;
			string appPath = AppDomain.CurrentDomain.BaseDirectory;
			string fullPath = string.Format("{0}connect.udl", appPath);
			if (!File.Exists(fullPath))
			{
				File.Create(fullPath);
			}
			else
			{
				using (StreamReader sr = File.OpenText(fullPath))
				{
					string? line;

					while ((line = sr.ReadLine()) != null)
					{
						if (line.Contains("Data Source=") && line.Contains("Initial Catalog="))
							connectionStr = line.Replace("Provider=SQLOLEDB.1;", "");
					}
				}
			}
			_connectionString = connectionStr;
		}
	}
}