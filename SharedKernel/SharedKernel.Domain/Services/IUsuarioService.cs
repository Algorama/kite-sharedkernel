using SharedKernel.Domain.Dtos;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Services
{
    public interface IUsuarioService : ICrudService<Usuario>
    {
        Token Login(string login, string senha);
        void TrocaSenha(string login, string senhaAntiga, string senhaNova);
    }
}