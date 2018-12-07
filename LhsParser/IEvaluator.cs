using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LhsBracketParser
{

    public interface IEvaluator
    {
        object Evaluate(string query);
    }
    
}
