using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using NHibernate;

namespace SharedKernel.Repository.NHibernate
{
    public class Repository<T> : QueryRepository<T>, IRepository<T> where T : EntityBase, IAggregateRoot
    {
        public Repository(ISession session) : base(session)
        {
        }

        public void Insert(T entity)
        {
            Session.Save(entity);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public void Save(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            Session.Delete(entity);
        }

        public void RunCommand(string hqlCommand)
        {
            Session.CreateQuery(hqlCommand).ExecuteUpdate();
        }
    }
}