﻿using System;
using System.Linq;
using System.Web.Http.Controllers;
using Newtonsoft.Json;

namespace Kite.Base.Api.Token
{
    public static class ApiToken
    {
        public static string Secret { get; set; }

        public static void GerarSecret()
        {
            var secret = Guid.NewGuid().ToString("N").Substring(0, 20);
            Secret = secret;
        }

        public static string GerarTokenString(this Dominio.ViewModels.Token token)
        {
            if (string.IsNullOrWhiteSpace(Secret))
                throw new Exception("A chave de criptografia está ausente!");

            return JWT.JsonWebToken.Encode(token, Secret, JWT.JwtHashAlgorithm.HS256);
        }

        public static string RecuperarTokenString(this HttpActionContext actionContext)
        {
            return actionContext.Request.Headers.GetValues("Token").FirstOrDefault();
        }

        public static Dominio.ViewModels.Token RecuperarToken(this HttpActionContext actionContext)
        {
            try
            {
                var tokenString = actionContext.Request.Headers.GetValues("Token").FirstOrDefault();
                var json = JWT.JsonWebToken.Decode(tokenString, Secret);
                return JsonConvert.DeserializeObject<Dominio.ViewModels.Token>(json);
            }
            catch (JWT.SignatureVerificationException)
            {
                Console.Out.WriteLine("Token Inválido!");
            }
            return null;
        }
    }
}