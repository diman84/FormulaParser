namespace Expressions
{
    public enum UnaryExpressionType
    {
        Negate
    }

    public class UnaryExpression : IFormulaExpression
    {
        public UnaryExpression(UnaryExpressionType type, IFormulaExpression expression)
        {
            Type = type;
            Expression = expression;
        }

        public IFormulaExpression Expression { get; }

        public UnaryExpressionType Type { get; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}