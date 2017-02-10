using Jeffijoe.MessageFormat.Formatting.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeffijoe.MessageFormat.Pluralizers
{
    public class OverlayingPluralizerCollection : IPluralizerCollection
    {
        private readonly IPluralizerCollection _defaults;
        private readonly IPluralizerCollection _overlay;
        public OverlayingPluralizerCollection(IPluralizerCollection defaults, IPluralizerCollection overlay)
        {
            _overlay = overlay;
            _defaults = defaults;
        }

        public bool TryAddPluralizer(string name, Pluralizer pluralizer)
        {
            return _overlay.TryAddPluralizer(name, pluralizer);
        }

        public bool TryGetPluralizer(string name, out Pluralizer pluralizer)
        {
            if(_overlay.TryGetPluralizer(name, out pluralizer))
            {
                return true;
            }
            return _defaults.TryGetPluralizer(name, out pluralizer);
        }
    }
}
