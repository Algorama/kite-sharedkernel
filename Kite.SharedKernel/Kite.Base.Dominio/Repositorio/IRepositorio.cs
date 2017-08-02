using Kite.Base.Dominio.Entidades;

namespace Kite.Base.Dominio.Repositorio
{
    public interface IRepositorio<T> : IRepositorioConsulta<T> where T : EntidadeBase, IAggregateRoot
    {
        void Inclui(T entidade);
        void Altera(T entidade);
        void Salva(T entidade);
        void Exclui(T entidade);
        void ExecutaComando(string comandoHql);
    }
}