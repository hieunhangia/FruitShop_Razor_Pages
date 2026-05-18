using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Repository;
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

            order.OrderStatus = Repository.Constants.OrderStatus.Shipping;

            order.OrderShippings.Add(new Repository.Models.Orders.OrderShipping
            {
                ShippingStatus = Repository.Constants.ShippingStatus.Shipping,
                OccurredAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }

       
    }
}
