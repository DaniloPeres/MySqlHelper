using MySqlHelper.DataBase;
using MySqlHelper.IntegrationTests.Configuration;
using static MySqlHelper.Attributes.TableAttribute;

namespace MySqlHelper.IntegrationTests.Helper
{
    public static class DataBase
    {
        public static void CleanTable<T>() where T : new()
        {
            var table = GetTableName<T>();
            var cleanTableQuery = $"TRUNCATE {table}";
            DataBaseDataReader.ExecuteNonQuery(new ConfigurationSettings().ConnectionString, cleanTableQuery);
        }
    }
}
