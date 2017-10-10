using System;
using System.Collections.Generic;
using System.Linq;
using Data.Formatting;
using Expressions;

namespace Data
{

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
