using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncfe.CodeTest.contracts
{
    public interface IFailoverModeEvaluator
    {
        bool EligibleToUseFailover();
    }
}
