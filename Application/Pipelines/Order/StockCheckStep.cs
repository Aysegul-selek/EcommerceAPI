using Application.Dtos.Order;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Pipelines.Order
{
    public class StockCheckStep : IOrderPipelineStep
    {
        private readonly IProductRepository _productRepository;

        public StockCheckStep(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

       

        public async Task<Domain.Entities.Order> ExecuteAsync(Domain.Entities.Order order, CreateOrderDto request)
        {
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new Exception($"Ürün bulunamadı. ProductId: {item.ProductId}");
                if (item.Quantity <= 0) throw new Exception("Geçersiz miktar");
                if (product.Stok < item.Quantity) throw new Exception($"Yetersiz stok. ProductId: {item.ProductId}");
            }

            return order;
        }
    }
}
