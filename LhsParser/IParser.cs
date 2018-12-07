using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LhsParser
{

    public interface IParser
    {
        IEnumerable<Token> Parse(string query);
    }
}
