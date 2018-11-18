namespace Atata
{
    /// <summary>
    /// Specifies the term case.
    /// </summary>
    public enum TermCase
    {
        /// <summary>
        /// Doesn't apply the case.
        /// </summary>
        None,

        /// <summary>
        /// Uses title case (e.g. <c>"Some of the Terms"</c>).
        /// </summary>
        Title,

        /// <summary>
        /// Uses title case with all words capitalized (e.g. <c>"Some Of The Terms"</c>).
        /// </summary>
        Capitalized,

        /// <summary>
        /// Uses sentence case (e.g. <c>"Some term"</c>).
        /// </summary>
        Sentence,

        /// <summary>
        /// Uses mid-sentence case where the first word is not capitalized (e.g. <c>"some term"</c>).
        /// </summary>
        MidSentence,

        /// <summary>
        /// Uses lower case (e.g. <c>"some term"</c>).
        /// </summary>
        Lower,

        /// <summary>
        /// Uses lower case with words merging (e.g. <c>"someterm"</c>).
        /// </summary>
        LowerMerged,

        /// <summary>
        /// Uses upper case (e.g. <c>"SOME TERM"</c>).
        /// </summary>
        Upper,

        /// <summary>
        /// Uses upper case with words merging (e.g. <c>"SOMETERM"</c>).
        /// </summary>
        UpperMerged,

        /// <summary>
        /// Uses camel case (e.g. <c>"someTerm"</c>).
        /// </summary>
        Camel,

        /// <summary>
        /// Uses pascal case (e.g. <c>"SomeTerm"</c>).
        /// </summary>
        Pascal,

        /// <summary>
        /// Uses dash ('-') and lower case (e.g. <c>"some-term"</c>).
        /// </summary>
        Kebab,

        /// <summary>
        /// Uses hyphen ('‐') and lower case (e.g. <c>"some‐term"</c>).
        /// </summary>
        HyphenKebab,

        /// <summary>
        /// Uses dash ('-') and pascal case (e.g. <c>"Some-Term"</c>).
        /// </summary>
        PascalKebab,

        /// <summary>
        /// Uses hyphen ('‐') and pascal case (e.g. <c>"Some‐Term"</c>).
        /// </summary>
        PascalHyphenKebab,

        /// <summary>
        /// Uses underscore ('_') and lower case (e.g. <c>"some_term"</c>).
        /// </summary>
        Snake
    }
}
