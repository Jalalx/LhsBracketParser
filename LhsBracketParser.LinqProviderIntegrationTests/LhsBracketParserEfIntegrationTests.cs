using LhsBracketParser.LinqProvider;
using LhsBracketParser.LinqProviderIntegrationTests.Internals;
using System.Linq;
using Xunit;

namespace LhsBracketParser.LinqProviderIntegrationTests
{
    public class LhsBracketParserEfIntegrationTests : IClassFixture<PeopleDbContextFixture>
    {
        public LhsBracketParserEfIntegrationTests(PeopleDbContextFixture peopleDbContextFixture)
        {
            PeopleDbContextFixture = peopleDbContextFixture;
        }

        public PeopleDbContextFixture PeopleDbContextFixture { get; }
        

        [Fact]
        public void Evaluate_EqualOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[eq]45");

            var filteredData = PeopleDbContextFixture.DbContext.People.Where(predicate).ToArray();

            Assert.Single(filteredData);
            Assert.Equal(45, filteredData[0].Age);
            Assert.Equal("Mark Jackson", filteredData[0].FullName);
        }

        [Fact]
        public void Evaluate_LessThanOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[lt]45");

            var filteredData = PeopleDbContextFixture.DbContext.People.Where(predicate).OrderBy(x => x.Age).ToArray();

            Assert.Equal(2, filteredData.Length);

            Assert.Equal(30, filteredData[0].Age);
            Assert.Equal("John King", filteredData[0].FullName);

            Assert.Equal(33, filteredData[1].Age);
            Assert.Equal("Leo Messi", filteredData[1].FullName);
        }

        [Fact]
        public void Evaluate_LessThanOrEqualOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[lte]45");

            var filteredData = PeopleDbContextFixture.DbContext.People.Where(predicate).OrderBy(x => x.Age).ToArray();

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
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[gt]45");

            var filteredData = PeopleDbContextFixture.DbContext.People.Where(predicate).ToArray();

            Assert.Single(filteredData);

            Assert.Equal(50, filteredData[0].Age);
            Assert.Equal("Will Smith", filteredData[0].FullName);
        }

        [Fact]
        public void Evaluate_GreaterThanOrEqualOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("Age[gte]45");

            var filteredData = PeopleDbContextFixture.DbContext.People.Where(predicate).OrderByDescending(x => x.Age).ToArray();

            Assert.Equal(2, filteredData.Length);

            Assert.Equal(50, filteredData[0].Age);
            Assert.Equal("Will Smith", filteredData[0].FullName);

            Assert.Equal(45, filteredData[1].Age);
            Assert.Equal("Mark Jackson", filteredData[1].FullName);
        }

        [Fact]
        public void Evaluate_LikeOperatorOnInMemoryCollection_ReturnsExpectedResult()
        {
            var evaluator = new LinqExpressionEvaluator<Person>();

            var predicate = evaluator.Evaluate("FullName[like]\"Will\"");

            var filteredData = PeopleDbContextFixture.DbContext.People.Where(predicate).ToArray();

            Assert.Single(filteredData);

            Assert.Equal(50, filteredData[0].Age);
            Assert.Equal("Will Smith", filteredData[0].FullName);
        }
    }
}
