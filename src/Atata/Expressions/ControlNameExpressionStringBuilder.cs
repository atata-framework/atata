using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Atata
{
    internal class ControlNameExpressionStringBuilder : ExpressionStringBuilder
    {
        private int operatorAndCount;
        private int operatorElseCount;

        protected ControlNameExpressionStringBuilder()
        {
        }

        /// <summary>
        /// Output a given expression tree to a string.
        /// </summary>
        /// <param name="node">The expression node.</param>
        /// <returns>The string representing the expression.</returns>
        public static new string ExpressionToString(Expression node)
        {
            Debug.Assert(node != null, "'node' should not be null.");

            ControlNameExpressionStringBuilder expressionStringBuilder = new ControlNameExpressionStringBuilder();
            expressionStringBuilder.Visit(node);

            string result = expressionStringBuilder.ToString();

            result = TrimBrackets(result);

            if (expressionStringBuilder.operatorAndCount > 0 && expressionStringBuilder.operatorElseCount == 0)
                return NormalizeBrackets(result, "&&");
            else if (expressionStringBuilder.operatorAndCount == 0 && expressionStringBuilder.operatorElseCount > 0)
                return NormalizeBrackets(result, "||");
            else
                return result;
        }

        private static string NormalizeBrackets(string expression, string conditionalOperator)
        {
            string conditionalOperatorWithSpaces = $" {conditionalOperator} ";

            // TODO: Handle the case when there is " && " or " || " in string variable(s).
            string[] parts = expression.Split(new[] { conditionalOperatorWithSpaces }, StringSplitOptions.RemoveEmptyEntries);

            int expectedStartOpenBraketsCount = parts.Length - 2;
            string expectedExpressionStart = new string(Enumerable.Repeat('(', expectedStartOpenBraketsCount).ToArray());

            if (parts.Length > 2 && parts[0].StartsWith(expectedExpressionStart))
            {
                parts[0] = parts[0].Substring(expectedStartOpenBraketsCount, parts[0].Length - expectedStartOpenBraketsCount);

                for (int i = 1; i < parts.Length - 1; i++)
                {
                    parts[i] = parts[i].Substring(0, parts[i].Length - 1);
                }

                return string.Join(conditionalOperatorWithSpaces, TrimBrackets(parts));
            }
            else if (parts.Length == 2)
            {
                return string.Join(conditionalOperatorWithSpaces, TrimBrackets(parts));
            }
            else
            {
                return expression;
            }
        }

        private static string[] TrimBrackets(string[] expressionParts)
        {
            for (int i = 0; i < expressionParts.Length; i++)
            {
                expressionParts[i] = TrimBrackets(expressionParts[i]);
            }

            return expressionParts;
        }

        private static string TrimBrackets(string expression)
        {
            if (expression.StartsWith("(") && expression.EndsWith(")"))
                return expression.Substring(1, expression.Length - 2);
            else
                return expression;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Visit(node.Body);
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression?.NodeType == ExpressionType.Constant)
            {
                object value = Expression.Lambda(node).Compile().DynamicInvoke();

                if (value == null)
                    Out("null");
                else if (value is string)
                    Out($"\"{value}\"");
                else
                    Out(value.ToString());

                return node;
            }
            else if (node.NodeType == ExpressionType.MemberAccess && node.Expression?.NodeType == ExpressionType.Parameter)
            {
                Out(node.Member.Name);
                return node;
            }
            else
            {
                return base.VisitMember(node);
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            bool isExtensionMethod = Attribute.GetCustomAttribute(node.Method, typeof(ExtensionAttribute)) != null;

            if (node.Object?.NodeType == ExpressionType.Parameter || (isExtensionMethod && node.Arguments[0].NodeType == ExpressionType.Parameter))
            {
                Out(node.Method.Name);
                Out("(");

                int firstArgumentIndex = isExtensionMethod ? 1 : 0;

                for (int i = firstArgumentIndex, n = node.Arguments.Count; i < n; i++)
                {
                    if (i > firstArgumentIndex)
                        Out(", ");
                    Visit(node.Arguments[i]);
                }

                Out(")");
                return node;
            }
            else
            {
                return base.VisitMethodCall(node);
            }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.AndAlso)
                operatorAndCount++;

            if (node.NodeType == ExpressionType.OrElse)
                operatorElseCount++;

            return base.VisitBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    Visit(node.Operand);
                    return node;
                default:
                    return base.VisitUnary(node);
            }
        }
    }
}
