using System;
using BookCollabSaaS.Domain.User;

namespace BookCollabSaaS.Infrastructure.Data.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<RoleEntity>> GetAllAsync();

    Task<RoleEntity> GetByIdAsync(Guid id);

    Task<RoleEntity> GetByNameAsync(string name);

    Task AddAsync(RoleEntity role);
}
