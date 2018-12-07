using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LhsBracketParser
{

    public interface IParser
    {
        IEnumerable<Token> Parse(string query);
    }
}
