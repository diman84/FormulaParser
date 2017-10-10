namespace Expressions
{
    public enum BinaryExpressionType
    {
        Sum,
        Subtraction,
        Multiplication,
        Division,
        Pow
    }

    public class BinaryExpresssion : IFormulaExpression
    {
        public BinaryExpresssion(BinaryExpressionType type, IFormulaExpression leftExpression, IFormulaExpression rightExpression)
        {
            Type = type;
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }

        public IFormulaExpression LeftExpression { get; }

        public IFormulaExpression RightExpression { get; }

        public BinaryExpressionType Type { get; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
