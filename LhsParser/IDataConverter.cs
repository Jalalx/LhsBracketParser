using System;

namespace LhsBracketParser
{
    public interface IDataConverter
    {
        object Convert(Type dataType, Token constant);

        object GetUpperRange(Type dataType, Token token);
        
        object GetLowerRange(Type dataType, Token token);
    }
}
