using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlHelper.DataBase
{
    public class DataBaseExecuteReader : IDisposable
    {
        public readonly MySqlDataReader DataReader;
        private readonly MySqlConnection myCon;
        private readonly MySqlCommand cmd;

        public DataBaseExecuteReader(string connectionString, string query, List<MySqlParameter> parameters = null)
        {
            myCon = new MySqlConnection(connectionString);
            myCon.Open();

            cmd = new MySqlCommand(query, myCon);
            parameters?.ForEach(parameter => cmd.Parameters.Add(parameter));
            DataReader = cmd.ExecuteReader();
        }

        public void Dispose()
        {
            DataReader?.Dispose();
            cmd?.Dispose();
            myCon?.Dispose();
        }
    }
}
