using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MySql.Data.MySqlClient;

namespace MySqlHelper.DataBase
{
    public class DataBaseDataReader
    {
        internal readonly string ConnectionString;

        public DataBaseDataReader(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static void ExecuteNonQuery(string connectionString, string strQuery, List<MySqlParameter> parameters = null)
        {
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new MySqlCommand
                {
                    CommandText = strQuery,
                    CommandType = System.Data.CommandType.Text,
                    Connection = con
                })
                {
                    parameters?.ForEach(parameter => cmd.Parameters.Add(parameter));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static ulong GetLastInsertId(string connectionString)
        {
            var query = "SELECT LAST_INSERT_ID()";
            using (var exe = new DataBaseExecuteReader(connectionString, query))
            {
                if (exe.DataReader.Read())
                    return exe.DataReader.GetULong(0);
            }
            return 0;
        }
    }
}
