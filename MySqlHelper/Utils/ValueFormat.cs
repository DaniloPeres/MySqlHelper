using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MySqlHelper.Attributes;

namespace MySqlHelper.Utils
{
    public static class ValueFormat
    {
        public static string GenerateFormattedValueForQuery(object value)
        {
            if (value == null)
                return "NULL";

            var type = value.GetType();
            var attributes = type.GetTypeInfo().GetCustomAttributes(typeof(ColumnAttribute), true);

            if (attributes.Any())
            {
                switch (((ColumnAttribute)attributes[0]).Type)
                {
                    case ColumnTypeEnum.Date:
                        return DateFormat((DateTime)value);
                    case ColumnTypeEnum.Time:
                        return TimeFormat((DateTime)value);
                }
            }

            return value switch
            {
                string stringValue => $"'{StringFormat(stringValue)}'",
                bool boolValue => BoolFormat(boolValue),
                byte byteValue => NumberFormat(byteValue),
                sbyte sbyteValue => NumberFormat(sbyteValue),
                short shortValue => NumberFormat(shortValue),
                ushort ushortValue => NumberFormat(ushortValue),
                int intValue => NumberFormat(intValue),
                uint uintValue => NumberFormat(uintValue),
                long longValue => NumberFormat(longValue),
                ulong ulongValue => NumberFormat(ulongValue),
                double doubleValue => DoubleFormat(doubleValue),
                float floatValue => FloatFormat(floatValue),
                decimal decimalValue => DecimalFormat(decimalValue),
                DateTime dateValue => DateTimeFormat(dateValue),
                _ => throw new Exception($"The type '{value.GetType()}' of value in GenerateFormattedValueForQuery is not valid."),
            };
        }

        public static string BoolFormat(string value)
        {
            return BoolFormat(string.IsNullOrEmpty(value) ? false : bool.Parse(value));
        }

        public static string BoolFormat(bool value)
        {
            return NumberFormat(value ? 1 : 0);
        }

        public static string IntFormat(string value)
        {
            return NumberFormat(string.IsNullOrEmpty(value) ? 0 : int.Parse(value));
        }

        public static string NumberFormat(byte value)
        {
            return value.ToString();
        }

        public static string NumberFormat(sbyte value)
        {
            return value.ToString();
        }

        public static string NumberFormat(short value)
        {
            return value.ToString();
        }

        public static string NumberFormat(ushort value)
        {
            return value.ToString();
        }

        public static string NumberFormat(int value)
        {
            return value.ToString();
        }

        public static string NumberFormat(uint value)
        {
            return value.ToString();
        }

        public static string NumberFormat(long value)
        {
            return value.ToString();
        }

        public static string NumberFormat(ulong value)
        {
            return value.ToString();
        }

        public static string DoubleFormat(string value)
        {
            return DoubleFormat(string.IsNullOrEmpty(value) ? 0 : double.Parse(value));
        }

        public static string DoubleFormat(double value)
        {
            return value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string FloatFormat(float value)
        {
            return value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string DecimalFormat(decimal value)
        {
            return value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string DateFormat(string value)
        {
            return DateFormat(DateTime.Parse(value));
        }

        public static string DateFormat(DateTime value)
        {
            return "'" + value.ToString("yyyy-MM-dd") + "'";
        }
        
        public static string TimeFormat(string value)
        {
            return TimeFormat(DateTime.Parse(value));
        }

        public static string TimeFormat(DateTime value)
        {
            return "'" + value.ToString("HH:mm:ss") + "'";
        }

        public static string DateTimeFormat(string value)
        {
            return DateTimeFormat(DateTime.Parse(value));
        }

        public static string DateTimeFormat(DateTime value)
        {
            return "'" + value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }

        public static string StringFormat(string value)
        {
            if (value == null)
                return "NULL";

            var output = value;
            output = output.Replace("'", "''");
            output = output.Replace("\\", "\\\\");
            return output;
        }
    }
}
