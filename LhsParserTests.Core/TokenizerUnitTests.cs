using LhsParser;
using System;
using System.IO;
using Xunit;

namespace LhsParserTests
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
    }
}
