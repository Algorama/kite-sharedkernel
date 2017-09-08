using System;
using System.Web.Http;
using SharedKernel.Api.Security;
using SharedKernel.DependencyInjection;
using SharedKernel.Domain.Dtos;
using SharedKernel.Domain.Services;
using SharedKernel.Domain.Validation;

namespace SharedKernel.Api.Controllers
{
    /// <summary>
    /// Recurso para Autenticar Usuários da Aplicação
    /// </summary>	
    public class TokenController : ApiController
    {
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Recurso para Autenticar Usuários da Aplicação
        /// </summary>
        public TokenController()
	    {
	        _usuarioService = Kernel.Get<IUsuarioService>();
        }

        /// <summary>
        /// Efetua a Autenticação do Usuário
        /// </summary>
        /// <param name="login">Login e Senha do Usuário para a Autenticação</param>
        /// <returns>Token de Autenticação à ser utilizado nas requisições privadas</returns>
        public IHttpActionResult Post([FromBody]LoginRequest login)
        {
            try
            {
                var usuario = _usuarioService.Login(login);
                if (usuario == null) return Unauthorized();

                var token = usuario.GerarTokenString();
                return Ok(token);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}