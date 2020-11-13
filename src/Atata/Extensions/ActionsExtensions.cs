using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
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

            switch (offsetKind)
            {
                case UIComponentOffsetKind.FromCenterInPercents:
                    return actions.MoveToElement(
                        toElement,
                        (int)(elementSize.Width * ((float)offsetX / 100)),
                        (int)(elementSize.Height * ((float)offsetY / 100)),
                        MoveToElementOffsetOrigin.Center);
                case UIComponentOffsetKind.FromCenterInPixels:
                    return actions.MoveToElement(
                        toElement,
                        offsetX,
                        offsetY,
                        MoveToElementOffsetOrigin.Center);
                case UIComponentOffsetKind.FromTopLeftInPercents:
                    return actions.MoveToElement(
                        toElement,
                        (int)(elementSize.Width * ((float)offsetX / 100)) - (elementSize.Width / 2),
                        (int)(elementSize.Height * ((float)offsetY / 100)) - (elementSize.Height / 2),
                        MoveToElementOffsetOrigin.Center);
                case UIComponentOffsetKind.FromTopLeftInPixels:
                    return actions.MoveToElement(
                        toElement,
                        offsetX - (elementSize.Width / 2),
                        offsetY - (elementSize.Height / 2),
                        MoveToElementOffsetOrigin.Center);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(offsetKind, nameof(offsetKind));
            }
        }
    }
}
