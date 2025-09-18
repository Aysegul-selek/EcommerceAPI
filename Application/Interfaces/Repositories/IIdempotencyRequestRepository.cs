using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IIdempotencyRequestRepository : IRepositoryBase<IdempotencyRequest>
    {
        Task<IdempotencyRequest?> GetByKeyAsync(string key);
        Task AddRequestAsync(IdempotencyRequest request);
        Task SaveChangesAsync();
    }
}
