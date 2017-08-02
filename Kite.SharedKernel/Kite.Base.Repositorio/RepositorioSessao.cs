using NHibernate;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio
{
    public class RepositorioSessao : IRepositorioSessao
    {
        private readonly ISession _sessao;
        private ITransaction _transacao;

        public RepositorioSessao(ISession sessao)
        {
            _sessao = sessao;
        }

        public IRepositorioConsulta<T> GetRepositorioConsulta<T>() where T : EntidadeBase
        {
            return new RepositorioConsulta<T>(_sessao);
        }

        public IRepositorio<T> GetRepositorio<T>() where T : EntidadeBase, IAggregateRoot
        {
            return new Repositorio<T>(_sessao);
        }

        public void IniciaTransacao()
        {
            _transacao = _sessao.BeginTransaction();
        }

        public void ComitaTransacao()
        {
            _transacao.Commit();
            _transacao.Dispose();
        }

        public void RollBackTransacao()
        {
            _transacao.Rollback();
            _transacao.Dispose();
        }

        public void Dispose()
        {
            _sessao.Close();
        }
    }
}