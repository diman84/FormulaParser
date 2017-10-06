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
        private readonly IFormulaExpression _leftExpression;
        private readonly IFormulaExpression _rightExpression;
        private readonly BinaryExpressionType _type;

        public BinaryExpresssion(BinaryExpressionType type, IFormulaExpression leftExpression, IFormulaExpression rightExpression)
        {
            _type = type;
            _leftExpression = leftExpression;
            _rightExpression = rightExpression;
        }

        public IFormulaExpression LeftExpression
        {
            get { return _leftExpression; }
        }

        public IFormulaExpression RightExpression
        {
            get { return _rightExpression; }
        }

        public BinaryExpressionType Type
        {
            get { return _type; }
        }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
