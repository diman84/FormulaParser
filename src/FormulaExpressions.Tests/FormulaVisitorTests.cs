using NUnit.Framework;
using Antlr.Expressions;
using Expressions;
using System.Linq;

namespace FormulaExpressions.Tests
{
    [TestFixture]
    public class FormulaVisitorTests
    {
        [Test]
        public void ShouldCreateBinaryExpression()
        {
            //arrange
            var formula = "2 - 8";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);
        }

        [Test]
        public void ShouldSetCorrectExpressionTypeForBinaryExpression()
        {
            //arrange
            var formula = "2 + 8";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);
            Assert.AreEqual(BinaryExpressionType.Sum, ((BinaryExpresssion)result).Type);
        }

        [Test]
        public void ShouldRespectArithmeticOperationPriority()
        {
            //arrange
            var formula = "2 * 8 - 4";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);
            Assert.AreEqual(((BinaryExpresssion)result).Type, BinaryExpressionType.Subtraction);
            Assert.AreEqual(((BinaryExpresssion)((BinaryExpresssion)result).LeftExpression).Type, BinaryExpressionType.Multiplication);
        }

        [Test]
        public void ShouldCreatePowExpression()
        {
            //arrange
            var formula = "2^8";

            //act
            var result = Parse(formula);

            //assert 
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);

            Assert.AreEqual(((BinaryExpresssion)result).Type, BinaryExpressionType.Pow);
        }

        [Test]
        public void ShouldRespectPowOperationPriority()
        {
            //arrange
            var formula = "{0} ^ 2 + 3 * 5";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);
            Assert.AreEqual(((BinaryExpresssion)result).Type, BinaryExpressionType.Sum);
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), ((BinaryExpresssion)result).LeftExpression);
            Assert.AreEqual(((BinaryExpresssion)((BinaryExpresssion)result).LeftExpression).Type, BinaryExpressionType.Pow);
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), ((BinaryExpresssion)result).RightExpression);
            Assert.AreEqual(((BinaryExpresssion)((BinaryExpresssion)result).RightExpression).Type, BinaryExpressionType.Multiplication);
        }

        [Test]
        public void ShouldRespectPowOperationPriorityWithParenthes()
        {
            //arrange
            var formula = "{0} ^ (2 + 3) * 5";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);
            Assert.AreEqual(((BinaryExpresssion)result).Type, BinaryExpressionType.Multiplication);
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), ((BinaryExpresssion)result).LeftExpression);
            Assert.AreEqual(((BinaryExpresssion)((BinaryExpresssion)result).LeftExpression).Type, BinaryExpressionType.Pow);
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), ((BinaryExpresssion)((BinaryExpresssion)result).LeftExpression).RightExpression);
            Assert.IsAssignableFrom(typeof(ParameterExpression), ((BinaryExpresssion)((BinaryExpresssion)result).LeftExpression).LeftExpression);
            Assert.IsAssignableFrom(typeof(ValueExpression), ((BinaryExpresssion)result).RightExpression);
        }


        [Test]
        public void ShouldCreateUnaryExpression()
        {
            //arrange
            var formula = "-8";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(UnaryExpression), result);
            Assert.AreEqual(((UnaryExpression)result).Type, UnaryExpressionType.Negate);
        }

        [Test]
        public void ShouldRespectNegateOperationPriority()
        {
            //arrange
            var formula = "-8*2";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), result);
            Assert.IsAssignableFrom(typeof(UnaryExpression), ((BinaryExpresssion)result).LeftExpression);
            Assert.AreEqual(((UnaryExpression)((BinaryExpresssion)result).LeftExpression).Type, UnaryExpressionType.Negate);
        }

        [Test]
        public void ShouldRespectNegateAndPowOperationPriority()
        {
            //arrange
            var formula = "-8^2";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(UnaryExpression), result);
            Assert.AreEqual(((UnaryExpression)result).Type, UnaryExpressionType.Negate);
            Assert.IsAssignableFrom(typeof(BinaryExpresssion), ((UnaryExpression)result).Expression);
            Assert.AreEqual(((BinaryExpresssion)((UnaryExpression)result).Expression).Type, BinaryExpressionType.Pow);
        }

        [Test]
        public void ShouldCreateFunctionExpression()
        {
            //arrange
            var formula = "myFunc()";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(FunctionExpression), result);
        }

        [Test]
        public void ShouldCreateParametersForFunctionExpression()
        {
            //arrange
            var formula = "myFunc(5,4,3)";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(FunctionExpression), result);
            Assert.IsNotNull(((FunctionExpression)result).Expressions);
            Assert.AreEqual(((FunctionExpression)result).Expressions.Length, 3);
        }

        [Test]
        public void ShoulCreateParameterExpression()
        {
            //arrange
            var formula = "{5}";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(ParameterExpression), result);
            Assert.IsNotNull(result);
            Assert.AreEqual(((ParameterExpression)result).Name, "5");
            Assert.IsNull(((ParameterExpression)result).Argument);
        }

        [Test]
        public void ShoulCreateParameterExpressionWithArgument()
        {
            //arrange
            var formula = "{5, argument}";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(ParameterExpression), result);
            Assert.IsNotNull(result);
            Assert.AreEqual(((ParameterExpression)result).Name, "5");
            Assert.AreEqual(((ParameterExpression)result).Argument, "argument");
        }

        [Test]
        public void ShoulNotCreateParameterExpressionFromLiterals()
        {
            //arrange
            var formula = "{id}";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void ShouldCreateVariableExpression()
        {
            //arrange
            var formula = "var";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsAssignableFrom(typeof(VariableExpression), result);
            Assert.IsNotNull(result);
            Assert.AreEqual(((VariableExpression)result).Name, "var");
        }

        [Test]
        public void ShouldCreateVariableExpressionInsideOther()
        {
            //arrange
            var formula = "PCH(abc)";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom(typeof(FunctionExpression), result);            
            Assert.IsAssignableFrom(typeof(VariableExpression), ((FunctionExpression)result).Expressions.First());
            Assert.AreEqual(((VariableExpression)((FunctionExpression)result).Expressions.First()).Name, "abc");

        }

        [Test]
        public void ShouldCreateStringExpression()
        {
            //arrange
            var formula = "'2010-01-01;ANNL;1.51'";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom(typeof(ValueExpression), result);            
            Assert.IsNotNull(((ValueExpression)result).Text);
            Assert.AreEqual(((ValueExpression)result).Text, "2010-01-01;ANNL;1.51");
            Assert.AreEqual(((ValueExpression)result).Type, ValueType.Text);
        }

        [Test]
        public void ShouldCreateStringExpressionFromSingleQuoted()
        {
            //arrange
            var formula = @"'\'3.5'";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom(typeof(ValueExpression), result);
            Assert.IsNotNull(((ValueExpression)result).Text);
            Assert.AreEqual(((ValueExpression)result).Text, "'3.5");
            Assert.AreEqual(((ValueExpression)result).Type, ValueType.Text);
        }

        [Test]
        public void ShouldCreateStringExpressionFromEscapedSlash()
        {
            //arrange
            var formula = @"'\\3.5'";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom(typeof(ValueExpression), result);
            Assert.IsNotNull(((ValueExpression)result).Text);
            Assert.AreEqual(((ValueExpression)result).Text, @"\3.5");
            Assert.AreEqual(((ValueExpression)result).Type, ValueType.Text);
        }

        [Test]
        public void ShouldNotCreateStringExpressionFromSingleSlashed()
        {
            //arrange
            var formula = @"'\3.5'";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNull(result);            
        }

        [Test]
        public void ShouldBeAbleToCreateComplexFormula()
        {
            //arrange
            var formula = "PCH(-PCHYA(1*7)+(2*(4+5)^2)-9,{0},{1, ANNUAL}, '2010-01-01;ANN\\'L;1.51')";

            //act
            var result = Parse(formula);

            //assert
            Assert.IsNotNull(result);
        }

        protected static IFormulaExpression Parse(string formula)
        {
            return ExpressionFormulaParser.Parse(formula);
        }


    }
}
