using System;

namespace Atata
{
    [ControlDefinition("input[@type='text' or @type='date' or not(@type)]")]
    public class DateInput<TOwner> : Input<DateTime?, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string DefaultFormat = "d";

        protected override void InitValueTermOptions(TermOptions termOptions, UIComponentMetadata metadata)
        {
            base.InitValueTermOptions(termOptions, metadata);

            if (termOptions.Format == null)
                termOptions.Format = DefaultFormat;
        }
    }
}
