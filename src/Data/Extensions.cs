using System;
using System.Collections.Generic;
using System.Linq;
using Data.Formatting;
using Expressions;

namespace Data
{
    public static class CalculationExtensions {

        public static IDictionary<DateTime, double?> ExecuteArithmetic(this IDictionary<DateTime, double?> left,
            IDictionary<DateTime, double?> right,
            Func<double, double, double> operation)
        {
            return left.ToDictionary(l => l.Key, ExecuteArithmetic(right, operation));
        }

        public static IDictionary<DateTime, double?> ExecutePow(this IDictionary<DateTime, double?> left, IDictionary<DateTime, double?> right)
        {
            return left.ToDictionary(l => l.Key, ExecutePow(right));
        }

        private static Func<KeyValuePair<DateTime, double?>, double?> ExecuteArithmetic(IDictionary<DateTime, double?> right,
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

    public static class DataExtensions
    {
        public static string FormatRecord(this DataRecord record, IDataRecordFormatter formatter = null)
        {
            return (formatter ?? DefaultFormatter).FormatRecord(record);
        }

        public static string FormatHeader(this Data data)
        {
            var startYear = data.DateRange?.StartDate.Year ?? 0;
            var endYear = data.DateRange?.EndDate.Year ?? 0;
            return "Country         Concept         Currency    Scale       Id          Type                " +
                   string.Join("        ",
                       Enumerable.Range(startYear, endYear - startYear));
        }

        static readonly IDataRecordFormatter DefaultFormatter = new DataRecordFormatter();
    }

    public static class EnumerableExtensions
    {
        
        public static IDictionary<int ,T> ToIndexDictionary<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToIndexDictionary(x => x);
        }

        public static IDictionary<int, TValue> ToIndexDictionary<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> valueSelector)
        {
            return enumerable.ToIndexDictionary(x => x, valueSelector);
        }

        public static IDictionary<int, TValue> ToIndexDictionary<T, TValue>(this IEnumerable<T> enumerable, Func<int, int> keySelector, Func<T, TValue> valueSelector)
        {
            return enumerable.Select((value, index) => new { value, index })
                .ToDictionary(x => keySelector(x.index), x => valueSelector(x.value));
        }
    }
}
