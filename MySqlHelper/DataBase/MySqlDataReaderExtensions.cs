using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlHelper.DataBase
{
    public static class MySqlDataReaderExtensions
    {
        public static string GetString(this MySqlDataReader myDrr, int fieldIndex)
        {
            return myDrr.IsDBNull(fieldIndex) ? null : myDrr[fieldIndex].ToString();
        }

        public static string GetString(this MySqlDataReader myDrr, string field)
        {
            return myDrr[field]?.ToString();
        }

        public static byte[] GetByteArray(this MySqlDataReader myDrr, int fieldIndex)
        {
            return myDrr.IsDBNull(fieldIndex) ? null : (byte[])myDrr[fieldIndex];
        }

        public static int GetInt(this MySqlDataReader myDrr, int fieldIndex)
        {
            var output = 0;
            if (!myDrr.IsDBNull(fieldIndex))
                int.TryParse(myDrr[fieldIndex].ToString(), out output);
            return output;
        }

        public static int GetInt(this MySqlDataReader myDrr, string field)
        {
            int.TryParse(myDrr[field].ToString(), out var output);
            return output;
        }

        public static short GetShort(this MySqlDataReader myDrr, int fieldIndex)
        {
            return (short)GetInt(myDrr, fieldIndex);
        }

        public static ushort GetUShort(this MySqlDataReader myDrr, int fieldIndex)
        {
            return (ushort)GetInt(myDrr, fieldIndex);
        }

        public static byte GetByte(this MySqlDataReader myDrr, int fieldIndex)
        {
            return (byte)GetInt(myDrr, fieldIndex);
        }

        public static ulong GetULong(this MySqlDataReader myDrr, int fieldIndex)
        {
            ulong output = 0;
            if (!myDrr.IsDBNull(fieldIndex))
                ulong.TryParse(myDrr[fieldIndex].ToString(), out output);
            return output;
        }

        public static long GetLong(this MySqlDataReader myDrr, int fieldIndex)
        {
            long output = 0;
            if (!myDrr.IsDBNull(fieldIndex))
                long.TryParse(myDrr[fieldIndex].ToString(), out output);
            return output;
        }

        public static long GetLong(this MySqlDataReader myDrr, string field)
        {
            long.TryParse(myDrr[field].ToString(), out var output);
            return output;
        }

        public static uint GetUInt(this MySqlDataReader myDrr, int fieldIndex)
        {
            uint output = 0;
            if (!myDrr.IsDBNull(fieldIndex))
                uint.TryParse(myDrr[fieldIndex].ToString(), out output);
            return output;
        }

        public static DateTime GetDate(this MySqlDataReader myDrr, int fieldIndex)
        {
            DateTime.TryParse(myDrr[fieldIndex].ToString(), out var output);
            return output;
        }

        public static DateTime GetDate(this MySqlDataReader myDrr, string field)
        {
            return myDrr.GetDateTime(field);
        }

        public static TimeSpan GetTime(this MySqlDataReader myDrr, string field)
        {
            TimeSpan.TryParse(myDrr[field].ToString(), out var output);
            return output;
        }

        public static double GetDecimal(this MySqlDataReader myDrr, int fieldIndex)
        {
            double.TryParse(myDrr[fieldIndex].ToString(), out var output);
            return output;
        }

        public static double GetDecimal(this MySqlDataReader myDrr, string field)
        {
            double.TryParse(myDrr[field].ToString(), out var output);
            return output;
        }

        public static bool GetBool(this MySqlDataReader myDrr, int fieldIndex)
        {
            if (myDrr.IsDBNull(fieldIndex))
                return false;
            if (myDrr[fieldIndex].ToString() == "1")
                return true;
            bool.TryParse(myDrr[fieldIndex].ToString(), out var output);
            return output;
        }

        public static bool GetBool(this MySqlDataReader myDrr, string field)
        {
            if (myDrr[field].ToString() == "1")
                return true;
            bool.TryParse(myDrr[field].ToString(), out var output);
            return output;
        }
    }
}
