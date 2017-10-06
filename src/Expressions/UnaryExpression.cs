namespace Expressions
{
    public enum UnaryExpressionType
    {
        Negate
    }

    public class UnaryExpression : IFormulaExpression
    {
        private readonly IFormulaExpression _expression;
        private readonly UnaryExpressionType _type;

        public UnaryExpression(UnaryExpressionType type, IFormulaExpression expression)
        {
            _type = type;
            _expression = expression;
        }

        public IFormulaExpression Expression
        {
            get { return _expression; }
        }

        public UnaryExpressionType Type
        {
            get { return _type; }
        }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}