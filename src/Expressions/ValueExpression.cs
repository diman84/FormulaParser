namespace Expressions
{
    public enum ValueType
    {
        Int,
        Double,
        Text
    }

    public class ValueExpression : IFormulaExpression
    {
        private readonly string _text;
        private readonly ValueType _type;

        public ValueExpression(string text, ValueType type)
        {
            _text = text;
            _type = type;
        }

        public string Text
        {
            get { return _text; }
        }

        public ValueType Type
        {
            get { return _type; }
        }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}