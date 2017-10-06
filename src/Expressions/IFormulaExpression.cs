namespace Expressions
{
    public interface IFormulaExpression
    {
        void Accept(IFormulaExpressionVisitor visitor);
    }
    
    public interface IFormulaExpressionVisitor
    {
        void Visit(IFormulaExpression expression);
        void Visit(BinaryExpresssion expression);
        void Visit(UnaryExpression expression);
        void Visit(ValueExpression expression);
        void Visit(FunctionExpression function);
        void Visit(ParameterExpression parameter);
    }
}