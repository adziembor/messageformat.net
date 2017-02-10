using Jeffijoe.MessageFormat.Pluralizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using Xunit;
using Jeffijoe.MessageFormat.Formatting.Formatters;

namespace Jeffijoe.MessageFormat.Tests
{

    public class PluralizerTests
    {
        [Fact]
        public void DefaultsAreInvariant()
        {
            var collection = new DefaultPluralizerCollection();
            Pluralizer oldPluralizer;
            Assert.True(collection.TryGetPluralizer("en", out oldPluralizer));
            var added = new Mock<Pluralizer>();
            Assert.False(collection.TryAddPluralizer("en", added.Object));
            Pluralizer newPluralizer;
            Assert.True(collection.TryGetPluralizer("en", out newPluralizer));
            Assert.Equal(oldPluralizer, newPluralizer);
        }

        [Fact]
        public void DictionaryPluralizersAllowAddingOnce()
        {
            var collection = new DictionaryPluralizerCollection();
            var first = new Mock<Pluralizer>();
            var second = new Mock<Pluralizer>();
            Assert.True(collection.TryAddPluralizer("en", first.Object));
            Assert.False(collection.TryAddPluralizer("en", second.Object));
            Pluralizer get;
            Assert.True(collection.TryGetPluralizer("en", out get));
            Assert.Equal(first.Object, get);
        }

        class PluralizerLookupTester : IPluralizerCollection
        {
            public bool TryAddPluralizer(string name, Pluralizer pluralizer)
            {
                throw new NotImplementedException();
            }

            public bool IU_CA = true;

            public bool TryGetPluralizer(string name, out Pluralizer pluralizer)
            {
                pluralizer = null;
                if (IU_CA && name.Equals("iu_ca", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                if (name.Equals("iu", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                return false;
            }
        }

        [Fact]
        public void FindingPluralizersIsSmart()
        {
            var tester = new PluralizerLookupTester();
            var collection = new FindingPluralizerCollection(tester);
            Pluralizer p;
            Assert.True(collection.TryGetPluralizer("iu-Latn-CA", out p));
            Assert.True(collection.TryGetPluralizer("iu-CA", out p));
            Assert.True(collection.TryGetPluralizer("iu_CA", out p));
            tester.IU_CA = false;
            Assert.True(collection.TryGetPluralizer("iu-CA", out p));
            Assert.True(collection.TryGetPluralizer("iu_CA", out p));
            Assert.True(collection.TryGetPluralizer("IU", out p));
            Assert.False(collection.TryGetPluralizer("en", out p));
        }

        [Fact]
        public void FindingPluralizersAddWorks()
        {
            var collection = new FindingPluralizerCollection(new DictionaryPluralizerCollection());
            Pluralizer pluralizer;
            var first = new Mock<Pluralizer>().Object;
            Assert.True(collection.TryAddPluralizer("pl_PL", first));
            Assert.True(collection.TryGetPluralizer("pl-PL", out pluralizer));
            Assert.Equal(pluralizer, first);
        }

        [Fact]
        public void DictionaryIsCaseInsensitive()
        {
            var collection = new DictionaryPluralizerCollection();
            var first = new Mock<Pluralizer>();
            Assert.True(collection.TryAddPluralizer("en", first.Object));
            Pluralizer get;
            Assert.True(collection.TryGetPluralizer("EN", out get));
            Assert.Equal(first.Object, get);
        }


        [Fact]
        public void OverlayingPluralizerAddsToOverlay()
        {
            var defaults = new DictionaryPluralizerCollection();
            var overlay = new DictionaryPluralizerCollection();

            Pluralizer pluralizer;
            var first = new Mock<Pluralizer>().Object;
            var second = new Mock<Pluralizer>().Object;

            Assert.True(defaults.TryAddPluralizer("first", first));
            var collection = new OverlayingPluralizerCollection(defaults, overlay);
            Assert.True(collection.TryAddPluralizer("second", second));
            Assert.True(overlay.TryGetPluralizer("second", out pluralizer));
            Assert.False(defaults.TryGetPluralizer("second", out pluralizer));
        }

        [Fact]
        public void OverlayingPluralizerWorks()
        {
            var defaults = new DictionaryPluralizerCollection();
            var overlay = new DictionaryPluralizerCollection();

            Pluralizer pluralizer;
            var first = new Mock<Pluralizer>().Object;
            var overlayFirst = new Mock<Pluralizer>().Object;
            var third = new Mock<Pluralizer>().Object;

            Assert.True(defaults.TryAddPluralizer("first", first));
            Assert.True(defaults.TryAddPluralizer("third", third));
            var collection = new OverlayingPluralizerCollection(defaults, overlay);
            Assert.True(collection.TryAddPluralizer("first", overlayFirst));

            Assert.True(collection.TryGetPluralizer("first", out pluralizer));
            Assert.Equal(pluralizer, overlayFirst);
            Assert.NotEqual(pluralizer, first);
            Assert.True(collection.TryGetPluralizer("third", out pluralizer));
            Assert.Equal(pluralizer, third);
        }
    }
}
