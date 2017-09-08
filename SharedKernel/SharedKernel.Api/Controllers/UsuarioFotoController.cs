using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using SharedKernel.DependencyInjection;
using SharedKernel.Domain.Services;
using SharedKernel.Domain.Validation;

namespace SharedKernel.Api.Controllers
{
    /// <summary>
    /// Recurso para Gerenciar os Usuários da Aplicação
    /// </summary>	
    public class UsuarioFotoController : ApiController
    {
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Recurso para Gerenciar os Usuários da Aplicação
        /// </summary>
        public UsuarioFotoController()
	    {
	        _usuarioService = Kernel.Get<IUsuarioService>();
        }

        /// <summary>
        /// Retorna a Foto do Usuário
        /// </summary>
        /// <returns>Foto</returns>
        [Route("api/usuario/perfil/foto")]
        public HttpResponseMessage GetFoto(string login)
        {
            var response = new HttpResponseMessage();
            try
            {
                var usuario = _usuarioService.GetAll(x => x.Login == login).FirstOrDefault();

                if (usuario == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.StatusCode = HttpStatusCode.OK;
                var ms = new MemoryStream(usuario.Foto);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                return response;
            }
            catch (ValidatorException ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent(ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
                return response;
            }
        }
    }
}