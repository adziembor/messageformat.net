using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Jeffijoe.MessageFormat.Formatting.Formatters;

namespace Jeffijoe.MessageFormat.Pluralizers
{
    public class DictionaryPluralizerCollection : IPluralizerCollection,
        IEnumerable<KeyValuePair<string, Pluralizer>>
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

        public void Add(string name, Pluralizer pluralizer)
        {
            _collection.Add(name, pluralizer);
        }

        public bool TryGetPluralizer(string name, [NotNullWhen(true), MaybeNullWhen(false)] out Pluralizer pluralizer)
        {
            return _collection.TryGetValue(name, out pluralizer);
        }

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

        public IEnumerator<KeyValuePair<string, Pluralizer>> GetEnumerator()
            => _collection.GetEnumerator();
    }
}
