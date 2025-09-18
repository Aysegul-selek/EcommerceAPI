namespace Domain.Entities
{
    public class IdempotencyRequest : BaseEntity
    {
        public string Key { get; set; } = string.Empty;
        public string ResponseData { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
