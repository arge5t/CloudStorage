using CloudStorage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Persistence.Interfaces
{
    public interface ITokenRepository
    {
        Task Save(Token token, CancellationToken cancellationToken);
    }
}
