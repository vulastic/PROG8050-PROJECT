using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PROG8050_PROJECT
{
	public sealed class SQLiteDBManager : Singleton<SQLiteDBManager>
	{
		private string dataSource = @"Data Source=database.sqlite";

		#region Connection

		private SQLiteConnection connection;
		public SQLiteConnection Connection => connection;

		public SQLiteDBManager()
		{
			if (dataSource != null)
			{
				Connect(dataSource);
			}
		}

		~SQLiteDBManager()
		{
			if (connection != null)
			{
				connection.Close();
			}
		}

		public void Connect(string dataSoruce)
		{
			connection = new SQLiteConnection(dataSoruce);
			connection.Open();
		}
		
		public void Close()
		{
			connection.Close();
			connection = null;
		}

		public bool IsOpen()
		{
			return connection != null && connection.State == System.Data.ConnectionState.Open;
		}

		#endregion

		#region Getter Methods

		/*
		public T ExecuteReader<T>(string sql, Dictionary<string, object> parameters = null) where T : class, new()
		{
			T result = null;
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = sql;

				if (parameters != null)
				{
					foreach (KeyValuePair<string, object> param in parameters)
					{
						SQLiteParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = param.Key;
						parameter.Value = param.Value;
						cmd.Parameters.Add(parameter);
					}
				}

				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						result = BuildData<T>(reader);
					}
				}
			}

			return result;
		}
		*/

		/*
		public T ExecuteReader<T>(int id) where T : class, new()
		{
			string name = typeof(T).Name;
			string sql = $"SELECT * FROM {name} WHERE {name}ID = @id LIMIT 1";

			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters["@id"] = id;

			return ExecuteReader<T>(sql, parameters);
		}
		*/

		public List<T> ExecuteReader<T>(string sql, Dictionary<string, object> parameters = null) where T : class, new()
		{
			List<T> result = new List<T>();
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = sql;

				if (parameters != null)
				{
					foreach(KeyValuePair<string, object> param in parameters)
					{
						SQLiteParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = param.Key;
						parameter.Value = param.Value;
						cmd.Parameters.Add(parameter);
					}
				}

				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						T data = BuildData<T>(reader);
						result.Add(data);
					}
				}
			}
			return result;
		}

		public SQLiteDataReader ExecuteReader(string sql, Dictionary<string, object> parameters = null)
		{
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = sql;
				if (parameters != null)
				{
					foreach (KeyValuePair<string, object> param in parameters)
					{
						SQLiteParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = param.Key;
						parameter.Value = param.Value;
						cmd.Parameters.Add(parameter);
					}
				}
				return cmd.ExecuteReader();
			}
		}

		public List<string> GetTableNames()
		{
			List<string> names = new List<string>();
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = string.Format("SELECT name FROM sqlite_master WHERE type='table';");
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						names.Add(reader["name"].ToString());
					}
				}
			}
			return names;
		}
		#endregion

		#region Data Manipulation Methods

		public int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
		{
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = sql;
				if (parameters != null)
				{
					foreach (KeyValuePair<string, object> param in parameters)
					{
						SQLiteParameter parameter = cmd.CreateParameter();
						parameter.ParameterName = param.Key;
						parameter.Value = param.Value;
						cmd.Parameters.Add(parameter);
					}
				}
				return cmd.ExecuteNonQuery();
			}
		}

		public void TruncateTalbe(string tableName)
		{
			using(SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = $"DELETE FROM {tableName};";

				int rowsAffected = cmd.ExecuteNonQuery();
				Debug.WriteLine($"{tableName}: {rowsAffected} rows affected.");

				cmd.CommandText = "VACUUM;";
				cmd.ExecuteNonQuery();
			}
		}
		#endregion

		#region Helper Functions

		public T BuildData<T>(SQLiteDataReader reader) where T : class, new()
		{
			T data = new T();

			foreach(var prop in data.GetType().GetProperties())
			{
				try
				{
					if (ColumnExists(reader, prop.Name))
					{
						PropertyInfo propertyInfo = data.GetType().GetProperty(prop.Name);
						if (reader[prop.Name] == DBNull.Value)
						{
							propertyInfo.SetValue(data, null, null);
						}
						else
						{
							Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
							object safeValue = reader[prop.Name] == null ? null : Convert.ChangeType(reader[prop.Name], t);
							propertyInfo.SetValue(data, safeValue, null);
						}
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e);
					continue;
				}
			}

			return data;
		}

		public bool ColumnExists(SQLiteDataReader reader, string columnName)
		{
			return reader.GetSchemaTable().Rows.OfType<DataRow>().Any(row => row["ColumnName"].ToString() == columnName);
		}

		public static string GetParseSql(string sql, Dictionary<string, object> parameters = null)
		{
			if (parameters != null)
			{
				foreach(KeyValuePair<string, object> param in parameters)
				{
					sql = sql.Replace(param.Key, param.Value.ToString());
				}
			}
			return sql;
		}
		#endregion

	}
}
