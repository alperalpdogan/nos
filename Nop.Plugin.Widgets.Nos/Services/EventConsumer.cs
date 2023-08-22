using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Services.Events;
using Nop.Services.Orders;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Nos.Services
{
    public class EventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly NosService _nosService;
        private readonly IOrderService _orderService;

        public EventConsumer(NosService nosService,
                             IOrderService orderService)
        {
            _nosService = nosService;
            _orderService = orderService;
        }

        /// <summary>
        /// Remove product nos entry from cache when order is placed
        /// </summary>
        public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
        {
            var orderItems = await _orderService.GetOrderItemsAsync(eventMessage.Order.Id);
            var productIds = orderItems.Select(o => o.ProductId).ToList();
            await _nosService.RemoveFromCacheAsync(productIds);
        }
    }
}
