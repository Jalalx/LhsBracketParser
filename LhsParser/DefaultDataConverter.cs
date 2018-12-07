using System;

namespace LhsBracketParser
{

    public class DefaultDataConverter : IDataConverter
    {
        public object Convert(Type dataType, Token constant)
        {
            return constant.GetParsedValue(dataType);
        }
    }
}
