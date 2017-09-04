using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using NHibernate;

namespace SharedKernel.Repository.NHibernate
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public SessionRepository(ISession session)
        {
            _session = session;
        }

        public IQueryRepository<T> QueryRepository<T>() where T : EntityBase
        {
            return new QueryRepository<T>(_session);
        }

        public IRepository<T> Repository<T>() where T : EntityBase, IAggregateRoot
        {
            return new Repository<T>(_session);
        }

        public void StartTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }

        public void RollBackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public void Dispose()
        {
            _session.Close();
        }
    }
}