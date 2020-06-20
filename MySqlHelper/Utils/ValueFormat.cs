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

            switch (value)
            {
                case string stringValue:
                    return $"'{StringFormat(stringValue)}'";
                case bool boolValue:
                    return BoolFormat(boolValue);
                case byte byteValue:
                    return NumberFormat(byteValue);
                case sbyte sbyteValue:
                    return NumberFormat(sbyteValue);
                case short shortValue:
                    return NumberFormat(shortValue);
                case ushort ushortValue:
                    return NumberFormat(ushortValue);
                case int intValue:
                    return NumberFormat(intValue);
                case uint uintValue:
                    return NumberFormat(uintValue);
                case long longValue:
                    return NumberFormat(longValue);
                case ulong ulongValue:
                    return NumberFormat(ulongValue);
                case double doubleValue:
                    return DoubleFormat(doubleValue);
                case float floatValue:
                    return FloatFormat(floatValue);
                case decimal decimalValue:
                    return DecimalFormat(decimalValue);
                case DateTime dateValue:
                    return DateTimeFormat(dateValue);
                default:
                    throw new Exception($"The type '{value.GetType()}' of value in GenerateFormattedValueForQuery is not valid.");
            }
        }

        private static string BoolFormat(bool value)
        {
            return NumberFormat(value ? 1 : 0);
        }

        private static string NumberFormat(byte value)
        {
            return value.ToString();
        }


        private static string NumberFormat(sbyte value)
        {
            return value.ToString();
        }

        private static string NumberFormat(short value)
        {
            return value.ToString();
        }

        private static string NumberFormat(ushort value)
        {
            return value.ToString();
        }

        private static string NumberFormat(int value)
        {
            return value.ToString();
        }

        private static string NumberFormat(uint value)
        {
            return value.ToString();
        }

        private static string NumberFormat(long value)
        {
            return value.ToString();
        }

        private static string NumberFormat(ulong value)
        {
            return value.ToString();
        }

        private static string DoubleFormat(double value)
        {
            return value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        private static string FloatFormat(float value)
        {
            return value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string DecimalFormat(decimal value)
        {
            return value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public static string DateFormat(DateTime value)
        {
            return "'" + value.ToString("yyyy-MM-dd") + "'";
        }

        public static string TimeFormat(DateTime value)
        {
            return "'" + value.ToString("HH:mm:ss") + "'";
        }

        public static string DateTimeFormat(DateTime value)
        {
            return "'" + value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }

        private static string StringFormat(string value)
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
