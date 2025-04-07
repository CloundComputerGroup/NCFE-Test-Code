using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Ncfe.CodeTest.contracts;

namespace Ncfe.CodeTest.Test
{
    public class LearnerServiceTest
    {
        private readonly Mock<IFailoverModeEvaluator> _failoverModeEvaluatorMock;
        private readonly Mock<IArchiveDataService> _archiveDataServiceMock;
        private readonly Mock<ILearnerDataAccess> _learnerDataAccessMock;
        private readonly Mock<IFailoverLearnerDataAccess> _failoverLearnerDataAccessMock;

        private readonly LearnerService _learnerService;

        public LearnerServiceTest()
        {
            _failoverModeEvaluatorMock = new Mock<IFailoverModeEvaluator>();
            _archiveDataServiceMock = new Mock<IArchiveDataService>();
            _learnerDataAccessMock = new Mock<ILearnerDataAccess>();
            _failoverLearnerDataAccessMock = new Mock<IFailoverLearnerDataAccess>();
            _learnerService = new LearnerService(_archiveDataServiceMock.Object, _learnerDataAccessMock.Object,
                 _failoverLearnerDataAccessMock.Object,_failoverModeEvaluatorMock.Object
                );
        }

        [Fact]
        public void FetchLearner_WhenThereNoArchived_And_FailOverDisabled()
        {
            // Arrange data
            int learnerId = 10;
            bool isLearnerArchived = false;
            var learnerModel = new Learner() { Id = learnerId};
            var responseModel = new LearnerResponse { Learner = learnerModel, IsArchived = false };
            _failoverModeEvaluatorMock.Setup(x => x.EligibleToUseFailover()).Returns(false);
            _learnerDataAccessMock.Setup(d => d.LoadLearner(learnerId)).Returns(responseModel);
            
            //Act
            var result = _learnerService.GetLearner(learnerId, isLearnerArchived);

            //Assert
            Assert.Equal(learnerModel,result);

        }

        [Fact]
        public void FetchLearner_LoadsArchivedLearner_WhenResponseIsArchived()
        {
            // Arrange data
            int learnerId = 10;
            bool isLearnerArchived = false;
            var responseModel = new LearnerResponse { IsArchived = true };
            var archivedLearner = new Learner { Id = learnerId };

            _failoverModeEvaluatorMock.Setup(e => e.EligibleToUseFailover()).Returns(false);
            _learnerDataAccessMock.Setup(d => d.LoadLearner(learnerId)).Returns(responseModel);
            _archiveDataServiceMock.Setup(a => a.GetArchivedLearner(learnerId)).Returns(archivedLearner);

            // Act
            var result = _learnerService.GetLearner(learnerId, isLearnerArchived);

            // Assert
            Assert.Equal(archivedLearner, result);

        }

        [Fact]
        public void FetchLearner_LoadsArchivedLearner_When_IsLearnerArchived_Is_True()
        {
            // Arrange data
            int learnerId = 10;
            bool isLearnerArchived = true;
            var archivedLearner = new Learner { Id = learnerId };

            _archiveDataServiceMock.Setup(a => a.GetArchivedLearner(learnerId)).Returns(archivedLearner);
           
            // Act
            var result = _learnerService.GetLearner(learnerId, isLearnerArchived);

            // Assert
            Assert.Equal(archivedLearner, result);

        }

        [Fact]
        public void FetchLearner_WhenFailoverModeEvaluatorReturn_True()
        {
            // Arrange data
            int learnerId = 10;
            bool isLearnerArchived = false;
            var archivedLearner = new Learner { Id = learnerId };

            _failoverModeEvaluatorMock.Setup(e => e.EligibleToUseFailover()).Returns(true);

            _failoverLearnerDataAccessMock.Setup(f => f.GetLearnerById(learnerId))
            .Returns(new LearnerResponse { Learner = archivedLearner, IsArchived = false });
            // Act
            var result = _learnerService.GetLearner(learnerId, isLearnerArchived);

            // Assert
            Assert.Equal(archivedLearner, result);

        }

    }
}
