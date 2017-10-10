using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressions;

namespace Data.Calculation
{
    public class DataCalculator
    {
        private DataRecord[] _records;
        private readonly Dictionary<string, DataRecord> _recordsById;

        public DataCalculator(Data data)
        {
            _records = data.Records.ToArray();
            _recordsById = data.Records.ToDictionary(x => x.Id);
        }

        public DataRecord TransformData(DataRecord record, IFormulaExpression formulaExpression)
        {
            var visitor = new DataTransformationVisitor(x => record, FindById);
            return visitor.RunTransformation(formulaExpression);
        }

        private DataRecord FindById(string arg)
        {
            return _recordsById.ContainsKey(arg) ? _recordsById[arg] : null;
        }
    }
}
