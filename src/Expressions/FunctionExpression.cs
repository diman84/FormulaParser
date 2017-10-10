namespace Expressions
{
    public class FunctionExpression : IFormulaExpression
    {
        public FunctionExpression(string identifier, IFormulaExpression[] expressions)
        {
            Identifier = identifier;
            Expressions = expressions;
        }

        public string Identifier { get; }

        public IFormulaExpression[] Expressions { get; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

}