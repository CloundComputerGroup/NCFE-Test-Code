using System;
using System.Configuration.Internal;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Ncfe.CodeTest.contracts;

namespace Ncfe.CodeTest
{
    public class FailoverModeEvaluator : IFailoverModeEvaluator
    {
        private IFailoverRepository _failOverRepository;
        private IConfiguration _configuration;
        public FailoverModeEvaluator(IFailoverRepository failOverRepository, IConfiguration configuration) { 
            _failOverRepository = failOverRepository;
            _configuration = configuration;
        
        }
        public bool EligibleToUseFailover()
        {
            try
            {
                var failOverEntries = _failOverRepository.GetFailOverEntries();
                var numberOfFailures = failOverEntries.Count(x => x.DateTime > DateTime.Now.AddMinutes(-10));
                var isFailoverActive = _configuration["IsFailoverModeEnabled"]?.ToLower() == "true";
                return (numberOfFailures > 100 && isFailoverActive) ? true : false;

            }
            catch (Exception e)
            {
                //here we can handle exception
                return false;
            }
            
            
        }
    }
}
