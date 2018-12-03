using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LhsParser
{

    public class Tokenizer : IDisposable
    {
        private readonly StringReader _reader;

        public Tokenizer(StringReader reader)
        {
            _reader = reader;
            NextChar();
            NextToken();
        }

        public Token CurrentToken { get; set; }

        public char CurrentChar { get; set; }

        public int CurrentIndex { get; set; } = -1;

        private void NextChar()
        {
            int ch = _reader.Read();
            CurrentChar = ch < 0 ? '\0' : (char)ch;
            CurrentIndex++;
        }

        public bool CanRead()
        {
            return CurrentChar != 0;
        }

        public Token NextToken()
        {
            while (char.IsWhiteSpace(CurrentChar) && CanRead())
            {
                NextChar();
            }

            if (!CanRead())
            {
                return null;
            }

            if (CurrentChar == '"')
            {
                ReadString();
            }
            else if (char.IsDigit(CurrentChar))
            {
                ReadDigit();
            }
            else if (char.IsLetter(CurrentChar))
            {
                ReadIdentifier();
            }
            else if (CurrentChar == '(')
            {
                CurrentToken = new Token("(", TokenType.ParenthesesOpen);
                NextChar();
            }
            else if (CurrentChar == ')')
            {
                CurrentToken = new Token(")", TokenType.ParenthesesClose);
                NextChar();
            }
            else if (CurrentChar == '[')
            {
                ReadOperator();
            }
            else if (char.IsLetterOrDigit(CurrentChar) || new[] { '-', ',', '@' }.Contains(CurrentChar))
            {
                var startIndex = CurrentIndex;
                var builder = new StringBuilder();

                while (!char.IsWhiteSpace(CurrentChar) && !CanRead() && CurrentChar != '[' && CurrentChar != '(' && CurrentChar != ')')
                {
                    builder.Append(CurrentChar);
                    NextChar();
                }

                var tokenStr = builder.ToString();
                CurrentToken = new Token(tokenStr, TokenType.Identifier);
            }

            return CurrentToken;
        }

        private void ReadOperator()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();
            builder.Append(CurrentChar);

            while (!CanRead())
            {
                NextChar();
                builder.Append(CurrentChar);

                if (CurrentChar == ']')
                {
                    break;
                }
            }

            var tokenStr = builder.ToString().ToLower();
            switch (tokenStr)
            {
                case "[eq]":
                    CurrentToken = new Token(tokenStr, TokenType.Equal);
                    break;

                case "[lt]":
                    CurrentToken = new Token(tokenStr, TokenType.LessThan);
                    break;

                case "[lte]":
                    CurrentToken = new Token(tokenStr, TokenType.LessThanOrEqual);
                    break;

                case "[gt]":
                    CurrentToken = new Token(tokenStr, TokenType.GreaterThan);
                    break;

                case "[gte]":
                    CurrentToken = new Token(tokenStr, TokenType.GreaterThanOrEqual);
                    break;

                default:
                    throw new InvalidSytaxException($"Invalid char '{CurrentChar}' at index {CurrentIndex}", CurrentIndex);
            }

            NextChar();
        }

        private void ReadIdentifier()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();

            while (!CanRead())
            {
                builder.Append(CurrentChar);
                NextChar();

                if (char.IsSymbol(CurrentChar) || char.IsWhiteSpace(CurrentChar))
                {
                    break;
                }
            }

            var tokenStr = builder.ToString();
            CurrentToken = new Token(tokenStr, TokenType.Identifier);
            NextChar();
        }

        private void ReadDigit()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();

            while (!CanRead())
            {
                builder.Append(CurrentChar);
                NextChar();

                if (!char.IsDigit(CurrentChar))
                {
                    break;
                }
            }

            var tokenStr = builder.ToString();
            CurrentToken = new Token(tokenStr, TokenType.Number);
            NextChar();
        }

        private void ReadString()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();

            while (!CanRead())
            {
                builder.Append(CurrentChar);
                NextChar();

                if (CurrentChar == '"')
                {
                    break;
                }
            }

            var tokenStr = builder.ToString();
            CurrentToken = new Token(tokenStr, TokenType.Identifier);
            NextChar();
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
