using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public abstract class XPathBuilder<TBuilder>
        where TBuilder : XPathBuilder<TBuilder>
    {
        public string XPath { get; protected set; } = string.Empty;

        public TBuilder Descendant
        {
            get { return string.IsNullOrEmpty(XPath) ? _(".//") : AppendAxis("descendant"); }
        }

        public TBuilder DescendantOrSelf
        {
            get { return AppendAxis("descendant-or-self"); }
        }

        public TBuilder Child
        {
            get { return _("/"); }
        }

        public TBuilder Self
        {
            get { return AppendAxis("self"); }
        }

        public TBuilder Parent
        {
            get { return AppendAxis("parent"); }
        }

        public TBuilder Following
        {
            get { return AppendAxis("following"); }
        }

        public TBuilder FollowingSibling
        {
            get { return AppendAxis("following-sibling"); }
        }

        public TBuilder Ancestor
        {
            get { return AppendAxis("ancestor"); }
        }

        public TBuilder AncestorOrSelf
        {
            get { return AppendAxis("ancestor-or-self"); }
        }

        public TBuilder Preceding
        {
            get { return AppendAxis("preceding"); }
        }

        public TBuilder PrecedingSibling
        {
            get { return AppendAxis("preceding-sibling"); }
        }

        public TBuilder Any
        {
            get { return _("*"); }
        }

        public TBuilder Or
        {
            get { return _(" or "); }
        }

        public TBuilder And
        {
            get { return _(" and "); }
        }

        public TBuilder this[Func<TBuilder, string> condition]
        {
            get { return Where(condition); }
        }

        public TBuilder this[object condition]
        {
            get { return Where(condition); }
        }

        protected TBuilder AppendAxis(string axisName)
        {
            string xPath = axisName + "::";

            if (!string.IsNullOrEmpty(XPath))
            {
                char lastChar = XPath.Last();

                if (!new[] { '[', '(', ' ' }.Contains(lastChar))
                    return _("/" + xPath);
            }

            return _(xPath);
        }

#pragma warning disable S100, SA1300 // Methods and properties should be named in camel case
        public TBuilder _(string xPath)
#pragma warning restore S100, SA1300 // Methods and properties should be named in camel case
        {
            TBuilder newBuidler = CreateInstance();
            newBuidler.XPath = XPath + xPath;

            return newBuidler;
        }

        public TBuilder Where(Func<TBuilder, string> condition)
        {
            string subPath = CreateSubPath(condition);
            return _($"[{subPath}]");
        }

        public TBuilder Where(object condition)
        {
            return _($"[{condition}]");
        }

        public TBuilder WherePosition(int position)
        {
            return _($"[{position}]");
        }

        public TBuilder WhereIndex(int index)
        {
            return _($"[{index + 1}]");
        }

        public TBuilder Wrap(Func<TBuilder, string> buildAction)
        {
            string subPath = CreateSubPath(buildAction);
            return _($"({subPath})");
        }

        public TBuilder WrapWithIndex(int index, Func<TBuilder, string> buildFunction)
        {
            string subPath = CreateSubPath(buildFunction);

            return _($"({subPath})[{index + 1}]");
        }

        public TBuilder WrapWithPosition(int position, Func<TBuilder, string> buildFunction)
        {
            string subPath = CreateSubPath(buildFunction);

            return _($"({subPath})[{position}]");
        }

        public TBuilder JoinOr(IEnumerable<string> conditions)
        {
            return _(string.Join(" or ", conditions));
        }

        public TBuilder JoinAnd(IEnumerable<string> conditions)
        {
            return _(string.Join(" and ", conditions));
        }

        protected abstract TBuilder CreateInstance();

        protected string CreateSubPath(Func<TBuilder, string> buildFunction)
        {
            TBuilder subBuilder = CreateInstance();
            return buildFunction(subBuilder);
        }

        public override string ToString()
        {
            return XPath;
        }
    }
}
