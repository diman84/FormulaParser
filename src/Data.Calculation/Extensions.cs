using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressions;

namespace Data.Calculation
{

    public static class FunctionExtensions
    {
        public static string GetBasicFunctionParameter(this FunctionExpression function)
        {
            if (function.Expressions.Length <= 1)
                return null;

            var parameterValue = function.Expressions[1] as ValueExpression;

            return parameterValue?.Text;
        }
    }

    public static class CalculationExtensions
    {

        public static IDictionary<DateTime, double?> ExecuteArithmetic(this IDictionary<DateTime, double?> left,
            IDictionary<DateTime, double?> right,
            Func<double, double, double> operation)
        {
            return left.ToDictionary(l => l.Key, ExecuteArithmetic(right, operation));
        }

        public static IDictionary<DateTime, double?> ExecutePow(this IDictionary<DateTime, double?> left,
            IDictionary<DateTime, double?> right)
        {
            return left.ToDictionary(l => l.Key, ExecutePow(right));
        }

        private static Func<KeyValuePair<DateTime, double?>, double?> ExecuteArithmetic(
            IDictionary<DateTime, double?> right,
            Func<double, double, double> operation)
        {
            return left =>
            {
                if (!right.ContainsKey(left.Key) || !right[left.Key].HasValue)
                    return left.Value;

                if (!left.Value.HasValue)
                    return operation(0, right[left.Key].Value);

                return operation(left.Value.Value, right[left.Key].Value);
            };
        }

        private static Func<KeyValuePair<DateTime, double?>, double?> ExecutePow(IDictionary<DateTime, double?> right)
        {
            return left =>
            {
                if (!right.ContainsKey(left.Key) || !right[left.Key].HasValue)
                    return 1;

                if (!left.Value.HasValue)
                    return 0;

                return Math.Pow(left.Value.Value, right[left.Key].Value);
            };
        }
    }
}
