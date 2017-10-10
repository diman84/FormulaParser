namespace Expressions
{
    public class ParameterExpression : IFormulaExpression
    {
        public ParameterExpression(int intValue)
        {
            Name = intValue.ToString();
        }

        public ParameterExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string Argument { get; set; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}