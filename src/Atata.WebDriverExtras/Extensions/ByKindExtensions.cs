using OpenQA.Selenium;

namespace Atata
{
    public static class ByKindExtensions
    {
        public static By Button(this By by, string name = null)
        {
            return by.OfKind("button", name);
        }

        public static By Link(this By by, string name = null)
        {
            return by.OfKind("link", name);
        }

        public static By Input(this By by, string name = null)
        {
            return by.OfKind("input", name);
        }

        public static By Field(this By by, string name = null)
        {
            return by.OfKind("field", name);
        }

        public static By DropDown(this By by, string name = null)
        {
            return by.OfKind("drop-down", name);
        }

        public static By DropDownList(this By by, string name = null)
        {
            return by.OfKind("drop-down list", name);
        }

        public static By DropDownOption(this By by, string name = null)
        {
            return by.OfKind("drop-down option", name);
        }

        public static By DropDownButton(this By by, string name = null)
        {
            return by.OfKind("drop-down button", name);
        }

        public static By Table(this By by, string name = null)
        {
            return by.OfKind("table", name);
        }

        public static By TableColumn(this By by, string name = null)
        {
            return by.OfKind("table column", name);
        }

        public static By TableRow(this By by, string name = null)
        {
            return by.OfKind("table row", name);
        }

        public static By TableHeader(this By by, string name = null)
        {
            return by.OfKind("table header", name);
        }

        public static By Label(this By by, string name = null)
        {
            return by.OfKind("label", name);
        }

        public static By Popup(this By by, string name = null)
        {
            return by.OfKind("popup", name);
        }

        public static By Window(this By by, string name = null)
        {
            return by.OfKind("window", name);
        }

        public static By PopupWindow(this By by, string name = null)
        {
            return by.OfKind("popup window", name);
        }

        public static By Dialog(this By by, string name = null)
        {
            return by.OfKind("dialog", name);
        }

        public static By Menu(this By by, string name = null)
        {
            return by.OfKind("menu", name);
        }

        public static By DropDownMenu(this By by, string name = null)
        {
            return by.OfKind("drop-down menu", name);
        }
    }
}
