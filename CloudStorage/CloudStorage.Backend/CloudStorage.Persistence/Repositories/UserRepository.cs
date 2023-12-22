using CloudStorage.Domain.Entities;
using CloudStorage.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public UserRepository(IApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetUserByActivationLink(string activationLink, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.ActivationLink == activationLink, cancellationToken);

            return user;
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

            return user;
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
