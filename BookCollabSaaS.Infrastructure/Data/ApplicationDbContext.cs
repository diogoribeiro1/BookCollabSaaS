using System;
using BookCollabSaaS.Domain.Subscription;
using BookCollabSaaS.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace BookCollabSaaS.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired();

            entity.HasMany(u => u.Roles)
                  .WithMany();
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
        });
    }
}
