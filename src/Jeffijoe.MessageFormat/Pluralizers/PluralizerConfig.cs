using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Jeffijoe.MessageFormat.Pluralizers
{
    public static class PluralizerConfig
    {
        public static readonly IPluralizerCollection DefaultPluralizer
            = new DefaultPluralizerCollection();

        public static IPluralizerCollection Create()
            => new FindingPluralizerCollection(
                new OverlayingPluralizerCollection(
                    DefaultPluralizer,
                    new DictionaryPluralizerCollection()));
    }
}