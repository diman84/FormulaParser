namespace Expressions
{
    public class ParameterExpression : IFormulaExpression
    {
        private readonly string _name;

        public ParameterExpression(int intValue)
        {
            _name = intValue.ToString();
        }

        public ParameterExpression(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Argument { get; set; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}