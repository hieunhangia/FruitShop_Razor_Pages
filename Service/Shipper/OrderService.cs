using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Orders;
using Service.DTOs.Customer.Order;

namespace Service.Shipper
{
    public class OrderService(AppDbContext context, OrderMapper mapper)
    {
        public async Task<List<OrderSummaryDto>> GetAvailableOrdersForShipper(int shipperId) 
        {
            var orders = await context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderStatus ==  Repository.Constants.OrderStatus.Shipping
                && o.ShipperId == shipperId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return mapper.ToOrderSummaryDtoList(orders);


        }
        public async Task<OrderDetailDto> GetShipperOrderDetailAsync(long orderId)
        {
            var order = await context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderShippings)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại trong hệ thống.");
            }

            return mapper.ToOrderDetailDto(order);
        }

        public async Task StartShippingAsync(long orderId) 
        {
            var order = await context.Orders
              .Include(o=> o.OrderShippings)
              .FirstOrDefaultAsync(o => o.Id == orderId);

            if(order == null)
            {
                throw new Exception("Đơn hàng không tồn tại");
            }

            order.OrderStatus = Repository.Constants.OrderStatus.Delivered;

            order.OrderShippings.Add(new Repository.Models.Orders.OrderShipping
            {
                ShippingStatus = Repository.Constants.ShippingStatus.Delivered,
                OccurredAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }

        public async Task AdvanceShippingStatusAsync(long orderId)
        {
            var order = await context.Orders
                  .Include(o => o.OrderShippings)
                  .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại.");
            }
            var latestShipping = order.OrderShippings?
        .OrderByDescending(os => os.OccurredAt)
        .FirstOrDefault();

            if(latestShipping == null)
            {
                order.OrderShippings.Add(new Repository.Models.Orders.OrderShipping
                {
                    ShippingStatus = Repository.Constants.ShippingStatus.PickingUp,
                    OccurredAt = DateTime.UtcNow
                });
            }
            else if (latestShipping.ShippingStatus == ShippingStatus.PickingUp)
            {
                order.OrderShippings.Add(new OrderShipping
                {
                    ShippingStatus = ShippingStatus.PickedUp,
                    OccurredAt = DateTime.Now
                });
            }
            else if (latestShipping.ShippingStatus == ShippingStatus.PickedUp)
            {
                order.OrderShippings.Add(new OrderShipping
                {
                    ShippingStatus = ShippingStatus.Shipping,
                    OccurredAt = DateTime.Now
                });
            }
            else if (latestShipping.ShippingStatus == ShippingStatus.Shipping)
            {
                order.OrderShippings.Add(new OrderShipping
                {
                    ShippingStatus = ShippingStatus.Delivered,
                    OccurredAt = DateTime.Now
                });

                order.OrderStatus = OrderStatus.Delivered;
            }
            else
            {
                throw new Exception("Đơn hàng đã hoàn thành xong toàn bộ lộ trình giao nhận.");
            }

            await context.SaveChangesAsync();
        }
       
    }
}
