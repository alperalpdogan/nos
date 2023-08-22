using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Services.Configuration;

namespace Nop.Plugin.Widgets.Nos.Services
{
    public class NosService
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        public NosService(IStaticCacheManager staticCacheManager,
                          IRepository<Order> orderRepository,
                          IRepository<OrderItem> orderItemRepository)
        {
            _staticCacheManager = staticCacheManager;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<int> GetNumberOfSalesAsync(int productId, int numberOfMonths)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NosDefaults.NumberOfSalesForProductCacheKey, productId, numberOfMonths);

            return await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var orders = _orderRepository.Table
                .Where(o => o.CreatedOnUtc >= DateTime.UtcNow.AddMonths(-numberOfMonths));

                var productNos = _orderItemRepository
                .Table.Where(oi => orders.Select(o => o.Id).Contains(oi.OrderId))
                .Where(oi => oi.ProductId == productId)
                .Sum(oi => oi.Quantity);

                return productNos;
            });
        }

        public async Task RemoveFromCacheAsync(List<int> productIds)
        {
            foreach (var productId in productIds.Distinct())
            {
                await _staticCacheManager.RemoveAsync(NosDefaults.NumberOfSalesForProductCacheKey, productId);
            }
        }
    }
}
