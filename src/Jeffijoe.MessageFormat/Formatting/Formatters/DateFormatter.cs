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
        public bool CanFormat(FormatterRequest request)
        {
            return request.FormatterName == "date";
        }

        public DateTime CastDate(object value)
        {
            if(value is long)
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)value);
            if (value is ulong)
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((ulong)value);
            if (value is int)
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((int)value);
            if (value is DateTime)
                return (DateTime)value;
            throw new ArgumentException("Value does not have valid type, unexpected " + value.GetType().Name);
        }
        public string Format(
            string locale,
            FormatterRequest request,
            IDictionary<string, object> args,
            object value,
            IMessageFormatter messageFormatter)
        {
            var d = CastDate(value);
            var c = new CultureInfo(locale).DateTimeFormat;
            if(request.FormatterArguments == "short")
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

            throw new FormatException("Invalid date format: "+request.FormatterArguments);
        }
    }
}
