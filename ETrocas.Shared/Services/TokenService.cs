using ETrocas.Domain.Entities;
using ETrocas.Shared.Configuration;
using ETrocas.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ETrocas.Shared.Services
{
    public class TokenService(IOptions<TokenConfig> config) : ITokenService
    {
        private readonly TokenConfig _config = config.Value;

        public string Gerar(Usuario usuario)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Key);
            var credentials = new SigningCredentials(
                                                     new SymmetricSecurityKey(key),
                                                     SecurityAlgorithms.HmacSha256Signature
                                                     );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GerarClaims(usuario),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private ClaimsIdentity GerarClaims(Usuario usuario)  
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome));
            ci.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));

            return ci;
        }
    }
}