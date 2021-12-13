using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PROG8050_PROJECT.Core.Services
{
	interface IDBService
	{
		public bool IsOpen { get; }

		public DataTable ExecuteReader(string sql, Dictionary<string, object> parameters = null);
		public List<T> ExecuteReader<T>(string sql, Dictionary<string, object> parameters = null) where T : class, new();
		public int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null);
	}
}
