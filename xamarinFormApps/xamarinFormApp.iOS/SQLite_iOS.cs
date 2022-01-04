using System;
using xamarinFormApp.Services;
using Xamarin.Forms;
using System.IO; 
using Mono.Data.Sqlite;
using System.Data.Common;

[assembly: Dependency (typeof (SQLite_iOS))]

namespace xamarinFormApp.Services
{
	public class SQLite_iOS : ISQLite
	{
		public SQLite_iOS ()
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
