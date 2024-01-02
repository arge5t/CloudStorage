using CloudStorage.Domain.Dtos;
using CloudStorage.Domain.Entities;
using CloudStorage.Persistence.Interfaces;
using CloudStorage.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudStorage.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly Jwt _jwt;

        public TokenService(ITokenRepository tokenRepository, Jwt jwt)
        {
            _tokenRepository = tokenRepository;
            _jwt = jwt;
        }

        public async Task<Guid> Create(Guid userId, string jwt)
        {
            try
            {
                var entity = await _tokenRepository.GetTokenByUserId(userId, _cancellationToken);

                if (entity != null)
                {
                    await _tokenRepository.Delete(entity, _cancellationToken);
                }

                var token = new Token()
                {
                    Id = Guid.NewGuid(),
                    Refresh = Guid.NewGuid(),
                    Access = jwt,
                    UserId = userId,
                };

                await _tokenRepository.Save(token, _cancellationToken);

                return token.Refresh;

            }
            catch (Exception ex)
            {
                throw new Exception($"Token Create: {ex.Message}");
            }
        }

        public async Task<Guid> Edit(Guid userId, Guid refreshToken, string jwt)
        {
            try
            {
                var token = await _tokenRepository.GetToken(userId, refreshToken, _cancellationToken);

                if (token == null)
                {
                    throw new Exception("Token not found");
                }

                token.Access = jwt;
                token.Refresh = Guid.NewGuid();

                await _tokenRepository.Update(token, _cancellationToken);

                return token.Refresh;
            }
            catch (Exception ex)
            {
                throw new Exception($"Edit token error: {ex.Message}");
            }
        }

        public async Task Remove(Guid userId, Guid refreshToken)
        {
            try
            {
                var token = await _tokenRepository.GetToken(userId, refreshToken, _cancellationToken);

                if (token == null)
                {
                    throw new Exception("Token not found");
                }

                await _tokenRepository.Delete(token, _cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Remove token error: {ex.Message}");
            }
        }

        public string GenerateToken(string id, string name, string email)
        {
            var identity = GetIdentity(id, name, email);

            var expirationTime = DateTime.Now.Add(_jwt.ExpirationTime);

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtPayload payload = new JwtPayload(_jwt.Issuer, _jwt.Audience, identity.Claims, DateTime.Now, expirationTime);

            JwtHeader header = new JwtHeader(credentials);

            JwtSecurityToken jwtToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private ClaimsIdentity GetIdentity(string id, string name, string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name,name),
                new Claim(ClaimsIdentity.DefaultNameClaimType,email)
            };

            return new ClaimsIdentity(claims, "Token");
        }
    }
}
