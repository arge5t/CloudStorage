using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Persistence.Interfaces
{
    public interface IBaseDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
