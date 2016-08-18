using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Atata
{
    internal class ExpressionStringBuilder : ExpressionVisitor
    {
        private readonly StringBuilder builder;

        // Associate every unique label or anonymous parameter in the tree with an integer.
        // The label is displayed as Label_#.
        private Dictionary<object, int> ids;

        protected ExpressionStringBuilder()
        {
            builder = new StringBuilder();
        }

        public override string ToString()
        {
            return builder.ToString();
        }

        private void AddLabel(LabelTarget label)
        {
            if (ids == null)
            {
                ids = new Dictionary<object, int>();
                ids.Add(label, 0);
            }
            else
            {
                if (!ids.ContainsKey(label))
                {
                    ids.Add(label, ids.Count);
                }
            }
        }

        protected int GetLabelId(LabelTarget label)
        {
            if (ids == null)
            {
                ids = new Dictionary<object, int>();
                AddLabel(label);
                return 0;
            }
            else
            {
                int id;
                if (!ids.TryGetValue(label, out id))
                {
                    // label is met the first time
                    id = ids.Count;
                    AddLabel(label);
                }
                return id;
            }
        }

        private void AddParam(ParameterExpression p)
        {
            if (ids == null)
            {
                ids = new Dictionary<object, int>();
                ids.Add(ids, 0);
            }
            else
            {
                if (!ids.ContainsKey(p))
                {
                    ids.Add(p, ids.Count);
                }
            }
        }

        protected int GetParamId(ParameterExpression p)
        {
            if (ids == null)
            {
                ids = new Dictionary<object, int>();
                AddParam(p);
                return 0;
            }
            else
            {
                int id;
                if (!ids.TryGetValue(p, out id))
                {
                    // p is met the first time
                    id = ids.Count;
                    AddParam(p);
                }
                return id;
            }
        }

        protected void Out(string s)
        {
            builder.Append(s);
        }

        protected void Out(char c)
        {
            builder.Append(c);
        }

        /// <summary>
        /// Output a given expression tree to a string.
        /// </summary>
        public static string ExpressionToString(Expression node)
        {
            Debug.Assert(node != null, "'node' should not be null.");

            ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
            expressionStringBuilder.Visit(node);
            return expressionStringBuilder.ToString();
        }

        // More proper would be to make this a virtual method on Action
        private static string FormatBinder(CallSiteBinder binder)
        {
            ConvertBinder convert;
            GetMemberBinder getMember;
            SetMemberBinder setMember;
            DeleteMemberBinder deleteMember;
            InvokeMemberBinder call;
            UnaryOperationBinder unary;
            BinaryOperationBinder binary;

            if ((convert = binder as ConvertBinder) != null)
            {
                return "Convert " + convert.Type;
            }
            else if ((getMember = binder as GetMemberBinder) != null)
            {
                return "GetMember " + getMember.Name;
            }
            else if ((setMember = binder as SetMemberBinder) != null)
            {
                return "SetMember " + setMember.Name;
            }
            else if ((deleteMember = binder as DeleteMemberBinder) != null)
            {
                return "DeleteMember " + deleteMember.Name;
            }
            else if (binder is GetIndexBinder)
            {
                return "GetIndex";
            }
            else if (binder is SetIndexBinder)
            {
                return "SetIndex";
            }
            else if (binder is DeleteIndexBinder)
            {
                return "DeleteIndex";
            }
            else if ((call = binder as InvokeMemberBinder) != null)
            {
                return "Call " + call.Name;
            }
            else if (binder is InvokeBinder)
            {
                return "Invoke";
            }
            else if (binder is CreateInstanceBinder)
            {
                return "Create";
            }
            else if ((unary = binder as UnaryOperationBinder) != null)
            {
                return unary.Operation.ToString();
            }
            else if ((binary = binder as BinaryOperationBinder) != null)
            {
                return binary.Operation.ToString();
            }
            else
            {
                return "CallSiteBinder";
            }
        }

        private void VisitExpressions<T>(char open, IList<T> expressions, char close) where T : Expression
        {
            VisitExpressions(open, expressions, close, ", ");
        }

        private void VisitExpressions<T>(char open, IList<T> expressions, char close, string seperator) where T : Expression
        {
            Out(open);
            if (expressions != null)
            {
                bool isFirst = true;
                foreach (T e in expressions)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        Out(seperator);
                    }
                    Visit(e);
                }
            }
            Out(close);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            Out(FormatBinder(node.Binder));
            VisitExpressions('(', node.Arguments, ')');
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.ArrayIndex)
            {
                Visit(node.Left);
                Out("[");
                Visit(node.Right);
                Out("]");
            }
            else
            {
                string operatorString = GetBinaryOperator(node.NodeType);

                Out("(");
                Visit(node.Left);
                Out(' ');
                Out(operatorString);
                Out(' ');
                Visit(node.Right);
                Out(")");
            }
            return node;
        }

        protected virtual string GetBinaryOperator(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.AndAlso:
                    return "&&";
                case ExpressionType.OrElse:
                    return "||";
                case ExpressionType.Assign:
                    return "=";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                    return "+=";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    return "-=";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.DivideAssign:
                    return "/=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.ModuloAssign:
                    return "%=";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                    return "*=";
                case ExpressionType.LeftShift:
                    return "<<";
                case ExpressionType.LeftShiftAssign:
                    return "<<=";
                case ExpressionType.RightShift:
                    return ">>";
                case ExpressionType.RightShiftAssign:
                    return ">>=";
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAssign:
                    return "&=";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrAssign:
                    return "|=";
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                    return "^";
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.PowerAssign:
                    return "^=";
                case ExpressionType.Coalesce:
                    return "??";
                default:
                    throw new InvalidOperationException();
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.IsByRef)
            {
                Out("ref ");
            }
            string name = node.Name;
            if (string.IsNullOrEmpty(name))
            {
                Out("Param_" + GetParamId(node));
            }
            else
            {
                Out(name);
            }
            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (node.Parameters.Count == 1)
            {
                // p => body
                Visit(node.Parameters[0]);
            }
            else
            {
                // (p1, p2, ..., pn) => body
                VisitExpressions('(', node.Parameters, ')');
            }
            Out(" => ");
            Visit(node.Body);
            return node;
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            Visit(node.NewExpression);
            Out(" {");
            for (int i = 0, n = node.Initializers.Count; i < n; i++)
            {
                if (i > 0)
                {
                    Out(", ");
                }
                Out(node.Initializers[i].ToString());
            }
            Out("}");
            return node;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Out("IIF(");
            Visit(node.Test);
            Out(", ");
            Visit(node.IfTrue);
            Out(", ");
            Visit(node.IfFalse);
            Out(")");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value != null)
            {
                string valueAsString = node.Value.ToString();
                if (node.Value is string)
                {
                    Out("\"");
                    Out(valueAsString);
                    Out("\"");
                }
                else if (valueAsString == node.Value.GetType().ToString())
                {
                    Out("value(");
                    Out(valueAsString);
                    Out(")");
                }
                else
                {
                    Out(valueAsString);
                }
            }
            else
            {
                Out("null");
            }
            return node;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            string s = string.Format(
                CultureInfo.CurrentCulture,
                "<DebugInfo({0}: {1}, {2}, {3}, {4})>",
                node.Document.FileName,
                node.StartLine,
                node.StartColumn,
                node.EndLine,
                node.EndColumn);

            Out(s);

            return node;
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            VisitExpressions('(', node.Variables, ')');
            return node;
        }

        // Prints ".instanceField" or "declaringType.staticField"
        protected virtual void OutMember(Expression instance, MemberInfo member)
        {
            if (instance != null)
            {
                Visit(instance);
                Out("." + member.Name);
            }
            else
            {
                // For static members, include the type name
                Out(member.DeclaringType.Name + "." + member.Name);
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            OutMember(node.Expression, node.Member);
            return node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            if (node.NewExpression.Arguments.Count == 0 &&
                node.NewExpression.Type.Name.Contains("<"))
            {
                // anonymous type constructor
                Out("new");
            }
            else
            {
                Visit(node.NewExpression);
            }
            Out(" {");
            for (int i = 0, n = node.Bindings.Count; i < n; i++)
            {
                MemberBinding b = node.Bindings[i];
                if (i > 0)
                {
                    Out(", ");
                }
                VisitMemberBinding(b);
            }
            Out("}");
            return node;
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Out(assignment.Member.Name);
            Out(" = ");
            Visit(assignment.Expression);
            return assignment;
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            Out(binding.Member.Name);
            Out(" = {");
            for (int i = 0, n = binding.Initializers.Count; i < n; i++)
            {
                if (i > 0)
                {
                    Out(", ");
                }
                VisitElementInit(binding.Initializers[i]);
            }
            Out("}");
            return binding;
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            Out(binding.Member.Name);
            Out(" = {");
            for (int i = 0, n = binding.Bindings.Count; i < n; i++)
            {
                if (i > 0)
                {
                    Out(", ");
                }
                VisitMemberBinding(binding.Bindings[i]);
            }
            Out("}");
            return binding;
        }

        protected override ElementInit VisitElementInit(ElementInit initializer)
        {
            Out(initializer.AddMethod.ToString());
            string sep = ", ";

            VisitExpressions('(', initializer.Arguments, ')', sep);
            return initializer;
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            Out("Invoke(");
            Visit(node.Expression);
            string sep = ", ";

            for (int i = 0, n = node.Arguments.Count; i < n; i++)
            {
                Out(sep);
                Visit(node.Arguments[i]);
            }
            Out(")");
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            int start = 0;
            Expression objectExpression = node.Object;

            if (Attribute.GetCustomAttribute(node.Method, typeof(ExtensionAttribute)) != null)
            {
                start = 1;
                objectExpression = node.Arguments[0];
            }

            if (objectExpression != null)
            {
                Visit(objectExpression);
                Out(".");
            }
            Out(node.Method.Name);
            Out("(");
            for (int i = start, n = node.Arguments.Count; i < n; i++)
            {
                if (i > start)
                    Out(", ");
                Visit(node.Arguments[i]);
            }
            Out(")");
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            if (node.NodeType == ExpressionType.NewArrayBounds)
            {
                Out("new " + node.Type);
                VisitExpressions('(', node.Expressions, ')');
            }
            else if (node.NodeType == ExpressionType.NewArrayInit)
            {
                Out("new [] ");
                VisitExpressions('{', node.Expressions, '}');
            }
            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            Out("new " + node.Type.Name);
            Out("(");
            var members = node.Members;
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                if (i > 0)
                {
                    Out(", ");
                }
                if (members != null)
                {
                    string name = members[i].Name;

                    Out(name);
                    Out(" = ");
                }
                Visit(node.Arguments[i]);
            }
            Out(")");
            return node;
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Out("(");
            Visit(node.Expression);

            if (node.NodeType == ExpressionType.TypeIs)
                Out(" is ");
            else if (node.NodeType == ExpressionType.TypeEqual)
                Out(" TypeEqual ");

            Out(node.TypeOperand.Name);
            Out(")");
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.TypeAs:
                    Out("(");
                    break;
                case ExpressionType.Not:
                    if (node.Type == typeof(bool) || node.Type == typeof(bool?))
                        Out("!");
                    else
                        Out("~");
                    break;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    Out("-");
                    break;
                case ExpressionType.UnaryPlus:
                    Out("+");
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.Throw:
                    Out("throw(");
                    break;
                case ExpressionType.Increment:
                    Out("Increment(");
                    break;
                case ExpressionType.Decrement:
                    Out("Decrement(");
                    break;
                case ExpressionType.PreIncrementAssign:
                    Out("++");
                    break;
                case ExpressionType.PreDecrementAssign:
                    Out("--");
                    break;
                case ExpressionType.OnesComplement:
                    Out("~(");
                    break;
                default:
                    Out(node.NodeType.ToString());
                    Out("(");
                    break;
            }

            Visit(node.Operand);

            switch (node.NodeType)
            {
                case ExpressionType.Not:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.UnaryPlus:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Quote:
                    break;
                case ExpressionType.TypeAs:
                    Out(" as ");
                    Out(node.Type.Name);
                    Out(")");
                    break;
                case ExpressionType.PostIncrementAssign:
                    Out("++");
                    break;
                case ExpressionType.PostDecrementAssign:
                    Out("--");
                    break;
                default:
                    Out(")");
                    break;
            }
            return node;
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            Out("{");
            foreach (var v in node.Variables)
            {
                Out("var ");
                Visit(v);
                Out(";");
            }
            Out(" ... }");
            return node;
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            Out("default(");
            Out(node.Type.Name);
            Out(")");
            return node;
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            Out("{ ... } ");
            DumpLabel(node.Target);
            Out(":");
            return node;
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            Out(node.Kind.ToString().ToLower(CultureInfo.CurrentCulture));
            DumpLabel(node.Target);
            if (node.Value != null)
            {
                Out(" (");
                Visit(node.Value);
                Out(") ");
            }
            return node;
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            Out("loop { ... }");
            return node;
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            Out("case ");
            VisitExpressions('(', node.TestValues, ')');
            Out(": ...");
            return node;
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            Out("switch ");
            Out("(");
            Visit(node.SwitchValue);
            Out(") { ... }");
            return node;
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            Out("catch (" + node.Test.Name);
            if (node.Variable != null)
            {
                Out(node.Variable.Name ?? string.Empty);
            }
            Out(") { ... }");
            return node;
        }

        protected override Expression VisitTry(TryExpression node)
        {
            Out("try { ... }");
            return node;
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            if (node.Object != null)
            {
                Visit(node.Object);
            }
            else
            {
                Debug.Assert(node.Indexer != null, "'node.Indexer' should not be null.");
                Out(node.Indexer.DeclaringType.Name);
            }
            if (node.Indexer != null)
            {
                Out(".");
                Out(node.Indexer.Name);
            }

            VisitExpressions('[', node.Arguments, ']');
            return node;
        }

        protected override Expression VisitExtension(Expression node)
        {
            // Prefer an overriden ToString, if available.
            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.ExactBinding;
            var toString = node.GetType().GetMethod("ToString", flags, null, Type.EmptyTypes, null);
            if (toString.DeclaringType != typeof(Expression))
            {
                Out(node.ToString());
                return node;
            }

            Out("[");

            // For 3.5 subclasses, print the NodeType.
            // For Extension nodes, print the class name.
            if (node.NodeType == ExpressionType.Extension)
            {
                Out(node.GetType().FullName);
            }
            else
            {
                Out(node.NodeType.ToString());
            }
            Out("]");
            return node;
        }

        private void DumpLabel(LabelTarget target)
        {
            if (!string.IsNullOrEmpty(target.Name))
            {
                Out(target.Name);
            }
            else
            {
                int labelId = GetLabelId(target);
                Out("UnamedLabel_" + labelId);
            }
        }
    }
}
