using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LhsParser
{
    [DebuggerDisplay("Value={Value}, Type={Type}")]
    public class Token
    {
        public Token()
        {
        }

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }

        public TokenType Type { get; set; }
    }

    internal static class TokenExtensions
    {
        internal static bool IsOperand(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Identifier:
                case TokenType.Number:
                case TokenType.Date:
                case TokenType.String:
                case TokenType.Boolean:
                    return true;

                default:
                    return false;
            }
        }

        internal static bool IsParenthesis(this Token token)
        {
            return token.Type == TokenType.ParenthesesOpen || token.Type == TokenType.ParenthesesClose;
        }

        internal static bool IsOperator(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Equal:
                case TokenType.NotEqual:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.LessThan:
                case TokenType.LessThanOrEqual:
                case TokenType.And:
                case TokenType.Or:
                    return true;
                default:
                    return false;
            }
        }
    }
}
