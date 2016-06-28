namespace Atata
{
    /// <summary>
    /// Specifies the term case.
    /// </summary>
    public enum TermCase
    {
        /// <summary>
        /// Uses inherited case or none if missing.
        /// </summary>
        Inherit,

        /// <summary>
        /// Doesn't apply case.
        /// </summary>
        None,

        /// <summary>
        /// Uses title case (e.g. "Some Term").
        /// </summary>
        Title,

        /// <summary>
        /// Uses title case with colon (':') ending (e.g. "Some Term:").
        /// </summary>
        TitleWithColon,

        /// <summary>
        /// Uses sentence case (e.g. "Some term").
        /// </summary>
        Sentence,

        /// <summary>
        /// Uses sentence case starting with first letter in lower case (e.g. "some term").
        /// </summary>
        SentenceLower,

        /// <summary>
        /// Uses sententce case with colon (':') ending (e.g. "Some term:").
        /// </summary>
        SentenceWithColon,

        /// <summary>
        /// Uses lower case (e.g. "some term").
        /// </summary>
        Lower,

        /// <summary>
        /// Uses lower case with words merging (e.g. "someterm").
        /// </summary>
        LowerMerged,

        /// <summary>
        /// Uses upper case (e.g. "SOME TERM").
        /// </summary>
        Upper,

        /// <summary>
        /// Uses upper case with words merging (e.g. "SOMETERM").
        /// </summary>
        UpperMerged,

        /// <summary>
        /// Uses camel case (e.g. "someTerm").
        /// </summary>
        Camel,

        /// <summary>
        /// Uses pascal case (e.g. "SomeTerm").
        /// </summary>
        Pascal,

        /// <summary>
        /// Uses dash ('-') and lower case (e.g. "some-term").
        /// </summary>
        Kebab,

        /// <summary>
        /// Uses hyphen ('‐') and lower case (e.g. "some‐term").
        /// </summary>
        HyphenKebab,

        /// <summary>
        /// Uses dash ('-') and pascal case (e.g. "Some-Term").
        /// </summary>
        PascalKebab,

        /// <summary>
        /// Uses hyphen ('‐') and pascal case (e.g. "Some‐Term").
        /// </summary>
        PascalHyphenKebab,

        /// <summary>
        /// Uses dash ('-'), lower case and "x-" prefix (e.g. "x-some-term").
        /// </summary>
        XKebab,

        /// <summary>
        /// Uses undersore ('_') and lower case (e.g. "some_term").
        /// </summary>
        Snake
    }
}
