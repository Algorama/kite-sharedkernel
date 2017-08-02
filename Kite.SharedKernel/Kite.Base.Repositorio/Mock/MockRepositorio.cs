using System.Linq;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio.Mock
{
    public class MockRepositorio<T> : MockRepositorioConsulta<T>, IRepositorio<T> where T : EntidadeBase, IAggregateRoot
    {
        public void Inclui(T entidade)
        {
            entidade.Id = GenerateId();
            Data.Add(entidade);
        }

        public void Altera(T entidade)
        {
            Exclui(entidade);
            Inclui(entidade);
        }

        public void Salva(T entidade)
        {
            Exclui(entidade);
            Inclui(entidade);
        }

        public void Exclui(T entidade)
        {
            Data.Remove(entidade);
        }

        public void ExecutaComando(string comandoHql)
        {
            
        }

        private static long GenerateId()
        {
            return Data.Count == 0 ? 1 : Data.Max(x => x.Id) + 1;
        }
    }
}