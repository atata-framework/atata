using System;

namespace Atata
{
    [UIComponent("input[@type='text' or @type='date' or not(@type)]")]
    public class DateInput<TOwner> : Input<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string DefaultStringFormat = "d";

        protected override void InitValueTermOptions(TermOptions termOptions, UIComponentMetadata metadata)
        {
            base.InitValueTermOptions(termOptions, metadata);

            if (termOptions.StringFormat == null)
                termOptions.StringFormat = DefaultStringFormat;
        }
    }
}
