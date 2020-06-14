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
                case int intValue:
                    return IntFormat(intValue);
                case double doubleValue:
                    return DoubleFormat(doubleValue);
                case float floatValue:
                    return FloatFormat(floatValue);
                case DateTime dateValue:
                    return DateTimeFormat(dateValue);
                default:
                    throw new Exception($"The type '{value.GetType()}' of value in GenerateFormattedValueForQuery is not valid.");
            }
        }

        private static string BoolFormat(bool value)
        {
            return IntFormat(value ? 1 : 0);
        }

        private static string IntFormat(int value)
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

        private static string DateFormat(DateTime value)
        {
            return "'" + value.ToString("yyyy-MM-dd") + "'";
        }

        private static string TimeFormat(DateTime value)
        {
            return "'" + value.ToString("HH:mm:ss") + "'";
        }

        private static string DateTimeFormat(DateTime value)
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
