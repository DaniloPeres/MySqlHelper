using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MySqlHelper.Extensions
{
    public static class CommonExtensions
    {
        public static (bool success, DateTime dateTime)
            ConvertToDate(this string valor, bool aceitarNull)
        {
            if (aceitarNull && string.IsNullOrEmpty(valor))
                return (true, default(DateTime));

            var culture = CultureInfo.CreateSpecificCulture("en-US");

            if (valor.Length != 10)
                return (false, default(DateTime));

            var success = DateTime.TryParse(valor, culture, DateTimeStyles.None, out var temp);

            return (success, temp);
        }

        public static (bool success, double valor) ConvertToDecimal(this string text)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var success = double.TryParse(text, NumberStyles.Number | NumberStyles.AllowThousands, culture, out var temp);
            return (success, temp);
        }
    }
}
