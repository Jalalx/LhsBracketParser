using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LhsParser
{

    public class InvalidSytaxException : Exception
    {
        public InvalidSytaxException()
        {
        }

        public int Index { get; set; }

        public InvalidSytaxException(string message, int index) : base(message)
        {
            Index = index;
        }
    }
}
