using SharedKernel.Domain.Repositories;

namespace SharedKernel.Repository.Mock
{
    public class MockHelperRepository : IHelperRepository
    {
        public ISessionRepository OpenSession()
        {
            return new MockSessionRepository();
        }
    }
}