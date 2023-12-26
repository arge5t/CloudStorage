using CloudStorage.Domain.Dtos;
using CloudStorage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(string id, string name, string email);
        Task<Guid> Create(Guid userId, string jwt);
        Task<Guid> Edit(Guid userId, string jwt);
        Task Remove(Guid userId);
    }
}
