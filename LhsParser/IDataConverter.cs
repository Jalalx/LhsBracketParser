using System;

namespace LhsBracketParser
{
    public interface IDataConverter
    {
        object Convert(Type dataType, Token constant);
    }
}
