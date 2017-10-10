using System;
using System.Linq;
using Antlr.Generated;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Expressions;
using ValueType = Expressions.ValueType;

namespace FormulaExpressions
{
    public class ExpressionFormulaVisitor : FormulaBaseVisitor<IFormulaExpression>
    {
        public override IFormulaExpression VisitFunc([NotNull] FormulaParser.FuncContext context)
        {
            if (context.LPAREN() != null && context.RPAREN() != null)
            {
                return new FunctionExpression(context.funcname().GetText(),
                    context.expression().Select(Visit).ToArray());
            }
            return base.VisitFunc(context);
        }
        
        public override IFormulaExpression VisitNumber([NotNull] FormulaParser.NumberContext context)
        {
            var isfloat = context.POINT() != null;
            return new ValueExpression(context.GetText(), isfloat ? ValueType.Double : ValueType.Int);
        }
        
        public override IFormulaExpression VisitExpression([NotNull] FormulaParser.ExpressionContext context)
        {
            return ParseBinary(context, ctx => ctx.multiplyingExpression().ToArray());
        }

        public override IFormulaExpression VisitPowExpression([NotNull] FormulaParser.PowExpressionContext context)
        {
            var atoms = context.atom();
            if (atoms != null)
            {
                var atom = context.atom(0);
                if (atom != null)
                {
                    return context.atom(1) != null
                        ? new BinaryExpresssion(
                            BinaryExpressionType.Pow,
                            Visit(atom),
                            Visit(context.atom(1)))

                        : Visit(atom);

                }
            }

            return base.VisitPowExpression(context);
        }

        public override IFormulaExpression VisitNegateExpression([NotNull] FormulaParser.NegateExpressionContext context)
        {
            if (context.MINUS() != null)
            {
                return new UnaryExpression(UnaryExpressionType.Negate, VisitPowExpression(context.powExpression()));
            }

            return base.VisitNegateExpression(context);
        }

        public override IFormulaExpression VisitAtom([NotNull] FormulaParser.AtomContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }
            return base.VisitAtom(context);
        }

        public override IFormulaExpression VisitMultiplyingExpression([NotNull] FormulaParser.MultiplyingExpressionContext context)
        {
            return ParseBinary(context, ctx => ctx.negateExpression().ToArray());
        }

        public override IFormulaExpression VisitVariable([NotNull] FormulaParser.VariableContext context)
        {
            return new VariableExpression
            {
                Name = context.GetText()
            };
        }

        public override IFormulaExpression VisitParameter([NotNull] FormulaParser.ParameterContext context)
        {
            if (context.LBRACKET() != null && context.RBRACKET() != null && context.DIGIT() != null)
            {
                var variable = context.variable();
                return new ParameterExpression(context.DIGIT().GetText())
                {
                    Argument =  variable != null ? variable.GetText() : null
                };
            }

            return base.VisitParameter(context);
        }

        public override IFormulaExpression VisitLiteral([NotNull] FormulaParser.LiteralContext context)
        {
            return new ValueExpression(UnescapeString(context.STRING().GetText()), ValueType.Text);
        }

        private string UnescapeString(string input)
        {
            //TODO: optimize algorithm
            return input.Trim('\'').Replace(@"\'", @"'").Replace(@"\\", @"\");
        }

        private IFormulaExpression ParseBinary<T1, T2>(T1 context, Func<T1,T2[]> expr) 
            where T1: ParserRuleContext 
            where T2 : ParserRuleContext
        {
            var expressions = expr(context);
            if (expressions.Length > 1)
            {
                var exp = Visit(expressions[0]);
                for (int i = 1; i < expressions.Length; i++)
                {
                    var token = context.GetChild<ITerminalNode>(i - 1);
                    exp = new BinaryExpresssion(GetBinaryType(token.Symbol.Type), exp, Visit(expressions[i]));
                }

                return exp;
            }

            return VisitChildren(context);
        }

        private BinaryExpressionType GetBinaryType(int type)
        {
            switch (type)
            {
                case FormulaParser.PLUS:
                    return BinaryExpressionType.Sum;
                case FormulaParser.MINUS:
                    return BinaryExpressionType.Subtraction;
                case FormulaParser.TIMES:
                    return BinaryExpressionType.Multiplication;
                case FormulaParser.DIV:
                    return BinaryExpressionType.Division;

            }

            throw new NotSupportedException();
        }
    }
}