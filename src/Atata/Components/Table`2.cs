using System;
using System.Linq;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace Atata
{
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<TRow, TOwner> : Control<TOwner>
        where TRow : TableRowBase<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
        private readonly string rowScopeXPath;

        public Table()
        {
            ControlDefinitionAttribute controlDefinition = UIComponentResolver.GetControlDefinition(typeof(TRow));
            rowScopeXPath = (controlDefinition != null ? controlDefinition.ScopeXPath : null) ?? "tr";
        }

        public TableRowList<TRow, TOwner> Rows { get; private set; }

        public ControlList<TableHeader<TOwner>, TOwner> Headers { get; private set; }

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
        }

        public TOwner VerifyColumns(params string[] columns)
        {
            Log.StartVerificationSection("{0} contains column(s) {1}", ComponentFullName, columns.ToQuotedValuesListOfString(true));

            // TODO: Remake XPath.
            foreach (string column in columns)
                Scope.Exists(By.XPath(".//th[contains(., '{0}')]").TableHeader(column));

            Log.EndSection();

            return Owner;
        }

        public TRow FirstRow()
        {
            string rowName = "<first>";

            Log.StartSection("Find \"{0}\" table row", rowName);

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

            Log.StartSection("Find \"{0}\" table row", rowName);

            By rowBy = CreateRowBy(values);
            TRow row = CreateRow(rowBy, rowName);

            Log.EndSection();

            return row;
        }

        public TRow Row(Expression<Func<TRow, bool>> predicateExpression)
        {
            string rowName = BuildRowName(predicateExpression);

            Log.StartSection("Find \"{0}\" table row", rowName);

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

            return By.XPath(".//{0}{1}".FormatWith(rowScopeXPath, condition)).TableRow();
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
            if (values == null || !values.Any())
                return null;
            else if (values.Length == 1)
                return values.First();
            else
                return values.ToQuotedValuesListOfString(true);
        }

        protected virtual IScopeLocator CreateRowElementFinder(By by)
        {
            return new DynamicScopeLocator(options => Scope.Get(by.With(options)));
        }

        protected virtual TRow CreateRow(IScopeLocator scopeLocator, string name)
        {
            TRow row = CreateControl<TRow>(name);

            row.ScopeLocator = scopeLocator;
            row.ColumnIndexToClick = ColumnIndexToClickOnRow;
            row.GoTemporarily = GoTemporarilyByClickOnRow;

            return row;
        }
    }
}
