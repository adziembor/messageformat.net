using Jeffijoe.MessageFormat.Formatting.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeffijoe.MessageFormat.Pluralizers
{
    internal static class DictionaryExtensions {
        public static void AddRange<T>(this IDictionary<string, T> dict, string[] range, T value)
        {
            foreach(var k in range)
                dict.Add(k, value);
        }
    }
    public class DefaultPluralizerCollection : IPluralizerCollection
    {
        private static Dictionary<string, Pluralizer> _collection;

        static DefaultPluralizerCollection()
        {
            _collection = new Dictionary<string, Pluralizer>(StringComparer.InvariantCultureIgnoreCase);
            // english doesn't have "0" pluralization, but I don't want to fix tests
            _collection.AddRange("en".Split(' '),
                i => i == 0 ? "zero" : (i == 1 ? "one" : "other"));
            // todo: cldr parser
            // http://cldr.unicode.org/index/downloads
            // /cldr-common-*/common/supplemental/plurals.xml
            _collection.AddRange("ast ca de et fi fy gl it ji nl sv sw ur yi".Split(' '),
                i => i == 1 ? "one" : "other");
            _collection.Add("pl",
                i =>
                    (i == 1) ? "one" : (
                    (i % 1 == 0 && i % 10 >= 2 && i % 10 <= 4 && (i % 100 < 12 || i % 100 > 14)) ? "few" : (
                    (i % 1 == 0) ? "many" : "other")));
        }
        
        public bool TryAddPluralizer(string name, Pluralizer pluralizer)
        {
            return false;
        }

        public bool TryGetPluralizer(string name, out Pluralizer pluralizer)
        {
            return _collection.TryGetValue(name, out pluralizer);
        }
    }
}
