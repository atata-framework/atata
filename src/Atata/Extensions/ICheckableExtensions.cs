namespace Atata
{
    public static class ICheckableExtensions
    {
        public static TOwner VerifyIsChecked<TOwner>(this ICheckable<TOwner> checkable)
            where TOwner : PageObject<TOwner>
        {
            ATContext.Current.Log.StartVerificationSection("{0} is checked", checkable.ComponentFullName);

            ATAssert.IsTrue(checkable.Get(), checkable.BuildErrorMessage());

            ATContext.Current.Log.EndSection();

            return checkable.Owner;
        }

        public static TOwner VerifyIsUnchecked<TOwner>(this ICheckable<TOwner> checkable)
            where TOwner : PageObject<TOwner>
        {
            ATContext.Current.Log.StartVerificationSection("{0} is unchecked", checkable.ComponentFullName);

            ATAssert.IsFalse(checkable.Get(), checkable.BuildErrorMessage());

            ATContext.Current.Log.EndSection();

            return checkable.Owner;
        }
    }
}
