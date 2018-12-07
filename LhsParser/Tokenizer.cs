using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LhsParser
{

    public class Tokenizer : IDisposable, IEnumerator<Token>
    {
        private readonly StringReader _reader;

        public Tokenizer(StringReader reader)
        {
            _reader = reader;
            NextChar();
        }

        public Token CurrentToken { get; set; }

        public char CurrentChar { get; set; }

        public int CurrentIndex { get; set; } = -1;

        public Token Current => CurrentToken;

        object IEnumerator.Current => CurrentToken;

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
                ReadNumberOrDate();
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

            return CurrentToken;
        }

        public IEnumerable<Token> ReadAll()
        {
            while (CanRead())
            {
                yield return NextToken();
            }
        }

        private void ReadOperator()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();
            builder.Append(CurrentChar);

            while (CanRead())
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

                case "[ne]":
                    CurrentToken = new Token(tokenStr, TokenType.NotEqual);
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
                    throw new InvalidSytaxException($"Invalid char '{CurrentChar}' at index {CurrentIndex}");
            }

            NextChar();
        }

        private void ReadIdentifier()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();

            while (CanRead())
            {
                builder.Append(CurrentChar);
                NextChar();

                if (!char.IsLetterOrDigit(CurrentChar))
                {
                    break;
                }
            }

            var tokenStr = builder.ToString().ToLower();
            switch (tokenStr)
            {
                case "and":
                    CurrentToken = new Token(builder.ToString(), TokenType.And);
                    break;

                case "or":
                    CurrentToken = new Token(builder.ToString(), TokenType.Or);
                    break;

                case "true":
                case "false":
                    CurrentToken = new Token(builder.ToString(), TokenType.Boolean);
                    break;

                default:
                    CurrentToken = new Token(builder.ToString(), TokenType.Identifier);
                    break;
            }
        }

        private void ReadNumberOrDate()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();

            while (CanRead())
            {
                builder.Append(CurrentChar);
                NextChar();

                if (!char.IsDigit(CurrentChar) && CurrentChar != '.' && CurrentChar != '/' && CurrentChar != '-')
                {
                    break;
                }
            }

            var tokenStr = builder.ToString();
            if (Regex.IsMatch(tokenStr, "(\\d{2,4})[/\\-](\\d{1,2})[/\\-](\\d{1,2})"))
            {
                CurrentToken = new Token(tokenStr, TokenType.Date);
            }
            else if (Regex.IsMatch(tokenStr, "\\d+([\\.]\\d*)?"))
            {
                CurrentToken = new Token(tokenStr, TokenType.Number);
            }
        }

        private void ReadString()
        {
            var startIndex = CurrentIndex;
            var builder = new StringBuilder();
            builder.Append(CurrentChar);

            while (CanRead())
            {
                NextChar();
                builder.Append(CurrentChar);

                if (CurrentChar == '"')
                {
                    break;
                }
            }

            var tokenStr = builder.ToString();
            CurrentToken = new Token(tokenStr, TokenType.String);
            NextChar();
        }


        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool MoveNext()
        {
            if (CanRead())
            {
                NextToken();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException("LHS Tokenizer doesn't support this operation.");
        }
    }
}
