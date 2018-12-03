using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LhsParser
{

    public enum TokenType
    {
        Identifier = 1,
        Number,
        String,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        ParenthesesOpen,
        ParenthesesClose,
        EndOfLine
    }
}
