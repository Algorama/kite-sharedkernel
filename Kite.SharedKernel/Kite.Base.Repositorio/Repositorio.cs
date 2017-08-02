using NHibernate;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio
{
    public class Repositorio<T> : RepositorioConsulta<T>, IRepositorio<T> where T : EntidadeBase, IAggregateRoot
    {
        public Repositorio(ISession sessao) : base(sessao)
        {
        }

        public void Inclui(T entidade)
        {
            Sessao.Save(entidade);
        }

        public void Altera(T entidade)
        {
            Sessao.Update(entidade);
        }

        public void Salva(T entidade)
        {
            Sessao.SaveOrUpdate(entidade);
        }

        public void Exclui(T entidade)
        {
            Sessao.Delete(entidade);
        }

        public void ExecutaComando(string comandoHql)
        {
            Sessao.CreateQuery(comandoHql).ExecuteUpdate();
        }
    }
}