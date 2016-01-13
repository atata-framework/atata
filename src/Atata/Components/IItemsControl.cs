namespace Atata
{
    public interface IItemsControl
    {
        IElementFindStrategy ItemsFindStrategy { get; set; }
        ElementFindOptions ItemsFindOptions { get; set; }
        IItemElementFindStrategy ItemFindStrategy { get; set; }
    }
}
