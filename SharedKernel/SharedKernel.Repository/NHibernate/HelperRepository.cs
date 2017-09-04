using SharedKernel.Domain.Repositories;

namespace SharedKernel.Repository.NHibernate
{
    public class HelperRepository : IHelperRepository
    {
        public ISessionRepository OpenSession()
        {
            return new SessionRepository(NHibernateHelper.OpenSession());
        }
    }
}