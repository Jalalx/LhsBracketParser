using System;

namespace LhsBracketParser
{

    public class InvalidSytaxException : Exception
    {
        public InvalidSytaxException()
        {
        }

        public InvalidSytaxException(string message) : base(message)
        {
        }
    }
}
