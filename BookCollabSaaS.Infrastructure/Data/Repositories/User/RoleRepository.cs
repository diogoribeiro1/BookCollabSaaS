using System;
using BookCollabSaaS.Domain.User;
using BookCollabSaaS.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookCollabSaaS.Infrastructure.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoleEntity>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<RoleEntity> GetByIdAsync(Guid id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<RoleEntity> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task AddAsync(RoleEntity role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }

}