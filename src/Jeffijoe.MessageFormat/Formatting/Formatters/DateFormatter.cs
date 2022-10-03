using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeffijoe.MessageFormat.Formatting.Formatters
{
    public class DateFormatter : IFormatter
    {
        public bool VariableMustExist => true;

        public bool CanFormat(FormatterRequest request)
        {
            return request.FormatterName == "date";
        }

        public static DateTime CastDate(object? value)
        {
            return value switch
            {
                long ms => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ms),
                ulong ms2 => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ms2),
                int s => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(s),
                DateTime dt => dt,
                null => throw new ArgumentException("expected date value was null"),
                _ => throw new ArgumentException("Value does not have valid type, unexpected " + value.GetType().Name)
            };
        }

        public string Format(
            string locale,
            FormatterRequest request,
            IDictionary<string, object?> args,
            object? value,
            IMessageFormatter messageFormatter)
        {
            var d = CastDate(value);
            var c = new CultureInfo(locale).DateTimeFormat;
            if (request.FormatterArguments == "short")
            {
                return d.ToString("d", c);
            }

            if (request.FormatterArguments == "long")
            {
                return d.ToString("D", c);
            }

            if (request.FormatterArguments == "full")
            {
                return d.ToString("D", c);
            }

            if (request.FormatterArguments == "" || request.FormatterArguments == "default")
            {
                return d.ToString("D", c);
            }

            throw new FormatException("Invalid date format: " + request.FormatterArguments);
        }
    }
}
