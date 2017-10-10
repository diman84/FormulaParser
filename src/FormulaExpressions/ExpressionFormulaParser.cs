using Antlr.Generated;
using Antlr4.Runtime;
using Expressions;

namespace FormulaExpressions
{
    public static class ExpressionFormulaParser
    {
        public static IFormulaExpression Parse(string formula)
        {
            AntlrInputStream input = new AntlrInputStream(formula);
            FormulaLexer lexer = new FormulaLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            FormulaParser parser = new FormulaParser(tokens);
            var visitor = new ExpressionFormulaVisitor();
            return visitor.Visit(parser.expression());
        }
    }
}