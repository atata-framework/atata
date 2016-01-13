using Humanizer;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Atata
{
    [UIComponent("table")]
    public class Table<TRow, TOwner> : Control<TOwner>
        where TRow : TableRowBase<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
        protected string ItemKindName { get; set; }
        protected string ItemKindNamePluralized { get; set; }

        protected TableSettingsAttribute Settings { get; set; }

        protected internal override void ApplyMetadata(UIPropertyMetadata metadata)
        {
            Settings = metadata.GetFirstOrDefaultPropertyAttribute<TableSettingsAttribute>()
                ?? new TableSettingsAttribute();

            ItemKindName = ComponentName.Singularize(false);
            ItemKindNamePluralized = ComponentName.Pluralize(false);
        }

        ////public TOwner VerifyRowsCount(string name, int value)
        ////{
        ////    Log.StartVerificationThat("'{0}' {1} count equals '{2}'", name, ItemKindNamePluralized, value);
        ////    Asserter.AreEqual(value, FindItems(name).Count());
        ////    Log.EndSection();
        ////    return Owner;
        ////}

        public TOwner VerifyColumns(params string[] columns)
        {
            Log.StartVerificationSection("'{0}' column(s) exist in table", string.Join(", ", columns));

            foreach (string column in columns)
                Scope.Get(By.XPath(".//th[contains(., '{0}')]").TableColumn(column));

            Log.EndSection();

            return Owner;
        }

        public TOwner RowExists(string name, string columnName, string columnValue)
        {
            return RowExists(name, new Dictionary<string, string>() { { columnName, columnValue } });
        }

        public TOwner RowExists(string name, Dictionary<string, string> columnValues)
        {
            Dictionary<string, int> columnIndices = columnValues.Keys.ToDictionary(x => x, x => GetColumnIndex(x));

            int itemsCount = FindItems(name).
                Where(x => columnValues.All(cv => GetColumnValue(x, columnIndices[cv.Key]) == cv.Value)).
                Count();

            Asserter.That(itemsCount == 1, "Failed to find '{0}' {1}", name, ItemKindName);

            return Owner;
        }

        protected IWebElement FindItem(string name, bool isFirst = false)
        {
            IWebElement item = GetItem(name, isFirst);
            Asserter.NotNull(item, "Unable to locate {0} table row containing '{1}'", ItemKindName, name);
            return item;
        }

        protected IWebElement[] FindItems(string name)
        {
            return Scope.GetAll(By.XPath(".//tr[contains(.,'{0}')]").TableRow(name)).ToArray();
        }

        protected IWebElement GetItem(string name, bool isFirst = false)
        {
            return Scope.Get(By.XPath(".//tr[td[contains(., '{0}')]]").TableRow(name).Safely());
        }

        protected void ClickItemLink(string name, string linkText)
        {
            var element = FindItem(name);
            element.Get(By.PartialLinkText(linkText).Immediately()).Click();
        }

        protected int GetColumnIndex(string header)
        {
            return Scope.GetAll(By.TagName("th")).Select((x, i) => new { Item = x, Index = i }).Single(x => x.Item.Text == header).Index;
        }

        protected string GetColumnValue(string name, int columnIndex)
        {
            return GetColumnValue(FindItem(name), columnIndex);
        }

        protected string GetColumnValue(IWebElement element, int columnIndex)
        {
            return element.GetAll(By.TagName("td")).ElementAt(columnIndex).Text;
        }

        public TRow FirstRow()
        {
            By rowBy = CreateRowBy();
            return CreateRow(rowBy, "first row");
        }

        public TRow Row(params string[] values)
        {
            By rowBy = CreateRowBy(values);
            string rowElementName = CreateRowElementName(values);
            return CreateRow(rowBy, rowElementName);
        }

        public TRow Row(Expression<Func<TRow, bool>> predicateExpression)
        {
            By rowBy = CreateRowBy();
            string rowElementName = "row";
            var predicate = predicateExpression.Compile();

            foreach (IWebElement rowElement in Scope.GetAll(rowBy))
            {
                TRow row = CreateRow(isSafely => rowElement, rowElementName);
                if (predicate(row))
                    return row;
            }

            return CreateRow(
                isSafely =>
                {
                    if (isSafely)
                        return null;
                    else
                        throw ExceptionsFactory.CreateForNoSuchElement(rowElementName);
                },
                rowElementName);
        }

        private TRow CreateRow(By by, string name)
        {
            ElementFinder rowElementFinder = CreateRowElementFinder(by.Named(name));
            return CreateRow(rowElementFinder, name);
        }

        protected virtual By CreateRowBy(params string[] values)
        {
            string condition = values != null && values.Any()
                ? "[{0}]".FormatWith(QualifierMatch.Contains.CreateXPathCondition(values))
                : null;
            return By.XPath(".//tr[td{0}]".FormatWith(condition)).TableRow();
        }

        protected virtual string CreateRowElementName(string[] values)
        {
            if (values != null && values.Any())
                return "row containing: {0}".FormatWith(string.Join(", ", values.Select(x => "'{0}'".FormatWith(x))));
            else
                return null;
        }

        protected virtual ElementFinder CreateRowElementFinder(By by)
        {
            return isSafely =>
                {
                    return Scope.Get(by.Safely(isSafely));
                };
        }

        protected virtual TRow CreateRow(ElementFinder elementFinder, string name)
        {
            TRow row = new TRow
            {
                ScopeElementFinder = elementFinder,
                ComponentName = name,
                Owner = Owner,
                Log = Log,
                Settings = Settings
            };

            row.InitComponent();

            return row;
        }
    }
}
