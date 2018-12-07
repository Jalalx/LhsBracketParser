using LhsParser;
using System.Linq;
using Xunit;

namespace LhsParserTests
{
    public class ParserUnitTests
    {
        [Theory]
        [InlineData("Subject[eq]\"Ticket1\"", "Subject\"Ticket1\"[eq]")]
        [InlineData("Subject [eq] \"Ticket1\"", "Subject\"Ticket1\"[eq]")]
        [InlineData("(Subject   [eq] \"Ticket1\")", "Subject\"Ticket1\"[eq]")]
        [InlineData("(Subject\t \t[eq]\t\"Ticket1\")", "Subject\"Ticket1\"[eq]")]
        public void Postfix_ForGivenQuery_ReturnsExpectedValues(string query, string expectedPostfix)
        {
            var parser = new DefaultParser();
            var postfixedResult = parser.Postfix(query);
            var actualPostfix = string.Join(string.Empty, postfixedResult.Select(x => x.Value).ToArray());

            Assert.Equal(expectedPostfix, actualPostfix);
        }
    }
}
