using Application.Dtos.Order;
using Domain.Entities;

namespace Application.Pipelines.Order
{
    public class DiscountStep : IOrderPipelineStep
    {
        public Task<Domain.Entities.Order> ExecuteAsync(Domain.Entities.Order order, CreateOrderDto request)
        {
            if (request.Discount is not null)
            {
                if (request.Discount.Type == "Percentage")
                {
                    order.Total -= order.Total * (request.Discount.Amount / 100);
                }
                else if (request.Discount.Type == "Fixed")
                {
                    order.Total -= request.Discount.Amount;
                }

                if (order.Total < 0) order.Total = 0;
            }

            return Task.FromResult(order);
        }
    }
}
