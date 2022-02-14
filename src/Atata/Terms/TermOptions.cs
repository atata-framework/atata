using System.Globalization;

namespace Atata
{
    public class TermOptions : ITermSettings, IHasOptionalProperties
    {
        public PropertyBag OptionalProperties { get; } = new PropertyBag();

        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        public TermMatch Match
        {
            get => OptionalProperties.GetOrDefault(nameof(Match), TermMatch.Equals);
            set => OptionalProperties[nameof(Match)] = value;
        }

        /// <summary>
        /// Gets or sets the term case.
        /// </summary>
        public TermCase Case
        {
            get => OptionalProperties.GetOrDefault(nameof(Case), TermCase.None);
            set => OptionalProperties[nameof(Case)] = value;
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get => OptionalProperties.GetOrDefault<string>(nameof(Format));
            set => OptionalProperties[nameof(Format)] = value;
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get => OptionalProperties.GetOrDefault(nameof(Culture), CultureInfo.CurrentCulture);
            set => OptionalProperties[nameof(Culture)] = value;
        }

        public TermOptions MergeWith(IHasOptionalProperties settingsAttribute)
        {
            settingsAttribute.CheckNotNull(nameof(settingsAttribute));

            if (settingsAttribute.OptionalProperties.Contains(nameof(Case)))
                Case = (TermCase)settingsAttribute.OptionalProperties[nameof(Case)];

            if (settingsAttribute.OptionalProperties.Contains(nameof(Match)))
                Match = (TermMatch)settingsAttribute.OptionalProperties[nameof(Match)];

            if (settingsAttribute.OptionalProperties.Contains(nameof(Format)))
                Format = (string)settingsAttribute.OptionalProperties[nameof(Format)];

            if (settingsAttribute.OptionalProperties.Contains(nameof(Culture)))
                Culture = (CultureInfo)settingsAttribute.OptionalProperties[nameof(Culture)];

            return this;
        }

        public TermOptions WithFormat(string format)
        {
            Format = format;
            return this;
        }
    }
}
