using System;

namespace LhsBracketParser
{

    public static class TokenExtensions
    {
        public static object GetParsedValue(this Token token, Type dataType)
        {
            var value = token.Value;
            if (dataType == typeof(bool))
            {
                return bool.Parse(value);
            }
            else if (dataType == typeof(byte))
            {
                return byte.Parse(value);
            }
            else if (dataType == typeof(short))
            {
                return short.Parse(value);
            }
            else if (dataType == typeof(int))
            {
                return int.Parse(value);
            }
            else if (dataType == typeof(long))
            {
                return long.Parse(value);
            }
            else if (dataType == typeof(float))
            {
                return float.Parse(value);
            }
            else if (dataType == typeof(decimal))
            {
                return decimal.Parse(value);
            }
            else if (dataType == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }
            else if (dataType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value);
            }
            else if (dataType == typeof(Guid))
            {
                return Guid.Parse(value);
            }
            else if (dataType == typeof(string))
            {
                return value.Trim('"');
            }
            else
            {
                throw new NotSupportedException($"DataType '{dataType}' is not supported for value '{value}'");
            }
        }

        public static bool IsOperand(this Token token)
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

        public static bool IsParenthesis(this Token token)
        {
            return token.Type == TokenType.ParenthesesOpen || token.Type == TokenType.ParenthesesClose;
        }

        public static bool IsOperator(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Equal:
                case TokenType.NotEqual:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.LessThan:
                case TokenType.LessThanOrEqual:
                case TokenType.Range:
                case TokenType.Like:
                case TokenType.And:
                case TokenType.Or:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLogicalOperator(this Token token)
        {
            return token.Type == TokenType.And || token.Type == TokenType.Or;
        }

        public static bool IsComparerOperator(this Token token)
        {
            return token.Type == TokenType.Equal || token.Type == TokenType.NotEqual ||
                token.Type == TokenType.LessThan || token.Type == TokenType.LessThanOrEqual ||
                token.Type == TokenType.GreaterThan || token.Type == TokenType.GreaterThanOrEqual ||
                token.Type == TokenType.Range || token.Type == TokenType.Like;
        }
    }
}
