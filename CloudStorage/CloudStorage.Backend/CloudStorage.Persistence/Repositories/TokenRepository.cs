using CloudStorage.Domain.Entities;
using CloudStorage.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Persistence.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public TokenRepository(IApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<Token> GetTokenByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(token => token.UserId == userId, cancellationToken);

            return token;
        }

        public async Task Save(Token token, CancellationToken cancellationToken)
        {
            await _dbContext.Tokens.AddAsync(token, cancellationToken); 
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(Token token, CancellationToken cancellationToken)
        {
            _dbContext.Tokens.Update(token);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
