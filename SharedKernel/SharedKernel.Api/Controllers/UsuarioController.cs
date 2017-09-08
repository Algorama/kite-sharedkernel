using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using SharedKernel.Api.Filters;
using SharedKernel.Api.Security;
using SharedKernel.DependencyInjection;
using SharedKernel.Domain.Dtos;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Services;
using SharedKernel.Domain.Validation;

namespace SharedKernel.Api.Controllers
{
    /// <summary>
    /// Recurso para Gerenciar os Usuários da Aplicação
    /// </summary>	
	[UserAuthorization]
    public class UsuarioController : CrudController<Usuario>
    {
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Recurso para Gerenciar os Usuários da Aplicação
        /// </summary>
        public UsuarioController()
	    {
	        _usuarioService = Kernel.Get<IUsuarioService>();
	        Service = _usuarioService;
	    }

        /// <summary>
        /// Opção para Troca de Senha
        /// </summary>
        /// <param name="changePasswordRequest">Informações para a troca de senha</param>
        /// <returns>Ok</returns>
        [Route("api/trocasenha")]
        public IHttpActionResult PostTrocaSenha([FromBody]ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var token = ActionContext.RecuperarToken();
                changePasswordRequest.Login = token.Login;
                _usuarioService.TrocaSenha(changePasswordRequest);                
                return Ok();
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

        /// <summary>
        /// Retorna o Tema do Usuário Logado
        /// </summary>
        /// <returns>Tema</returns>
        [Route("api/tema")]
        public virtual IHttpActionResult GetTema()
        {
            try
            {
                var token = ActionContext.RecuperarToken();
                var tema = _usuarioService.GetTema(token.UsuarioId);

                if (string.IsNullOrWhiteSpace(tema))
                    return NotFound();

                return Ok(tema);
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

        /// <summary>
        /// Muda o Tema do Usuário
        /// </summary>
        /// <returns>Ok</returns>
        [Route("api/tema")]
        public virtual IHttpActionResult PutTema(string tema)
        {
            try
            {
                var token = ActionContext.RecuperarToken();
                _usuarioService.ChangeTema(token.UsuarioId, tema);

                return Ok(tema);
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

        /// <summary>
        /// Atualiza Perfil do Usuário
        /// </summary>
        /// <returns></returns>
        [Route("api/usuario/perfil")]
        public async Task<HttpResponseMessage> PutPerfil()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            try
            {
                var root = HttpContext.Current.Server.MapPath("~/App_Data/Temp/FileUploads");
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                if (result.FormData["model"] == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Falha ao carregar informações do Perfil");

                var usuario = JsonConvert.DeserializeObject<Usuario>(result.FormData["model"]);

                if (usuario == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Falha ao carregar informações do Perfil");

                foreach (var file in result.FileData)
                {
                    if (file.Headers.ContentDisposition.Name.Contains("foto"))
                        usuario.Foto = File.ReadAllBytes(file.LocalFileName);

                    File.Delete(file.LocalFileName);
                }

                _usuarioService.Update(usuario, usuario.Login);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ValidatorException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Retorna o Perfil do Usuário Logado
        /// </summary>
        /// <returns>Tema</returns>
        [Route("api/usuario/perfil")]
        public IHttpActionResult GetPerfil()
        {
            try
            {
                var token = ActionContext.RecuperarToken();
                var usuario = _usuarioService.Get(token.UsuarioId);

                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
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