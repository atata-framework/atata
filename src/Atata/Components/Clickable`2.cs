namespace Atata
{
    [UIComponent("*", IgnoreNameEndings = "Button,Link")]
    public class Clickable<TNavigateTo, TOwner> : ClickableBase<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        ////private bool setAllGeneratables;

        public new TNavigateTo Click()
        {
            base.Click();

            TNavigateTo newObject = Owner.GoTo<TNavigateTo>();
            ////if (setAllGeneratables)
            ////    newObject.SetAllGeneratables();
            return newObject;
        }

        ////protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        ////{
        ////    base.ApplyMetadata(metadata);

        ////    setAllGeneratables = metadata.GetFirstOrDefaultDeclaringAttribute<SetAllGeneratablesAttribute>() != null;
        ////}
    }
}
