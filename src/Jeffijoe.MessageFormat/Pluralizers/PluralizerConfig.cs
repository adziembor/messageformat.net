using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Jeffijoe.MessageFormat.Pluralizers
{
    public static class PluralizerConfig
    {
        public static readonly IPluralizerCollection Default = 
            new FindingPluralizerCollection(
                new OverlayingPluralizerCollection(
                    new DefaultPluralizerCollection(),
                    new DictionaryPluralizerCollection()));
    }
}