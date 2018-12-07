using System;

namespace LhsParser
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
