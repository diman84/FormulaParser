namespace Expressions
{
    public class FunctionExpression : IFormulaExpression
    {
        private readonly string _identifier;
        private readonly IFormulaExpression[] _expressions;

        public FunctionExpression(string identifier, IFormulaExpression[] expressions)
        {
            _identifier = identifier;
            _expressions = expressions;
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public IFormulaExpression[] Expressions
        {
            get { return _expressions; }
        }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

}