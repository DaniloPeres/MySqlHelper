using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.DataBase
{
    public static class MySqlParameterHelper
    {
        public static MySqlParameter CreateSqlParameter(string parameterName, MySqlDbType type, object value)
        {
            return new MySqlParameter(parameterName, type) { Value = value };
        }

        public static List<MySqlParameter> CreateSqlParameters(params MySqlParameter[] args)
        {
            var parameters = new List<MySqlParameter>();
            foreach (MySqlParameter arg in args)
                parameters.Add(arg);
            return parameters;
        }
    }
}
