using Humanizer;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Atata
{
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<TRow, TOwner> : Control<TOwner>
        where TRow : TableRowBase<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
        protected string ItemKindName { get; set; }
        protected string ItemKindNamePluralized { get; set; }

        protected int? ColumnIndexToClickOnRow { get; set; }
        protected internal bool GoTemporarilyByClickOnRow { get; set; }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            if (ColumnIndexToClickOnRow == null)
            {
                var columnIndexToClickAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<ColumnIndexToClickAttribute>();
                if (columnIndexToClickAttribute != null)
                    ColumnIndexToClickOnRow = columnIndexToClickAttribute.Index;
            }

            var goTemporarilyAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<GoTemporarilyAttribute>();
            if (goTemporarilyAttribute != null)
                GoTemporarilyByClickOnRow = goTemporarilyAttribute.IsTemporarily;

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
            Log.StartVerificationSection("{0} contains column(s) {1}", ComponentFullName, columns.ToQuotedValuesListOfString(true));

            // TODO: Remake XPath.
            foreach (string column in columns)
                Scope.Get(By.XPath(".//th[contains(., '{0}')]").TableHeader(column));

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

            Assert.That(itemsCount == 1, "Failed to find '{0}' {1}", name, ItemKindName);

            return Owner;
        }

        protected IWebElement FindItem(string name, bool isFirst = false)
        {
            IWebElement item = GetItem(name, isFirst);
            Assert.NotNull(item, "Unable to locate {0} table row containing '{1}'", ItemKindName, name);
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

        protected int GetColumnIndex(string header)
        {
            return Scope.GetAll(By.TagName("th")).Select((x, i) => new { Item = x, Index = i }).Single(x => x.Item.Text == header).Index;
        }

        protected string GetColumnValue(IWebElement element, int columnIndex)
        {
            return element.GetAll(By.TagName("td")).ElementAt(columnIndex).Text;
        }

        public TRow FirstRow()
        {
            string rowName = "<first>";

            Log.StartSection("Find '{0}' table row", rowName);

            By rowBy = CreateRowBy();
            TRow row = CreateRow(rowBy, rowName);

            Log.EndSection();

            return row;
        }

        public TRow Row(params string[] values)
        {
            if (values == null || !values.Any())
                return FirstRow();

            string rowName = BuildRowName(values);

            Log.StartSection("Find '{0}' table row", rowName);

            By rowBy = CreateRowBy(values);
            TRow row = CreateRow(rowBy, rowName);

            Log.EndSection();

            return row;
        }

        public TRow Row(Expression<Func<TRow, bool>> predicateExpression)
        {
            string rowName = BuildRowName(predicateExpression);

            Log.StartSection("Find '{0}' table row", rowName);

            TRow row = CreateRow(predicateExpression, rowName);

            Log.EndSection();
            return row;
        }

        private TRow CreateRow(Expression<Func<TRow, bool>> predicateExpression, string rowName)
        {
            By rowBy = CreateRowBy();
            var predicate = predicateExpression.Compile();

            foreach (IWebElement rowElement in Scope.GetAll(rowBy))
            {
                TRow row = CreateRow(new DefinedScopeLocator(rowElement), rowName);
                if (predicate(row))
                    return row;
            }

            return CreateRow(
                new DynamicScopeLocator(options =>
                {
                    if (options.IsSafely)
                        return null;
                    else
                        throw ExceptionFactory.CreateForNoSuchElement(rowName);
                }),
                rowName);
        }

        private TRow CreateRow(By by, string name)
        {
            IScopeLocator locator = CreateRowElementFinder(by.Named(name));
            return CreateRow(locator, name);
        }

        protected virtual By CreateRowBy(params string[] values)
        {
            string condition = values != null && values.Any()
                ? string.Concat(values.Select(x => "[td[{0}]]".FormatWith(TermMatch.Contains.CreateXPathCondition(x))))
                : "[td]";

            return By.XPath(".//tr{0}".FormatWith(condition)).TableRow();
        }

        private string BuildRowName(Expression<Func<TRow, bool>> predicateExpression)
        {
            string parameterName = predicateExpression.Parameters[0].Name;
            string rowName = predicateExpression.Body.ToString();
            if (rowName.StartsWith("(") && rowName.EndsWith(")"))
                rowName = rowName.Substring(1, rowName.Length - 2);

            rowName = rowName.Replace(parameterName + ".", string.Empty);
            return rowName;
        }

        protected virtual string BuildRowName(string[] values)
        {
            if (values != null && values.Any())
                return values.ToQuotedValuesListOfString(true);
            else
                return null;
        }

        protected virtual IScopeLocator CreateRowElementFinder(By by)
        {
            return new DynamicScopeLocator(options => Scope.Get(by.With(options)));
        }

        protected virtual TRow CreateRow(IScopeLocator scopeLocator, string name)
        {
            TRow row = CreateComponent<TRow>(name);

            row.ScopeLocator = scopeLocator;
            row.ColumnIndexToClick = ColumnIndexToClickOnRow;
            row.GoTemporarily = GoTemporarilyByClickOnRow;

            return row;
        }
    }
}
