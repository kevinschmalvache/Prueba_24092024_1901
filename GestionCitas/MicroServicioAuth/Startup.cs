using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;
using MicroServicioAuth;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(MicroServicioLogin.Startup))]

namespace MicroServicioLogin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configuración de Web API
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            // Configuración de JWT
            var issuer = "https://localhost:44354";
            var audiences = new[] { "https://localhost:44399", "https://localhost:44389", "https://localhost:44379", "https://localhost:44354" };
            var secret = TextEncodings.Base64.Decode("kglJUkB64ZFna3MbEug2f0VpebMLwAwjTG9zOCkjm7A=");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,

                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = "https://localhost:44354",
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }
            });

            app.UseWebApi(config);
        }

    }
}
