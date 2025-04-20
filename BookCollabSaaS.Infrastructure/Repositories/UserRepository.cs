using System;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Domain.User;
using BookCollabSaaS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookCollabSaaS.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserEntity> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(UserEntity product)
    {
        await _context.Users.AddAsync(product);
        await _context.SaveChangesAsync();
    }
}
