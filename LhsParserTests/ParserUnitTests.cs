using LhsParser;
using System;
using System.Linq;
using Xunit;

namespace LhsParserTests
{
    public class ParserUnitTests
    {
        [Fact]
        public void Postfix_ForNullValue_ThrowsArgumentNullException()
        {
            string query = null;
            var parser = new PostfixParser();

            var ex = Record.Exception(() => parser.Parse(query));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [InlineData("", "")]

        [InlineData("Subject[eq]\"Ticket1\"", "Subject\"Ticket1\"[eq]")]
        [InlineData("Subject [eq] \"Ticket1\"", "Subject\"Ticket1\"[eq]")]
        [InlineData("(Subject [eq] \"Ticket1\")", "Subject\"Ticket1\"[eq]")]
        [InlineData("( Subject   [eq] \"Ticket1\" ) ", "Subject\"Ticket1\"[eq]")]
        [InlineData("\t(\tSubject\t \t[eq]\t\"Ticket1\")", "Subject\"Ticket1\"[eq]")]

        [InlineData("Subject [eq] \"Ticket1\" and CreateDate[lt]2018", "Subject\"Ticket1\"[eq]CreateDate2018[lt]and")]
        [InlineData("(Subject [eq] \"Ticket1\") and (CreateDate[lt]2018)", "Subject\"Ticket1\"[eq]CreateDate2018[lt]and")]
        [InlineData("(\tSubject [eq] \"Ticket1\" ) and \t( CreateDate[lt]2018\t)\t", "Subject\"Ticket1\"[eq]CreateDate2018[lt]and")]
        [InlineData("( ( Subject [eq] \"Ticket1\" ) ) and ( CreateDate[lt]2018)", "Subject\"Ticket1\"[eq]CreateDate2018[lt]and")]


        [InlineData("Subject [eq] \"Ticket1\" and CreateDate[lt]2018 or IsNew [eq] true", "Subject\"Ticket1\"[eq]CreateDate2018[lt]andIsNewtrue[eq]or")]
        [InlineData("(Subject [eq] \"Ticket1\") and (CreateDate[lt]2018) or IsNew [eq] true", "Subject\"Ticket1\"[eq]CreateDate2018[lt]andIsNewtrue[eq]or")]
        [InlineData("(\tSubject [eq] \"Ticket1\" ) and \t( CreateDate[lt]2018\t)\tor IsNew [eq] true", "Subject\"Ticket1\"[eq]CreateDate2018[lt]andIsNewtrue[eq]or")]
        [InlineData("( ( Subject [eq] \"Ticket1\" ) ) and ( CreateDate[lt]2018) or \t(IsNew \t[eq] true\t)", "Subject\"Ticket1\"[eq]CreateDate2018[lt]andIsNewtrue[eq]or")]

        [InlineData("Subject [eq] \"Ticket1\" or CreateDate[lt]2018 and IsNew [eq] true", "Subject\"Ticket1\"[eq]CreateDate2018[lt]IsNewtrue[eq]andor")]

        [InlineData("(Subject [eq] \"Ticket1\" or CreateDate[lt]2018) and IsNew [eq] true", "Subject\"Ticket1\"[eq]CreateDate2018[lt]orIsNewtrue[eq]and")]
        public void Postfix_ForGivenQuery_ReturnsExpectedValues(string query, string expectedPostfix)
        {
            var parser = new PostfixParser();
            var postfixedResult = parser.Parse(query);
            var actualPostfix = string.Join(string.Empty, postfixedResult.Select(x => x.Value).ToArray());

            Assert.Equal(expectedPostfix, actualPostfix);
        }
    }
}
