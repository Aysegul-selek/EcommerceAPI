using Application.Dtos.Order;
using Domain.Entities;

namespace Application.Pipelines.Order
{
    public class TotalCalculationStep : IOrderPipelineStep
    {
        public Task<Domain.Entities.Order> ExecuteAsync(Domain.Entities.Order order, CreateOrderDto request)
        {
            order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            return Task.FromResult(order);
        }
    }
}
