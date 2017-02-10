using System;
using System.Collections.Generic;
using Jeffijoe.MessageFormat.Formatting.Formatters;

namespace Jeffijoe.MessageFormat.Pluralizers
{
    public class DictionaryPluralizerCollection : IPluralizerCollection
    {
        private readonly Dictionary<string, Pluralizer> _collection =
            new Dictionary<string, Pluralizer>(StringComparer.InvariantCultureIgnoreCase);

        public bool TryAddPluralizer(string name, Pluralizer pluralizer)
        {
            if (!_collection.ContainsKey(name))
            {
                _collection.Add(name, pluralizer);
                return true;
            }
            return false;
        }

        public bool TryGetPluralizer(string name, out Pluralizer pluralizer)
        {
            return _collection.TryGetValue(name, out pluralizer);
        }
    }
}
