using System;
using System.Data.Common; 
 
namespace xamarinFormApp.Services
{
	public interface ISQLite
	{
		//SqliteConnection GetConnection();
		DbConnection GetConnectionSqlite(string dbname);
	}
}

