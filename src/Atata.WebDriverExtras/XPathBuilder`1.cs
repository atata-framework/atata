using System;
using System.Collections.Generic;

namespace Atata
{
    public abstract class XPathBuilder<TBuilder>
        where TBuilder : XPathBuilder<TBuilder>
    {
        public string XPath { get; protected set; } = string.Empty;

        public TBuilder Descendant
        {
            get { return _(string.IsNullOrEmpty(XPath) ? ".//" : "/descendant::"); }
        }

        public TBuilder DescendantOrSelf
        {
            get { return _((string.IsNullOrEmpty(XPath) ? string.Empty : "/") + "descendant-or-self::"); }
        }

        public TBuilder Self
        {
            get { return _((string.IsNullOrEmpty(XPath) ? string.Empty : "/") + "self::"); }
        }

        public TBuilder FollowingSibling
        {
            get { return _((string.IsNullOrEmpty(XPath) ? string.Empty : "/") + "following-sibling::"); }
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
