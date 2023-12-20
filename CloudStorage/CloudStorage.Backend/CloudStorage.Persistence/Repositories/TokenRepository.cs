using CloudStorage.Domain.Entities;
using CloudStorage.Persistence.Interfaces;

namespace CloudStorage.Persistence.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public TokenRepository(IApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task Save(Token token, CancellationToken cancellationToken)
        {
            await _dbContext.Tokens.AddAsync(token, cancellationToken); 
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
