namespace Application.Pipelines.Order
{
    public interface IOrderPipelineStep
    {
        Task<Domain.Entities.Order> ExecuteAsync(Domain.Entities.Order order, Dtos.Order.CreateOrderDto request);
    }
}
