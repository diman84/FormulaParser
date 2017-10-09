using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Formatting
{
    public interface IDataRecordFormatter
    {
        string FormatRecord(DataRecord record);
    }

    class DataRecordFormatter : IDataRecordFormatter
    {
        public string FormatRecord(DataRecord record)
        {
            var sb = new StringBuilder();
            sb.Append(record.Country.PadRight(16));
            sb.Append(record.Concept.PadRight(16));
            sb.Append(record.Currency.ToString().PadRight(12));
            sb.Append(record.Scale.ToString().PadRight(12));
            sb.Append(record.Id.PadRight(12));
            sb.Append(record.Type.ToString().PadRight(20));
            foreach (var value in record.Values)
            {
                sb.Append((value.Value?.ToString("F2") ?? string.Empty).PadRight(12));
            }
            return sb.ToString();
        }
    }
}
