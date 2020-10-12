using MySql.Data.MySqlClient;
using System;
using System.Globalization;

namespace MySqlHelper.Extensions
{
    public static class MySqlDataReaderExtensions
    {
        public static string GetDataStr(this MySqlDataReader myDRR, int col)
        {
            var _return = "";
            if (!myDRR.IsDBNull(col)) _return = myDRR[col].ToString();
            return _return;
        }

        public static string GetDataStr(this MySqlDataReader myDRR, string col)
        {
            var _return = "";
            _return = myDRR[col].ToString();
            return _return;
        }

        public static byte[] GetDataByteArray(this MySqlDataReader myDRR, int col)
        {
            if (!myDRR.IsDBNull(col)) return (byte[])myDRR[col];
            return null;
        }

        public static int GetDataInt(this MySqlDataReader myDRR, int col)
        {
            int _return = 0;
            if (!myDRR.IsDBNull(col)) int.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static int GetDataInt(this MySqlDataReader myDRR, string col)
        {
            int _return = 0;
            int.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static short GetDataShort(this MySqlDataReader myDRR, int col)
        {
            return (short)GetDataInt(myDRR, col);
        }

        public static ushort GetDataUShort(this MySqlDataReader myDRR, int col)
        {
            return (ushort)GetDataInt(myDRR, col);
        }

        public static byte GetDataByte(this MySqlDataReader myDRR, int col)
        {
            return (byte)GetDataInt(myDRR, col);
        }

        public static ulong GetDataULong(this MySqlDataReader myDRR, int col)
        {
            ulong _return = 0;
            if (!myDRR.IsDBNull(col)) ulong.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static long GetDataLong(this MySqlDataReader myDRR, int col)
        {
            long _return = 0;
            if (!myDRR.IsDBNull(col)) long.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static long GetDataLong(this MySqlDataReader myDRR, string col)
        {
            long _return = 0;
            long.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static uint GetDataUInt(this MySqlDataReader myDRR, int col)
        {
            uint _return = 0;
            if (!myDRR.IsDBNull(col)) uint.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static DateTime GetDataDate(this MySqlDataReader myDRR, int col)
        {
            DateTime _return = new DateTime();
            DateTime.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static DateTime GetDataDate(this MySqlDataReader myDRR, string col)
        {
            return myDRR.GetDateTime(col);
        }

        public static string GetDataDateAsString(this MySqlDataReader myDRR, string col)
        {
            return GetDataDate(myDRR, col).ToString("dd/MM/yyyy");
        }

        public static TimeSpan GetDataHora(this MySqlDataReader myDRR, string col)
        {
            TimeSpan.TryParse(myDRR[col].ToString(), out TimeSpan _return);
            return _return;
        }

        public static double GetDataDecimal(this MySqlDataReader myDRR, int col)
        {
            double.TryParse(myDRR[col].ToString(), out var _return);
            return _return;
        }

        public static double GetDataDecimal(this MySqlDataReader myDRR, string col)
        {
            double.TryParse(myDRR[col].ToString(), out var _return);
            return _return;
        }

        public static bool GetDataBool(this MySqlDataReader myDRR, int col)
        {
            Boolean _return = false;
            if (myDRR[col].ToString() == "1") return true;
            if (!myDRR.IsDBNull(col)) bool.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static bool GetDataBool(this MySqlDataReader myDRR, string col)
        {
            Boolean _return = false;
            if (myDRR[col].ToString() == "1") return true;
            Boolean.TryParse(myDRR[col].ToString(), out _return);
            return _return;
        }

        public static String SaveInt(object value)
        {
            return SaveIntWithError(value).result;
        }

        public static (bool error, string result) SaveIntWithError(object value)
        {
            return (!int.TryParse(value.ToString(), out var _return), _return.ToString());
        }

        public static String SaveBool(string value)
        {
            bool.TryParse(value, out var valueBool);
            return SaveBool(valueBool);
        }

        public static String SaveBool(Boolean value)
        {
            return SaveInt(value ? 1 : 0);
        }

        public static String SaveUInt(object value)
        {
            uint _return = 0;
            uint.TryParse((string)value.ToString(), out _return);
            return _return.ToString();
        }

        public static String SaveLong(object value)
        {
            long _return = 0;
            long.TryParse((string)value.ToString(), out _return);
            return _return.ToString();
        }

        public static String SaveULong(object value)
        {
            ulong _return = 0;
            ulong.TryParse(value.ToString(), out _return);
            return _return.ToString();
        }


        public static String SaveFloat(object value)
        {
            var result = value.ToString().ConvertToDecimal();
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            return result.valor.ToString(culture);
        }
        public static String SaveDouble(object value)
        {
            var result = value.ToString().ConvertToDecimal();
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            return result.valor.ToString(culture);
        }

        public static String SaveDate(object value)
        {
            DateTime _return = new DateTime();
            DateTime.TryParse((string)value.ToString(), out _return);
            return "'" + _return.ToString("yyyy-MM-dd") + "'";
        }

        public static String SaveDateTime(object value)
        {
            DateTime _return = new DateTime();
            DateTime.TryParse((string)value.ToString(), out _return);
            return "'" + _return.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }

        public static String SaveTime(object value)
        {
            DateTime _return = new DateTime();
            DateTime.TryParse((string)value.ToString(), out _return);
            return "'" + _return.ToString("HH:mm:ss") + "'";
        }
        public static String SaveString(object value, int maxLen = 0, bool addAspas = true)
        {
            if (value == null) value = "";
            String _return = value.ToString();
            if (maxLen > 0 && _return.Length > maxLen)
                _return = _return.Substring(0, maxLen);
            _return = _return.Replace("'", "''");
            _return = _return.Replace("\\", "\\\\");
            if (addAspas)
                return "'" + _return + "'";
            else
                return _return;
        }
    }
}

