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
        private const string DevelopmentFallbackJwtKey = "ETrocas-Dev-Only-Jwt-Key-Change-In-Production";
        private readonly TokenConfig _config = config.Value;
        private readonly string? _envTokenKey = Environment.GetEnvironmentVariable("ETROCAS_TOKEN_KEY");
        private readonly string? _aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public string Gerar(Usuario usuario)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenKey = string.IsNullOrWhiteSpace(_envTokenKey) ? _config.Key : _envTokenKey;
            var isDevelopmentEnvironment = string.Equals(_aspNetCoreEnvironment, "Development", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(tokenKey) && isDevelopmentEnvironment)
                tokenKey = DevelopmentFallbackJwtKey;

            if (string.IsNullOrWhiteSpace(tokenKey))
                throw new InvalidOperationException("A chave JWT não foi configurada.");

            var key = Encoding.ASCII.GetBytes(tokenKey);
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