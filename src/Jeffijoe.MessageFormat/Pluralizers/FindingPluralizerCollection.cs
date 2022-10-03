using System;
using System.Linq;
using System.Collections.Generic;
using Jeffijoe.MessageFormat.Formatting.Formatters;
using System.Diagnostics.CodeAnalysis;

namespace Jeffijoe.MessageFormat.Pluralizers
{

    /// <summary>
    /// A pluralizer collection that tries to find locale by given tag
    /// </summary>
    public class FindingPluralizerCollection : IPluralizerCollection
    {
        IPluralizerCollection _pluralizers;
        public FindingPluralizerCollection(IPluralizerCollection pluralizers)
        {
            _pluralizers = pluralizers;
        }
        public bool TryAddPluralizer(string name, Pluralizer pluralizer)
        {
            return _pluralizers.TryAddPluralizer(name.Replace("-", "_"), pluralizer);
        }
        private IEnumerable<string> GetMatchingLocaleTags(string name)
        {
            var n = name.Split('_');
            if (n.Length == 1) // iu
                yield return n[0];
            if (n.Length == 2) // iu_CA
            {
                yield return name;
                yield return n[0];
            }
            if (n.Length == 3) // iu_Latn_CA
            {
                yield return n[0] + "_" + n[2]; // iu_CA
                yield return n[0]; // iu
                yield return name; // iu_Latn_CA
            }
        }
        public bool TryGetPluralizer(string name, [NotNullWhen(true), MaybeNullWhen(false)] out Pluralizer pluralizer)
        {
            foreach (var tagName in GetMatchingLocaleTags(name.Replace("-", "_")))
            {
                if (_pluralizers.TryGetPluralizer(tagName, out pluralizer))
                    return true;
            }
            pluralizer = null;
            return false;
        }
    }

}