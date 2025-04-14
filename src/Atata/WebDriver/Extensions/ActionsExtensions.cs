namespace Atata;

public static class ActionsExtensions
{
    public static Actions MoveToElement(
        this Actions actions,
        IWebElement toElement,
        int offsetX,
        int offsetY,
        UIComponentOffsetKind offsetKind)
    {
        if (offsetX == 0 && offsetY == 0 && (offsetKind == UIComponentOffsetKind.FromCenterInPercents || offsetKind == UIComponentOffsetKind.FromCenterInPixels))
            return actions.MoveToElement(toElement);

        Size elementSize = toElement.Size;

        return offsetKind switch
        {
            UIComponentOffsetKind.FromCenterInPercents =>
                actions.MoveToElement(
                    toElement,
                    (int)(elementSize.Width * ((float)offsetX / 100)),
                    (int)(elementSize.Height * ((float)offsetY / 100))),
            UIComponentOffsetKind.FromCenterInPixels =>
                actions.MoveToElement(
                    toElement,
                    offsetX,
                    offsetY),
            UIComponentOffsetKind.FromTopLeftInPercents =>
                actions.MoveToElement(
                    toElement,
                    (int)(elementSize.Width * ((float)offsetX / 100)) - (elementSize.Width / 2),
                    (int)(elementSize.Height * ((float)offsetY / 100)) - (elementSize.Height / 2)),
            UIComponentOffsetKind.FromTopLeftInPixels =>
                actions.MoveToElement(
                    toElement,
                    offsetX - (elementSize.Width / 2),
                    offsetY - (elementSize.Height / 2)),
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(offsetKind)
        };
    }
}
