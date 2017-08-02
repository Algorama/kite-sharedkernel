using Kite.Base.Dominio.Repositorio;

namespace Kite.Base.Repositorio.Mock
{
    public class MockRepositorioHelper : IRepositorioHelper
    {
        public IRepositorioSessao AbrirSessao()
        {
            return new MockRepositorioSessao();
        }
    }
}