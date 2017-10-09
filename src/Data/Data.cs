using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data
{
    class DataParser
    {
        private const char Delimiter = ',';
        private const char Escape = '"';

        private static readonly IDictionary<string, Action<DataRecord, string>> ColMap =
            new Dictionary<string, Action<DataRecord, string>>
            {
                { "concept", (r, v) => r.Concept = v },
                { "country", (r, v) => r.Country = v },
                { "id", (r, v) => r.Id = v },
                { "currency", (r, v) => r.Currency = ParseEnum<Currency>(v) },
                { "scale", (r, v) => r.Scale = ParseEnum<Scale>(v) },
                { "type", (r, v) => r.Type = ParseEnum<RecordType>(v) }
            };

        public static Data Parse(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath");

            using (var stream = File.OpenText(filePath))
            {
                var header = stream.ReadLine();
                if (header == null)
                    throw new InvalidDataException();

                var cols = Split(header).ToArray();
                var metadataColsMap = cols.Take(ColMap.Keys.Count).ToIndexDictionary(x => x.ToLower());
                var dataColsMap = cols.Skip(ColMap.Keys.Count).ToIndexDictionary(x => x + ColMap.Keys.Count, ParseDate);

                var records = new List<DataRecord>();
                var result = new Data
                {
                    DateRange = new Period {
                        StartDate = dataColsMap.Values.Min(),
                        EndDate = dataColsMap.Values.Max()
                    },
                    Records = records
                };
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    var values = Split(line).ToArray();
                    records.Add(ParseDataRecord(values, metadataColsMap, dataColsMap));
                }

                return result;
            }
        }

        private static IEnumerable<string> Split(string input)
        {
            int start = 0;
            bool isEscaped = false;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == Escape)
                {
                    isEscaped = !isEscaped;
                }

                if (input[i] != Delimiter) continue;
                if (!isEscaped)
                {
                    yield return i > start ? input.Substring(start, i - start).Trim(Escape) : string.Empty;
                    start = i + 1;
                }
            }
            yield return input.Length > start ? input.Substring(start, input.Length - start).Trim(Escape) : string.Empty;
        }

        private static DataRecord ParseDataRecord(string[] line, IDictionary<int, string> metadataColsMap, IDictionary<int, DateTime> dataColsMap)
        {
            var record = new DataRecord();
            foreach (var col in metadataColsMap)
            {
                if (ColMap.ContainsKey(col.Value))
                    ColMap[col.Value](record, line[col.Key]);
            }

            record.Values = dataColsMap.ToDictionary(x => x.Value, x => ParseDouble(line[x.Key]));
            
            return record;
        }
        private static T ParseEnum<T>(string value) where T : struct
        {
            return Enum.TryParse<T>(value, out var enumValue) ? enumValue : default(T);
        }

        private static double? ParseDouble(string value)
        {
            return double.TryParse(value, out var val) ? val : default(double?);
        }
        private static DateTime ParseDate(string value)
        {
            var year = int.Parse(value);
            return new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }
    }

    public class Data
    {
        public static Data Load(string filePath)
        {
            return DataParser.Parse(filePath);
        }

        public IEnumerable<DataRecord> Records { get; set; }

        public Period DateRange { get; set; }
    }

    public class DataRecord
    {
        public string Country { get; set; }
        public string Concept { get; set; }
        public Scale? Scale { get; set; }
        public Currency? Currency { get; set; }
        public string Id { get; set; }
        public RecordType? Type { get; set; }

        public IDictionary<DateTime, double?> Values { get; set; }
    }

    public enum Currency
    {
        USD,
        EUR,
        GBP
    }

    public enum Scale
    {
        Unit,
        Thousands,
        Millions,
        Billions
    }

    public enum RecordType
    {
        Unspecified,
        CurrencyConversion
    }

    public class Period
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
