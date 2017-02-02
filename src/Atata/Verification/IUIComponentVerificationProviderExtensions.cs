using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atata
{
    public static class IUIComponentVerificationProviderExtensions
    {
        public static TOwner Exist<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            AtataContext.Current.Log.Start(new VerificationLogSection(should.Component, $"{should.GetShouldText()} exist"));

            SearchOptions searchOptions = new SearchOptions
            {
                IsSafely = false,
                Timeout = should.Timeout ?? AtataContext.Current.RetryTimeout,
                RetryInterval = should.RetryInterval ?? AtataContext.Current.RetryInterval
            };

            if (should.IsNegation)
                should.Component.Missing(searchOptions);
            else
                should.Component.Exists(searchOptions);

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        public static TOwner BeVisible<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.IsVisible.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeTrue() : dataShould.BeTrue();
        }

        public static TOwner BeHidden<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.IsVisible.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeFalse() : dataShould.BeFalse();
        }

        public static TOwner BeEnabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.IsEnabled.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeTrue() : dataShould.BeTrue();
        }

        public static TOwner BeDisabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.IsEnabled.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeFalse() : dataShould.BeFalse();
        }

        public static TOwner BeReadOnly<TData, TControl, TOwner>(this IFieldVerificationProvider<TData, TControl, TOwner> should)
            where TControl : EditableField<TData, TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.IsReadOnly.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeTrue() : dataShould.BeTrue();
        }

        public static TOwner BeChecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeTrue() : dataShould.BeTrue();
        }

        public static TOwner BeUnchecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            var dataShould = should.Component.Should.ApplySettings(should);
            return should.IsNegation ? dataShould.Not.BeFalse() : dataShould.BeFalse();
        }

        public static TOwner HaveChecked<TData, TOwner>(this IFieldVerificationProvider<TData, CheckBoxList<TData, TOwner>, TOwner> should, TData value)
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            IEnumerable<TData> expectedIndividualValues = should.Component.GetIndividualValues(value);
            string expectedIndividualValuesAsString = should.Component.ConvertIndividualValuesToString(expectedIndividualValues, true);

            string expectedMessage = new StringBuilder().
                Append("have checked").
                AppendIf(expectedIndividualValues.Count() > 1, ":").
                Append($" {expectedIndividualValuesAsString}").ToString();

            AtataContext.Current.Log.Start(new VerificationLogSection(should.Component, $"{should.GetShouldText()} {expectedMessage}"));

            IEnumerable<TData> actualIndividualValues = null;

            bool doesSatisfy = AtataContext.Current.Driver.Try().Until(
                _ =>
                {
                    actualIndividualValues = should.Component.GetIndividualValues(should.Component.Get());
                    int intersectionsCount = expectedIndividualValues.Intersect(actualIndividualValues).Count();
                    return should.IsNegation ? intersectionsCount == 0 : intersectionsCount == expectedIndividualValues.Count();
                },
                should.Timeout,
                should.RetryInterval);

            if (!doesSatisfy)
            {
                throw should.CreateAssertionException(
                    expectedMessage,
                    should.Component.ConvertIndividualValuesToString(actualIndividualValues, true));
            }

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }
    }
}
