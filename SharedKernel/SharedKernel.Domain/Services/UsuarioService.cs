using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SharedKernel.Domain.Dtos;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Helpers;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Validation;

namespace SharedKernel.Domain.Services
{
    public class UsuarioService : CrudService<Usuario>
    {
        public UsuarioService(IHelperRepository helper, UsuarioValidator validator) : base(helper, validator)
        {
            validator.Service = this;
        }

        public override void Insert(Usuario entidade, string user = "sistema")
        {
            entidade.Senha = CryptoTools.ComputeHashMd5(entidade.Senha);
            base.Insert(entidade, user);
        }

        public override void Update(Usuario entidade, string user = "sistema")
        {
            entidade.Senha = Get(entidade.Id).Senha;
            base.Update(entidade, user);
        }

        public Token Login(string login, string senha)
        {
            senha = CryptoTools.ComputeHashMd5(senha);
            var usuario = GetAll(x => 
                x.Login.ToUpper() == login.ToUpper() && 
                x.Senha == senha).FirstOrDefault();

            if (usuario == null) return null;

            var token = new Token
            {
                UsuarioId = usuario.Id,
                UsuarioNome = usuario.Nome,
                Login = usuario.Login,
                DataExpiracao = DateTime.Now
            };

            return token;
        }

        public void TrocaSenha(string login, string senhaAntiga, string senhaNova)
        {
            senhaAntiga = CryptoTools.ComputeHashMd5(senhaAntiga);
            var usuario = GetAll(x =>
                x.Login.ToUpper() == login.ToUpper() &&
                x.Senha == senhaAntiga).FirstOrDefault();

            if (usuario == null)
                throw new ValidationException("Senha antiga não confere");

            usuario.Senha = CryptoTools.ComputeHashMd5(senhaNova);
            base.Update(usuario);
        }
    }
}