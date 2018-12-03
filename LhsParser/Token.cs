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
}
