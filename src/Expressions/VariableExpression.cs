namespace Expressions
{
    public class VariableExpression : IFormulaExpression
    {
        public string Name { get; set; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}