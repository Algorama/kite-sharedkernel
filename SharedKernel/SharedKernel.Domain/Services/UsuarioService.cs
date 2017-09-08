﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SharedKernel.Domain.Dtos;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Helpers;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Validation;
using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Services
{
    public class UsuarioService : CrudService<Usuario>, IUsuarioService
    {
        public UsuarioService(IHelperRepository helper, UsuarioValidator validator) : base(helper, validator)
        {
            validator.Service = this;
        }

        public override Usuario Get(long id)
        {
            var usuario = base.Get(id);
            usuario.Senha = null;
            usuario.Foto = null;
            return usuario;
        }

        public override PageResult<Usuario> GetPaged(int page, PageSize size)
        {
            var usuarios = base.GetPaged(page, size);
            foreach (var usuario in usuarios.Data)
            {
                usuario.Senha = null;
                usuario.Foto = null;
            }
            return usuarios;
        }

        public override void Insert(Usuario entidade, string user = "sistema")
        {
            entidade.Senha = CryptoTools.ComputeHashMd5(entidade.Senha);
            base.Insert(entidade, user);
        }

        public override void Update(Usuario entidade, string user = "sistema")
        {
            var usuarioOld = Get(entidade.Id);
            entidade.Senha = usuarioOld.Senha;
            entidade.Foto = usuarioOld.Foto;
            base.Update(entidade, user);
        }

        public Token Login(LoginRequest loginRequest)
        {
            var senha = CryptoTools.ComputeHashMd5(loginRequest.Password);
            var usuario = GetAll(x => 
                x.Login.ToUpper() == loginRequest.Login.ToUpper() && 
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

        public void TrocaSenha(ChangePasswordRequest changePasswordRequest)
        {
            var senhaAntiga = CryptoTools.ComputeHashMd5(changePasswordRequest.OldPassword);
            var usuario = GetAll(x =>
                x.Login.ToUpper() == changePasswordRequest.Login.ToUpper() &&
                x.Senha == senhaAntiga).FirstOrDefault();

            if (usuario == null)
                throw new ValidationException("Senha antiga não confere");

            usuario.Senha = CryptoTools.ComputeHashMd5(changePasswordRequest.NewPassword);
            base.Update(usuario);
        }

        public string GetTema(long usuarioId)
        {
            var usuario = Get(usuarioId);
            if(usuario == null)
                throw new ValidationException("Usuário Inválido!");

            return usuario.Tema;
        }

        public void ChangeTema(long usuarioId, string newTema)
        {
            if(string.IsNullOrEmpty(newTema))
                throw new ValidationException("Tema Inválido!");

            var usuario = Get(usuarioId);
            if (usuario == null)
                throw new ValidationException("Usuário Inválido!");

            usuario.Tema = newTema;

            Update(usuario, "ChangeTema");
        }
    }
}