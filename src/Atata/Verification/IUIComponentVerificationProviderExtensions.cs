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
    public static class IUIComponentVerificationProviderExtensions
    {
        private const string PresenceVerificationStateName = "presence";

        private const string VisibilityVerificationStateName = "visibility";

        private static TOwner VerifyExistence<TComponent, TOwner>(
            IUIComponentVerificationProvider<TComponent, TOwner> verifier,
            string expectedMessage,
            string verificationStateName,
            Visibility? visibility = null)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            verifier.Component.Context.Log.ExecuteSection(
                new VerificationLogSection(verifier.Strategy.VerificationKind, verifier.Component.ComponentFullName, $"{verifier.GetShouldText()} {expectedMessage}"),
                () =>
                {
                    SearchOptions searchOptions = new SearchOptions
                    {
                        IsSafely = false,
                        Timeout = verifier.Timeout ?? verifier.Component.Context.VerificationTimeout,
                        RetryInterval = verifier.RetryInterval ?? verifier.Component.Context.VerificationRetryInterval
                    };

                    if (visibility.HasValue)
                        searchOptions.Visibility = visibility.Value;

                    try
                    {
                        StaleSafely.Execute(
                            options =>
                            {
                                if (verifier.IsNegation)
                                    verifier.Component.Missing(options);
                                else
                                    verifier.Component.Exists(options);
                            },
                            searchOptions);
                    }
                    catch (Exception exception)
                    {
                        var failureMessageBuilder = new StringBuilder().
                            Append($"{verifier.Component.ComponentFullName} {verificationStateName}").
                            AppendLine().
                            Append($"Expected: {verifier.GetShouldText()} {expectedMessage}");

                        if (exception is NoSuchElementException || exception is NotMissingElementException)
                        {
                            failureMessageBuilder.AppendLine().Append($"  Actual: {exception.Message.ToLowerFirstLetter()}");
                            verifier.ReportFailure(failureMessageBuilder.ToString());
                        }
                        else
                        {
                            verifier.ReportFailure(failureMessageBuilder.ToString(), exception);
                        }
                    }
                });

            return verifier.Owner;
        }

        /// <summary>
        /// Verifies that the component exist.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        // TODO: Atata v2. Make obsolete. Use BePresent instead.
        public static TOwner Exist<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(verifier, "exist", PresenceVerificationStateName);

        /// <summary>
        /// Verifies that the component is present.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BePresent<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(verifier, "be present", PresenceVerificationStateName);

        /// <summary>
        /// Verifies that the component is visible.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeVisible<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(verifier, "be visible", VisibilityVerificationStateName, Visibility.Visible);

        /// <summary>
        /// Verifies that the component is visible in view port (visible browser screen area).
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeVisibleInViewPort<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.IsVisibleInViewPort.Should.WithSettings(verifier).BeTrue();
        }

        /// <summary>
        /// Verifies that the component is hidden.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeHidden<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
            =>
            VerifyExistence(verifier, "be hidden", VisibilityVerificationStateName, Visibility.Hidden);

        /// <summary>
        /// Verifies that the control is enabled.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeEnabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> verifier)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.IsEnabled.Should.WithSettings(verifier).BeTrue();
        }

        /// <summary>
        /// Verifies that the control is disabled.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeDisabled<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> verifier)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.IsEnabled.Should.WithSettings(verifier).BeFalse();
        }

        /// <summary>
        /// Verifies that the control is read-only.
        /// </summary>
        /// <typeparam name="TData">The type of the control's data.</typeparam>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeReadOnly<TData, TControl, TOwner>(this IFieldVerificationProvider<TData, TControl, TOwner> verifier)
            where TControl : EditableField<TData, TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.IsReadOnly.Should.WithSettings(verifier).BeTrue();
        }

        /// <summary>
        /// Verifies that the control is checked.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeChecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> verifier)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.Should.WithSettings(verifier).BeTrue();
        }

        /// <summary>
        /// Verifies that the control is unchecked.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner BeUnchecked<TControl, TOwner>(this IUIComponentVerificationProvider<TControl, TOwner> verifier)
            where TControl : Field<bool, TOwner>, ICheckable<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.Should.WithSettings(verifier).BeFalse();
        }

        /// <summary>
        /// Verifies that the checkbox list has the specified value(s) checked.
        /// </summary>
        /// <typeparam name="TData">The type of the control's data.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="value">The expected value or combination of enumeration flag values.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner HaveChecked<TData, TOwner>(this IFieldVerificationProvider<TData, CheckBoxList<TData, TOwner>, TOwner> verifier, TData value)
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            IEnumerable<TData> expectedIndividualValues = verifier.Component.GetIndividualValues(value);
            string expectedIndividualValuesAsString = verifier.Component.ConvertIndividualValuesToString(expectedIndividualValues, true);

            string expectedMessage = new StringBuilder().
                Append("have checked").
                AppendIf(expectedIndividualValues.Count() > 1, ":").
                Append($" {expectedIndividualValuesAsString}").ToString();

            verifier.Component.Context.Log.ExecuteSection(
                new VerificationLogSection(verifier.Strategy.VerificationKind, verifier.Component.ComponentFullName, $"{verifier.GetShouldText()} {expectedMessage}"),
                () =>
                {
                    IEnumerable<TData> actualIndividualValues = null;
                    Exception exception = null;

                    bool doesSatisfy = verifier.Component.Context.Driver.Try().Until(
                        _ =>
                        {
                            try
                            {
                                actualIndividualValues = verifier.Component.GetIndividualValues(verifier.Component.Get());
                                int intersectionsCount = expectedIndividualValues.Intersect(actualIndividualValues).Count();
                                bool result = verifier.IsNegation ? intersectionsCount == 0 : intersectionsCount == expectedIndividualValues.Count();
                                exception = null;
                                return result;
                            }
                            catch (Exception e)
                            {
                                exception = e;
                                return false;
                            }
                        },
                        verifier.GetRetryOptions());

                    if (!doesSatisfy)
                    {
                        string actualMessage = exception == null ? verifier.Component.ConvertIndividualValuesToString(actualIndividualValues, true) : null;

                        string failureMessage = VerificationUtils.BuildFailureMessage(verifier, expectedMessage, actualMessage);

                        verifier.ReportFailure(failureMessage, exception);
                    }
                });

            return verifier.Owner;
        }

        /// <summary>
        /// Verifies that the component has the specified class(es).
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="classNames">The expected class names.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner HaveClass<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier, params string[] classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return verifier.HaveClass(classNames.AsEnumerable());
        }

        /// <summary>
        /// Verifies that the component has the specified class(es).
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="verifier">The verification provider.</param>
        /// <param name="classNames">The expected class names.</param>
        /// <returns>The owner instance.</returns>
        public static TOwner HaveClass<TComponent, TOwner>(this IUIComponentVerificationProvider<TComponent, TOwner> verifier, IEnumerable<string> classNames)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            verifier.CheckNotNull(nameof(verifier));

            return verifier.Component.Attributes.Class.Should.WithSettings(verifier).Contain(classNames);
        }
    }
}
