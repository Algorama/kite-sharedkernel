using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using JWT;
using SharedKernel.Api.Security;

namespace SharedKernel.Api.Filters
{
    public class UserAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Domain.Dtos.Token token = null;
            try
            {
                token = actionContext.RecuperarToken();
            }
            catch (SignatureVerificationException)
            {
                TokenExpirado(actionContext);
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    ex);
            }

            if (token == null || DateTime.Now > token.DataExpiracao)
                TokenExpirado(actionContext);

            base.OnActionExecuting(actionContext);
        }

        private static void TokenExpirado(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Unauthorized,
                "Usuário não autorizado!");
        }
    }
}