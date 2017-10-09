using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr.Expressions;
using Data;
using Data.Calculation;

namespace Client
{
    class Program
    {
        private const string Transformation = "SCALE(CONV({0}, 'EURUSD'), 'M')";

        static void Main(string[] args)
        {
            var formulaExpression = ExpressionFormulaParser.Parse(Transformation);
            var data = Data.Data.Load("data.csv");
            
            Console.WriteLine("Initial data:");
            Console.WriteLine(data.FormatHeader());
            foreach (var record in data.Records)
            {
                Console.WriteLine(record.FormatRecord());
            }
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("Transformed data:");
            Console.WriteLine(data.FormatHeader());
            var calculator = new DataCalculator(data);
            foreach (var record in data.Records.Where(x=>x.Type == Data.RecordType.Unspecified))
            {
                Console.WriteLine(calculator.TransformData(record, formulaExpression).FormatRecord());
            }
            Console.ReadLine();
        }
    }
}
