using System;
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

            string expectedMessage = "exist";

            AtataContext.Current.Log.Start(new VerificationLogSection(should.VerificationKind, should.Component, $"{should.GetShouldText()} {expectedMessage}"));

            SearchOptions searchOptions = new SearchOptions
            {
                IsSafely = false,
                Timeout = should.Timeout ?? AtataContext.Current.VerificationTimeout,
                RetryInterval = should.RetryInterval ?? AtataContext.Current.VerificationRetryInterval
            };

            try
            {
                StaleSafely.Execute(
                    options =>
                    {
                        if (should.IsNegation)
                            should.Component.Missing(options);
                        else
                            should.Component.Exists(options);
                    },
                    searchOptions);
            }
            catch (Exception exception)
            {
                string failureMessage = new StringBuilder().
                    Append($"{should.Component.ComponentFullName} presence.").
                    AppendLine().
                    Append($"Expected: {should.GetShouldText()} {expectedMessage}").
                    ToString();

                should.ReportFailure(failureMessage, exception);
            }

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        public static TOwner BeVisible<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsVisible.Should.WithSettings(should).BeTrue();
        }

        public static TOwner BeHidden<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsVisible.Should.WithSettings(should).BeFalse();
        }

        public static TOwner BeEnabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsEnabled.Should.WithSettings(should).BeTrue();
        }

        public static TOwner BeDisabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsEnabled.Should.WithSettings(should).BeFalse();
        }

        public static TOwner BeReadOnly<TData, TControl, TOwner>(this IFieldVerificationProvider<TData, TControl, TOwner> should)
            where TControl : EditableField<TData, TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsReadOnly.Should.WithSettings(should).BeTrue();
        }

        public static TOwner BeChecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Should.WithSettings(should).BeTrue();
        }

        public static TOwner BeUnchecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Should.WithSettings(should).BeFalse();
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

            AtataContext.Current.Log.Start(new VerificationLogSection(should.VerificationKind, should.Component, $"{should.GetShouldText()} {expectedMessage}"));

            IEnumerable<TData> actualIndividualValues = null;
            Exception exception = null;

            bool doesSatisfy = AtataContext.Current.Driver.Try().Until(
                _ =>
                {
                    try
                    {
                        actualIndividualValues = should.Component.GetIndividualValues(should.Component.Get());
                        int intersectionsCount = expectedIndividualValues.Intersect(actualIndividualValues).Count();
                        bool result = should.IsNegation ? intersectionsCount == 0 : intersectionsCount == expectedIndividualValues.Count();
                        exception = null;
                        return result;
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        return false;
                    }
                },
                should.GetRetryOptions());

            if (!doesSatisfy)
            {
                string actualMessage = exception == null ? should.Component.ConvertIndividualValuesToString(actualIndividualValues, true) : null;

                string failureMessage = VerificationUtils.BuildFailureMessage(should, expectedMessage, actualMessage);

                should.ReportFailure(failureMessage, exception);
            }

            AtataContext.Current.Log.EndSection();

            return should.Owner;
        }

        public static TOwner HaveClass<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should, params string[] classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.HaveClass(classNames.AsEnumerable());
        }

        public static TOwner HaveClass<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should, IEnumerable<string> classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Attributes.Class.Should.WithSettings(should).Contain(classNames);
        }
    }
}
