using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Calculation
{
    public static class DataFunctions
    {
        private static readonly IDictionary<string, Scale> ScaleShortcuts = new Dictionary<string, Scale>
        {
            {"B", Scale.Billions},
            {"M", Scale.Millions},
            {"T", Scale.Thousands},
            {"U", Scale.Unit}
        };

        private static readonly IDictionary<Scale, double> ScaleValues = new Dictionary<Scale, double>
        {
            {Scale.Billions, 1e9},
            {Scale.Millions, 1e6},
            {Scale.Thousands, 1e3},
            {Scale.Unit, 1}
        };

        public static DataRecord Rescale(DataRecord record, string targetScale)
        {
            if (!Enum.TryParse<Scale>(targetScale, out var scale))
            {
                scale = ScaleShortcuts.ContainsKey(targetScale)
                    ? ScaleShortcuts[targetScale]
                    : throw new NotSupportedException($"Scale {targetScale} not supported.");
            }

            record.Values = record.Values.ToDictionary(x => x.Key,
                x => x.Value / ScaleValues[scale] * ScaleValues[record.Scale ?? Scale.Unit]);

            record.Scale = scale;

            return record;
        }

        public static DataRecord Convert(DataRecord record, DataRecord conversionRecord)
        {
            if (!record.Currency.HasValue)
                return record;

            if (conversionRecord == null)
                throw new ArgumentException("conversionRecord");

            if (record.Currency != conversionRecord.Currency)
                return record;

            if (!conversionRecord.Currency.HasValue)
                throw new InvalidOperationException(
                    "Currency convertion records should contain source currency in Currency column.");

            var targetCurrencyId = conversionRecord.Id.Remove(0, 3);
            if (!Enum.TryParse<Currency>(targetCurrencyId, out var targetCurrency))
                throw new InvalidOperationException(
                    "Currency convertion records should contain target currency in Id.");

            record.Values = record.Values.ExecuteArithmetic(conversionRecord.Values, (x, y) => x * y);

            record.Currency = targetCurrency;

            return record;
        }
    }
}
