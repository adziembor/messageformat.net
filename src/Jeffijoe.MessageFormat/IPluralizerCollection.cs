using Jeffijoe.MessageFormat.Formatting.Formatters;
using Jeffijoe.MessageFormat.Pluralizers;
using System.Diagnostics.CodeAnalysis;

namespace Jeffijoe.MessageFormat
{
    public interface IPluralizerCollection
    {
        /// <summary>
        /// Adds a pluralizer to the collection.
        /// Not thread safe.
        /// </summary>
        /// <param name="name">Locale name</param>
        /// <param name="pluralizer">Pluralizer delegate to add</param>
        /// <returns>true if the pluralizer was added successfully; otherwise, false</returns>
        bool TryAddPluralizer(string name, Pluralizer pluralizer);
        /// <summary>
        /// Gets a pluralizer from the collection
        /// </summary>
        /// <param name="name">locale name</param>
        /// <param name="pluralizer">resolved pluralizer or null in case of failure</param>
        /// <returns>true if the pluralizer was found; otherwise, false</returns>
        bool TryGetPluralizer(string name, [NotNullWhen(true), MaybeNullWhen(false)] out Pluralizer pluralizer);
    }
}
