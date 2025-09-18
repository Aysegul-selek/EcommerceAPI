using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class IdempotencyRequestRepository : RepositoryBase<IdempotencyRequest>, IIdempotencyRequestRepository
    {
        public IdempotencyRequestRepository(AppDbContext context) : base(context) { }

        public async Task<IdempotencyRequest?> GetByKeyAsync(string key)
        {
            return await _context.IdempotencyRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == key);
        }

        public async Task AddRequestAsync(IdempotencyRequest request)
        {
            await _context.IdempotencyRequests.AddAsync(request);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
