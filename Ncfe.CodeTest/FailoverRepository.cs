using System.Collections.Generic;
using Ncfe.CodeTest.contracts;

namespace Ncfe.CodeTest
{
    public class FailoverRepository : IFailoverRepository
    {
        public IEnumerable<FailoverEntry> GetFailOverEntries()
        {
            // return all from fail entries from database
            return new List<FailoverEntry>();
        }
    }
}
