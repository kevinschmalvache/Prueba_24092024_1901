using MicroServicioLogin.Application.Interfaces;
using MicroServicioLogin.Domain.Interfaces;
using MicroServicioLogin.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MicroServicioLogin.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _usuarioRepository.GetByUsername(username);
            return user != null && user.Password == password;
        }

        public void RegisterUser(string username, string password, string email)
        {
            var user = new Usuario
            {
                Username = username,
                Password = password,
                Email = email,
                FechaCreacion = DateTime.Now
            };

            _usuarioRepository.Add(user);
        }

        public static string GenerateSecretKey(int length = 32)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray);
            }
        }

        public string GenerateToken(string username)
        {
            //var secretKey = GenerateSecretKey();
            var issuer = "https://localhost:44354"; // Cambia esto a tu dominio
            var audiences = new[] { "https://localhost:44399", "https://localhost:44389", "https://localhost:44379", "https://localhost:44354" }; // Lista de audiencias locales
            var secret = "kglJUkB64ZFna3MbEug2f0VpebMLwAwjTG9zOCkjm7A="; // Cambia esto a tu clave secreta
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: "https://localhost:44354", // Unir las audiencias en una cadena separada por comas
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Tiempo de expiración
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
