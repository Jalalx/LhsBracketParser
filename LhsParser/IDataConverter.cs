using System;

namespace LhsParser
{
    public interface IDataConverter
    {
        object Convert(Type dataType, Token constant);
    }
}
