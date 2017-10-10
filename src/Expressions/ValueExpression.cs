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
        public ValueExpression(string text, ValueType type)
        {
            Text = text;
            Type = type;
        }

        public string Text { get; }

        public ValueType Type { get; }

        public void Accept(IFormulaExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}