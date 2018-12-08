using LhsBracketParser;
using System;
using System.IO;
using Xunit;

namespace LhsBracketParserTests
{
    public class TokenizerUnitTests
    {
        [Theory]
        [InlineData("123", '1')]
        [InlineData("createDate", 'c')]
        public void Constructor_CreatedFromValidTexts_ReturnsExpectedChar(string text, char expectedCurrentChar)
        {
            using (var reader = new StringReader(text))
            using (var tokenizer = new Tokenizer(reader))
            {
                Assert.Equal(expectedCurrentChar, tokenizer.CurrentChar);
                Assert.Equal(0, tokenizer.CurrentIndex);
            }
        }

        [Theory]
        [InlineData("isAdmin[eq]true", "isAdmin", "[eq]", TokenType.Equal, "true", TokenType.Boolean)]
        [InlineData("isAdmin[eq]false", "isAdmin", "[eq]", TokenType.Equal, "false", TokenType.Boolean)]
        [InlineData("count[eq]18", "count", "[eq]", TokenType.Equal, "18", TokenType.Number)]
        [InlineData("count[range]18", "count", "[range]", TokenType.Range, "18", TokenType.Number)]
        [InlineData("price[eq]2500.00", "price", "[eq]", TokenType.Equal, "2500.00", TokenType.Number)]
        [InlineData("createDate[eq]\"yr,-1\"", "createDate", "[eq]", TokenType.Equal, "\"yr,-1\"", TokenType.String)]
        [InlineData("createDate[eq]2018", "createDate", "[eq]", TokenType.Equal, "2018", TokenType.Number)]
        [InlineData("createDate[eq]2018-02-15", "createDate", "[eq]", TokenType.Equal, "2018-02-15", TokenType.Date)]
        [InlineData("createDate[ne]2018-02-15", "createDate", "[ne]", TokenType.NotEqual, "2018-02-15", TokenType.Date)]
        [InlineData("createDate[lt]2018-02-15", "createDate", "[lt]", TokenType.LessThan, "2018-02-15", TokenType.Date)]
        [InlineData("createDate[lte]2018-02-15", "createDate", "[lte]", TokenType.LessThanOrEqual, "2018-02-15", TokenType.Date)]
        [InlineData("createDate[gt]2018-02-15", "createDate", "[gt]", TokenType.GreaterThan, "2018-02-15", TokenType.Date)]
        [InlineData("createDate[gte]2018-02-15", "createDate", "[gte]", TokenType.GreaterThanOrEqual, "2018-02-15", TokenType.Date)]
        [InlineData("createDate[eq]2018/02/15", "createDate", "[eq]", TokenType.Equal, "2018/02/15", TokenType.Date)]
        [InlineData("createDate[ne]2018/02/15", "createDate", "[ne]", TokenType.NotEqual, "2018/02/15", TokenType.Date)]
        [InlineData("createDate[lt]2018/02/15", "createDate", "[lt]", TokenType.LessThan, "2018/02/15", TokenType.Date)]
        [InlineData("createDate[lte]2018/02/15", "createDate", "[lte]", TokenType.LessThanOrEqual, "2018/02/15", TokenType.Date)]
        [InlineData("createDate[gt]2018/02/15", "createDate", "[gt]", TokenType.GreaterThan, "2018/02/15", TokenType.Date)]
        [InlineData("createDate[gte]2018/02/15", "createDate", "[gte]", TokenType.GreaterThanOrEqual, "2018/02/15", TokenType.Date)]
        [InlineData("createDate[eq]\"salam\"", "createDate", "[eq]", TokenType.Equal, "\"salam\"", TokenType.String)]
        [InlineData("createDate[eq]\"2018\"", "createDate", "[eq]", TokenType.Equal, "\"2018\"", TokenType.String)]
        [InlineData("createDate[eq]\"2018-01-01\"", "createDate", "[eq]", TokenType.Equal, "\"2018-01-01\"", TokenType.String)]
        [InlineData("createDate[eq]\"2018/01/01\"", "createDate", "[eq]", TokenType.Equal, "\"2018/01/01\"", TokenType.String)]
        public void NextToken_ForGivenValidBasicText_ReturnsExpectedTokens(string givenText, string expectedIdentifier, string expectedOpearator,
                        TokenType expectedOperatorType, string expectedValue, TokenType expectedValueType)
        {
            using (var reader = new StringReader(givenText))
            using (var tokenizer = new Tokenizer(reader))
            {
                var token1 = tokenizer.NextToken();
                var token2 = tokenizer.NextToken();
                var token3 = tokenizer.NextToken();

                Assert.Equal(expectedIdentifier, token1.Value);
                Assert.Equal(TokenType.Identifier, token1.Type);

                Assert.Equal(expectedOpearator, token2.Value);
                Assert.Equal(expectedOperatorType, token2.Type);

                Assert.Equal(expectedValue, token3.Value);
                Assert.Equal(expectedValueType, token3.Type);

                Assert.False(tokenizer.CanRead());
            }
        }

