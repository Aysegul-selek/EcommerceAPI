using Application.Dtos.Order;
using Application.Interfaces.Repositories;
using Application.Pipelines.Order;
using Domain.Entities;
using DomainOrder = Domain.Entities.Order;

namespace Application.Pipelines.Order
{
    public class OrderFactory
    {
        private readonly List<IOrderPipelineStep> _steps;
        private readonly IProductRepository _productRepository;

        public OrderFactory(IEnumerable<IOrderPipelineStep> steps, IProductRepository productRepository)
        {
            _steps = steps.ToList();
            _productRepository = productRepository;
        }

        public async Task<DomainOrder> CreateAsync(CreateOrderDto request, Func<long, Task<decimal>> getProductPrice)
        {
            var order = new DomainOrder
            {
                CreatedDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            foreach (var item in request.Items)
            {
                var price = await getProductPrice(item.ProductId); // fiyat buradan geliyor

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = price
                });
            }

            foreach (var step in _steps)
            {
                order = await step.ExecuteAsync(order, request);
            }

            return order;
        }

    }
}
