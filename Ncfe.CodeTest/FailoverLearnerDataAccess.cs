using Ncfe.CodeTest.contracts;

namespace Ncfe.CodeTest
{
    public class FailoverLearnerDataAccess : IFailoverLearnerDataAccess
    {
        public LearnerResponse GetLearnerById(int id)
        {
            // retrieve learner from database
            return new LearnerResponse();
        }
    }
}