        [Fact]
        public void NextToken_ForGivenLogicalTextWithAnd_ReturnsExpectedTokens()
        {
            string text = "Count[eq]1500 and createDate[eq]2018-05-26";
            using (var reader = new StringReader(text))
            using (var tokenizer = new Tokenizer(reader))
            {
                var token1 = tokenizer.NextToken();
                var token2 = tokenizer.NextToken();
                var token3 = tokenizer.NextToken();

                var token4 = tokenizer.NextToken();

                var token5 = tokenizer.NextToken();
                var token6 = tokenizer.NextToken();
                var token7 = tokenizer.NextToken();

                Assert.Equal("Count", token1.Value);
                Assert.Equal(TokenType.Identifier, token1.Type);
                Assert.Equal("[eq]", token2.Value);
                Assert.Equal(TokenType.Equal, token2.Type);
                Assert.Equal("1500", token3.Value);
                Assert.Equal(TokenType.Number, token3.Type);

                Assert.Equal("and", token4.Value);
                Assert.Equal(TokenType.And, token4.Type);

                Assert.Equal("createDate", token5.Value);
                Assert.Equal(TokenType.Identifier, token5.Type);
                Assert.Equal("[eq]", token6.Value);
                Assert.Equal(TokenType.Equal, token6.Type);
                Assert.Equal("2018-05-26", token7.Value);
                Assert.Equal(TokenType.Date, token7.Type);
            }
        }

        [Fact]
        public void NextToken_ForGivenLogicalTextWithAndWithParentheses_ReturnsExpectedTokens()
        {
            string text = "(Count[eq]1500 and createDate[eq]2018-05-26)";
            using (var reader = new StringReader(text))
            using (var tokenizer = new Tokenizer(reader))
            {
                var token0 = tokenizer.NextToken();

                var token1 = tokenizer.NextToken();
                var token2 = tokenizer.NextToken();
                var token3 = tokenizer.NextToken();

                var token4 = tokenizer.NextToken();

                var token5 = tokenizer.NextToken();
                var token6 = tokenizer.NextToken();
                var token7 = tokenizer.NextToken();

                var token8 = tokenizer.NextToken();

                Assert.Equal("(", token0.Value);
                Assert.Equal(TokenType.ParenthesesOpen, token0.Type);

                Assert.Equal("Count", token1.Value);
                Assert.Equal(TokenType.Identifier, token1.Type);
                Assert.Equal("[eq]", token2.Value);
                Assert.Equal(TokenType.Equal, token2.Type);
                Assert.Equal("1500", token3.Value);
                Assert.Equal(TokenType.Number, token3.Type);

                Assert.Equal("and", token4.Value);
                Assert.Equal(TokenType.And, token4.Type);

                Assert.Equal("createDate", token5.Value);
                Assert.Equal(TokenType.Identifier, token5.Type);
                Assert.Equal("[eq]", token6.Value);
                Assert.Equal(TokenType.Equal, token6.Type);
                Assert.Equal("2018-05-26", token7.Value);
                Assert.Equal(TokenType.Date, token7.Type);


                Assert.Equal(")", token8.Value);
                Assert.Equal(TokenType.ParenthesesClose, token8.Type);
            }
        }
    }
}
