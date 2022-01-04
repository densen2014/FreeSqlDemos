using System;
using xamarinFormApp.Services;
using Xamarin.Forms;
using System.IO;
using System.Data.Common;

[assembly: Dependency (typeof (SQLite_droid))]

namespace xamarinFormApp.Services
{
	public class SQLite_droid : ISQLite
	{
		public SQLite_droid()
		{
		}

		public DbConnection GetConnectionSqlite(string dbname)
		{
			string MicrosoftdbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"{dbname}.db");
			var connv3 = new Mono.Data.Sqlite.SqliteConnection($"Data Source={MicrosoftdbPath};");
			//connv3.Open();
			return connv3;
		}
	}
}
