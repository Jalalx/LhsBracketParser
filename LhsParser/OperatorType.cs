using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LhsBracketParser
{

    public enum OperatorType
    {
        Equal = TokenType.Equal,
        NotEqual = TokenType.NotEqual,
        GreaterThan = TokenType.GreaterThan,
        GreaterThanOrEqual = TokenType.GreaterThanOrEqual,
        LessThan = TokenType.LessThan,
        LessThanOrEqual = TokenType.LessThanOrEqual,
    }
}
