namespace Atata
{
    public interface IItemsControl
    {
        ////IElementFindStrategy ItemsFindStrategy { get; set; }
        ComponentScopeLocateOptions ItemsFindOptions { get; set; }
        IItemElementFindStrategy ItemFindStrategy { get; set; }
    }
}
