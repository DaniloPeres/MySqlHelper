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

        public MySqlDataReader ExecuteReader(string strQuery, List<MySqlParameter> parameters = null)
        {
            using (var con = new MySqlConnection(ConnectionString))
            {
                con.Open();
                var cmd = new MySqlCommand(strQuery, con);
                parameters?.ForEach(parameter => cmd.Parameters.Add(parameter));
                return cmd.ExecuteReader();
            }
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

        public Result<int> GetLastInsertId()
        {
            using (var myDrr = ExecuteReader("SELECT LAST_INSERT_ID()"))
            {
                if (myDrr.Read())
                    return myDrr.GetInt(0);
            }
            return Result.Failure<int>("Error to get Last Insert Id, no data returned.");
        }
    }
}
