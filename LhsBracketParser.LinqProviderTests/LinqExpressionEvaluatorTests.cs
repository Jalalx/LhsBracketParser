using LhsBracketParser.LinqProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LhsBracketParser.LinqProviderTests
{
    public class LinqExpressionEvaluatorTests
    {
        private static Person[] GetSamplePeople()
        {
            return new[]
            {
                new Person { FullName = "Will Smith", Age = 50 },
                new Person { FullName = "John King", Age = 30 },
                new Person { FullName = "Leo Messi", Age = 33 },
                new Person { FullName = "Mark Jackson", Age = 45 }
            };
        }


        [Fact]
        public void Evaluate_EqualOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var data = GetSamplePeople();
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[eq]45");

            var filteredData = data.Where(predicate).ToArray();

            Assert.Single(filteredData);
            Assert.Equal(45, filteredData[0].Age);
            Assert.Equal("Mark Jackson", filteredData[0].FullName);
        }

        [Fact]
        public void Evaluate_LessThanOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var data = GetSamplePeople();
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[lt]45");

            var filteredData = data.Where(predicate).ToArray();

            Assert.Equal(2, filteredData.Length);

            Assert.Equal(30, filteredData[0].Age);
            Assert.Equal("John King", filteredData[0].FullName);

            Assert.Equal(33, filteredData[1].Age);
            Assert.Equal("Leo Messi", filteredData[1].FullName);
        }

        [Fact]
        public void Evaluate_LessThanOrEqualOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var data = GetSamplePeople();
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[lte]45");

            var filteredData = data.Where(predicate).ToArray();

            Assert.Equal(3, filteredData.Length);

            Assert.Equal(30, filteredData[0].Age);
            Assert.Equal("John King", filteredData[0].FullName);

            Assert.Equal(33, filteredData[1].Age);
            Assert.Equal("Leo Messi", filteredData[1].FullName);

            Assert.Equal(45, filteredData[2].Age);
            Assert.Equal("Mark Jackson", filteredData[2].FullName);
        }

        [Fact]
        public void Evaluate_GreaterThanOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var data = GetSamplePeople();
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[gt]45");

            var filteredData = data.Where(predicate).ToArray();

            Assert.Single(filteredData);

            Assert.Equal(50, filteredData[0].Age);
            Assert.Equal("Will Smith", filteredData[0].FullName);
        }

        [Fact]
        public void Evaluate_GreaterThanOrEqualOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var data = GetSamplePeople();
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[gte]45");

            var filteredData = data.Where(predicate).ToArray();

            Assert.Equal(2, filteredData.Length);

            Assert.Equal(50, filteredData[0].Age);
            Assert.Equal("Will Smith", filteredData[0].FullName);

            Assert.Equal(45, filteredData[1].Age);
            Assert.Equal("Mark Jackson", filteredData[1].FullName);
        }

        [Fact]
        public void Evaluate_LikeOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var data = GetSamplePeople();
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("FullName[like]\"Will\"");

            var filteredData = data.Where(predicate).ToArray();

            Assert.Single(filteredData);

            Assert.Equal(50, filteredData[0].Age);
            Assert.Equal("Will Smith", filteredData[0].FullName);
        }
    }

    public class Person
    {
        public string FullName { get; set; }

        public int Age { get; set; }
    }
}
