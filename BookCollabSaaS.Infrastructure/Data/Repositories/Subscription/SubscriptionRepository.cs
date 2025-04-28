using System;
using System.Threading.Tasks;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Domain.Subscription;
using Microsoft.EntityFrameworkCore;

namespace BookCollabSaaS.Infrastructure.Data.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SubscriptionEntity subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubscriptionEntity>> GetAllAsync()
        {
            return await _context.Subscriptions.ToListAsync();
        }


        public async Task<SubscriptionEntity> GetByUserIdAsync(Guid userId)
        {
            return await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task UpdateAsync(SubscriptionEntity subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
