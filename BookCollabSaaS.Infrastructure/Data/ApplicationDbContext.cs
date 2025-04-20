using System;
using BookCollabSaaS.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace BookCollabSaaS.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
}
