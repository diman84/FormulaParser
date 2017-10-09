using System;
using System.Collections.Generic;
using System.Linq;
using Expressions;

namespace Data.Calculation
{
    public class DataTransformationVisitor : IFormulaExpressionVisitor
    {
        private readonly Func<string, DataRecord> _variableProvider;
        private readonly Func<string, DataRecord> _parameterProvider;
        private readonly IDictionary<string, Func<FunctionExpression, DataRecord>> _predefinedFunctions;
        private DataRecord _result;

        public DataTransformationVisitor(Func<string, DataRecord> parameterProvider, Func<string, DataRecord> variableProvider)
        {
            _variableProvider = variableProvider;
            _parameterProvider = parameterProvider;
            _predefinedFunctions = new Dictionary<string, Func<FunctionExpression, DataRecord>>
            {
                {"CONV", Convert },
                {"SCALE", Rescale }
            };
        }

        public void Visit(IFormulaExpression expression)
        {
            expression.Accept(this);
        }

        public void Visit(BinaryExpresssion expression)
        {
            expression.LeftExpression.Accept(this);
            var left = _result;

            expression.RightExpression.Accept(this);
            var right = _result;

            switch (expression.Type)
            {
                case BinaryExpressionType.Sum:
                    _result = ResolveDataRecordBinaryOperation(left, right, left.Values.ExecuteArithmetic(right.Values, (x, y) => x + y));
                    break;
                case BinaryExpressionType.Subtraction:
                    _result = new DataRecord
                    {
                        Values = left.Values.ExecuteArithmetic(right.Values, (x, y) => x - y)
                    };
                    break;
                case BinaryExpressionType.Multiplication:
                    _result = new DataRecord
                    {
                        Values = left.Values.ExecuteArithmetic(right.Values, (x, y) => x * y)
                    };
                    break;
                case BinaryExpressionType.Division:
                    _result = new DataRecord
                    {
                        Values = left.Values.ExecuteArithmetic(right.Values, (x, y) => x / y)
                    };
                    break;
                case BinaryExpressionType.Pow:
                    _result = new DataRecord
                    {
                        Values = left.Values.ExecutePow(right.Values)
                    };
                    break;
                default:
                    throw new NotSupportedException("Binary operation type is not supported");
            }
        }

        public virtual void Visit(UnaryExpression expression)
        {
            expression.Expression.Accept(this);

            switch (expression.Type)
            {
                case UnaryExpressionType.Negate:
                    _result.Values = _result.Values.ToDictionary(x => x.Key, x => -x.Value);
                    break;
                default:
                    throw new NotSupportedException("Unary operation type is not supported");
            }
        }

        public virtual void Visit(ValueExpression expression)
        {
            throw new NotSupportedException("Value expressions are not supported in current context");
        }

        public void Visit(FunctionExpression function)
        {
            if (!_predefinedFunctions.ContainsKey(function.Identifier))
                throw new NotSupportedException($"Function {function.Identifier} is not supported");

            _result = _predefinedFunctions[function.Identifier](function);
        }

        public void Visit(ParameterExpression parameter)
        {
            _result = _parameterProvider(parameter.Name);
        }

        public void Visit(VariableExpression parameter)
        {
            throw new NotSupportedException("Variables not supported");
        }

        public DataRecord RunTransformation(IFormulaExpression expression)
        {
            expression.Accept(this);
            return _result;
        }

        private DataRecord ResolveDataRecordBinaryOperation(DataRecord left, DataRecord right, IDictionary<DateTime, double?> values)
        {
            var record = left.Type == RecordType.Unspecified && right.Type == RecordType.Unspecified
                ? new DataRecord()
                : left;

            record.Values = values;
            return record;
        }
        
        private DataRecord Rescale(FunctionExpression function)
        {
            function.Expressions.First().Accept(this);
            var parameter = function.GetBasicFunctionParameter();
            if (parameter == null)
                throw new InvalidOperationException(
                    $"Parameters of types {string.Join(", ", function.Expressions.Skip(1).Select(x => x.GetType().Name))} not supported");
            return DataFunctions.Rescale(_result, parameter);
        }

        private DataRecord Convert(FunctionExpression function)
        {
            function.Expressions.First().Accept(this);
            var parameter = function.GetBasicFunctionParameter();
            if (parameter == null)
                throw new InvalidOperationException(
                    $"Parameters of types {string.Join(", ", function.Expressions.Skip(1).Select(x => x.GetType().Name))} not supported");
            return DataFunctions.Convert(_result, _variableProvider(parameter));
        }
    }
}