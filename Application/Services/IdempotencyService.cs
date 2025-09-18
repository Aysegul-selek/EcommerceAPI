using Application.Interfaces.Repositories;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Services
{
    public class IdempotencyService
    {
        private readonly IIdempotencyRequestRepository _repo;

        public IdempotencyService(IIdempotencyRequestRepository repo)
        {
            _repo = repo;
        }

        public async Task<T?> GetCachedResponseAsync<T>(string key) where T : class
        {
            var existing = await _repo.GetByKeyAsync(key);
            if (existing == null) return null;

            return JsonConvert.DeserializeObject<T>(existing.ResponseData);
        }

        public async Task SaveResponseAsync<T>(string key, T response) where T : class
        {
            var entity = new IdempotencyRequest
            {
                Key = key,
                ResponseData = JsonConvert.SerializeObject(response),
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddRequestAsync(entity);
            await _repo.SaveChangesAsync();
        }
    }
}
