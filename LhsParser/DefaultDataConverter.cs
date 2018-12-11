using System;

namespace LhsBracketParser
{

    public class DefaultDataConverter : IDataConverter
    {
        public object Convert(Type dataType, Token constant)
        {
            return constant.GetParsedValue(dataType);
        }

        public object GetLowerRange(Type dataType, Token token)
        {
            throw new NotImplementedException();
        }

        public object GetUpperRange(Type dataType, Token token)
        {
            throw new NotImplementedException();
        }
    }
}
