using CloudStorage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task Create(User user, CancellationToken cancellationToken);
        Task<User> GetUserByActivationLink(string activationLink, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
    }
}
