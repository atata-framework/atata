using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Provides a set of verification extension methods for <see cref="UIComponent{TOwner}"/> and its inheritors.
    /// </summary>
    // TODO: Atata v2. Rename all "should" parameters to "verifier".
    public static class IUIComponentVerificationProviderExtensions
    {
        private const string PresenceVerificationStateName = "presence";

        private const string VisibilityVerificationStateName = "visibility";

        private static TOwner VerifyExistence<TComponent, TOwner>(
            IUIComponentVerificationProvider<TComponent, TOwner> should,
            string expectedMessage,
            string verificationStateName,
            Visibility? visibility = null)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            AtataContext.Current.Log.ExecuteSection(
                new VerificationLogSection(should.VerificationKind, should.Component, $"{should.GetShouldText()} {expectedMessage}"),
                () =>
                {
                    SearchOptions searchOptions = new SearchOptions
                    {
                        IsSafely = false,
                        Timeout = should.Timeout ?? AtataContext.Current.VerificationTimeout,
                        RetryInterval = should.RetryInterval ?? AtataContext.Current.VerificationRetryInterval
                    };

                    if (visibility.HasValue)
                        searchOptions.Visibility = visibility.Value;

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
                        var failureMessageBuilder = new StringBuilder().
                            Append($"{should.Component.ComponentFullName} {verificationStateName}").
                            AppendLine().
                            Append($"Expected: {should.GetShouldText()} {expectedMessage}");

                        if (exception is NoSuchElementException || exception is NotMissingElementException)
                        {
                            failureMessageBuilder.AppendLine().Append($"  Actual: {exception.Message.ToLowerFirstLetter()}");
                            should.ReportFailure(failureMessageBuilder.ToString());
                        }
                        else
                        {
                            should.ReportFailure(failureMessageBuilder.ToString(), exception);
                        }
                    }
                });

            return should.Owner;
        }

        /// <summary>
        /// Verifies that the component exist.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        // TODO: Atata v2. Make obsolete. Use BePresent instead.
        public static TOwner Exist<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(should, "exist", PresenceVerificationStateName);

        /// <summary>
        /// Verifies that the component is present.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BePresent<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(should, "be present", PresenceVerificationStateName);

        /// <summary>
        /// Verifies that the component is visible.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeVisible<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(should, "be visible", VisibilityVerificationStateName, Visibility.Visible);

        /// <summary>
        /// Verifies that the component is visible in view port (visible browser screen area).
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeVisibleInViewPort<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsVisibleInViewPort.Should.WithSettings(should).BeTrue();
        }

        /// <summary>
        /// Verifies that the component is hidden.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeHidden<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(should, "be hidden", VisibilityVerificationStateName, Visibility.Hidden);

        /// <summary>
        /// Verifies that the control is enabled.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeEnabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsEnabled.Should.WithSettings(should).BeTrue();
        }

        /// <summary>
        /// Verifies that the control is disabled.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeDisabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsEnabled.Should.WithSettings(should).BeFalse();
        }

        /// <summary>
        /// Verifies that the control is read-only.
        /// </summary>
        /// <typeparam name="TData">The type of the control's data.</typeparam>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeReadOnly<TData, TControl, TOwner>(this IFieldVerificationProvider<TData, TControl, TOwner> should)
            where TControl : EditableField<TData, TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.IsReadOnly.Should.WithSettings(should).BeTrue();
        }

        /// <summary>
        /// Verifies that the control is checked.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeChecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Should.WithSettings(should).BeTrue();
        }

        /// <summary>
        /// Verifies that the control is unchecked.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeUnchecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> should)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Should.WithSettings(should).BeFalse();
        }

        /// <summary>
        /// Verifies that the checkbox list has the specified value(s) checked.
        /// </summary>
        /// <typeparam name="TData">The type of the control's data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <param name="value">The expected value or combination of enumeration flag values.</param>
        /// <returns>The owner instance.</returns>
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

            AtataContext.Current.Log.ExecuteSection(
                new VerificationLogSection(should.VerificationKind, should.Component, $"{should.GetShouldText()} {expectedMessage}"),
                () =>
                {
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
                });

            return should.Owner;
        }

        /// <summary>
        /// Verifies that the component has the specified class(es).
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <param name="classNames">The expected class names.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner HaveClass<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should, params string[] classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.HaveClass(classNames.AsEnumerable());
        }

        /// <summary>
        /// Verifies that the component has the specified class(es).
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="should">The verification provider.</param>
        /// <param name="classNames">The expected class names.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner HaveClass<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> should, IEnumerable<string> classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));

            return should.Component.Attributes.Class.Should.WithSettings(should).Contain(classNames);
        }
    }
}
