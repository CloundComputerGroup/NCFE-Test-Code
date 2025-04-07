using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ncfe.CodeTest.contracts;

namespace Ncfe.CodeTest
{
    public class LearnerService
    {
        private readonly IArchiveDataService _archiveDataService;
        private readonly ILearnerDataAccess _learnerDataAccess;
        private readonly IFailoverLearnerDataAccess _failoverLearnerDataAccess;
        private readonly IFailoverModeEvaluator _failoverModeEvaluator;

        public LearnerService(IArchiveDataService archiveDataService, ILearnerDataAccess learnerDataAccess,
            IFailoverLearnerDataAccess failoverLearnerDataAccess, IFailoverModeEvaluator failoverModeEvaluator)
        {
            _archiveDataService = archiveDataService;
            _learnerDataAccess = learnerDataAccess;
            _failoverLearnerDataAccess = failoverLearnerDataAccess;
            _failoverModeEvaluator = failoverModeEvaluator;

        }


        public Learner GetLearner(int learnerId, bool isLearnerArchived)
        {
            try
            {
                if (isLearnerArchived)
                {
                    return _archiveDataService.GetArchivedLearner(learnerId);
                }
                var response = _failoverModeEvaluator.EligibleToUseFailover() ? _failoverLearnerDataAccess.GetLearnerById(learnerId)
                    : _learnerDataAccess.LoadLearner(learnerId);
                return (response.IsArchived) ? _archiveDataService.GetArchivedLearner(learnerId) : response.Learner;

            }
            catch (Exception e)
            {
                //we can handle exception
                return new Learner();
            }
            
            
        }

    }
}
