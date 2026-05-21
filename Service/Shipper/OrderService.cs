using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Orders;
using Service.DTOs;
using Service.DTOs.Customer.Order;
using Service.DTOs.Shipper;

namespace Service.Shipper
{
    public class OrderService(AppDbContext context, OrderMapper mapper)
    {
        public class ShipperOrderFilter
        {
            public string? SearchTerm { get; set; }
        }

        public async Task<PagedAndSortedDto<OrderSummaryDto>> GetPagedOrdersForShipperAsync(
          int shipperId,
          PagedAndSortedRequest<OrderFilterDto> request)
        {
            var query = context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderStatus == OrderStatus.Shipping && o.ShipperId == shipperId);

            if (request.Filter != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
                {
                    query = query.Where(o => o.Id.ToString().Contains(request.Filter.SearchTerm.Trim()));
                }

                if (request.Filter.FromDate.HasValue)
                {
                    var fromDate = request.Filter.FromDate.Value.Date;
                    query = query.Where(o => o.OrderDate >= fromDate);
                }

                if (request.Filter.ToDate.HasValue)
                {
                    var toDate = request.Filter.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(o => o.OrderDate <= toDate);
                }

                if (request.Filter.PaymentMethod.HasValue)
                {
                    query = query.Where(o => o.PaymentMethod == (PaymentMethod)request.Filter.PaymentMethod.Value);
                }
            }

            int totalCount = await query.CountAsync();

            string sortColumn = request.SortColumn?.ToLower().Trim() ?? "orderdate";
            bool isDesc = request.SortDirection == SortDirection.Descending;

            query = sortColumn switch
            {
                "orderdate" => isDesc ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate),
                "totalamount" => isDesc ? query.OrderByDescending(o => o.TotalAmount) : query.OrderBy(o => o.TotalAmount),
                "id" => isDesc ? query.OrderByDescending(o => o.Id) : query.OrderBy(o => o.Id),
                _ => query.OrderByDescending(o => o.OrderDate)
            };

            var orders = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedAndSortedDto<OrderSummaryDto>(
                mapper.ToOrderSummaryDtoList(orders),
                totalCount,
                request.PageIndex,
                request.PageSize,
                request.SortColumn ?? "OrderDate",
                request.SortDirection ?? SortDirection.Descending
            );
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

   

        public async Task AdvanceShippingStatusAsync(long orderId, int shipperId)
        {
            var order = await context.Orders
                  .Include(o => o.OrderShippings)
                  .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại.");
            }

            if (order.ShipperId != shipperId)
            {
                throw new Exception("Lỗi bảo mật: Bạn không có quyền cập nhật trạng thái tiến độ của đơn hàng này!");
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
                var customer = await context.CustomerData.FindAsync(order.CustomerId);
                customer?.LoyaltyPoints += order.LoyaltyPointsEarned;
            }
            else
            {
                throw new Exception("Đơn hàng đã hoàn thành xong toàn bộ lộ trình giao nhận.");
            }

            await context.SaveChangesAsync();
        }

        public async Task<PagedAndSortedDto<OrderSummaryDto>> GetDeliveredOrdersForShipperAsync(
    int shipperId,
    PagedAndSortedRequest<OrderFilterDto> request)
        {
            // Lọc theo trạng thái ĐÃ GIAO THÀNH CÔNG (Delivered)
            var query = context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderStatus == OrderStatus.Delivered && o.ShipperId == shipperId);

            // [Giữ nguyên bộ lọc Search theo Mã đơn, Từ ngày, Đến ngày y hệt hàm cũ]
            if (request.Filter != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
                {
                    query = query.Where(o => o.Id.ToString().Contains(request.Filter.SearchTerm.Trim()));
                }
                if (request.Filter.FromDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate >= request.Filter.FromDate.Value.Date);
                }
                if (request.Filter.ToDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate <= request.Filter.ToDate.Value.Date.AddDays(1).AddTicks(-1));
                }
            }

            int totalCount = await query.CountAsync();

            // Sắp xếp chữ thường
            string sortColumn = request.SortColumn?.ToLower().Trim() ?? "orderdate";
            bool isDesc = request.SortDirection == SortDirection.Descending;
            query = sortColumn switch
            {
                "orderdate" => isDesc ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate),
                "totalamount" => isDesc ? query.OrderByDescending(o => o.TotalAmount) : query.OrderBy(o => o.TotalAmount),
                "id" => isDesc ? query.OrderByDescending(o => o.Id) : query.OrderBy(o => o.Id),
                _ => query.OrderByDescending(o => o.OrderDate)
            };

            var orders = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedAndSortedDto<OrderSummaryDto>(
                mapper.ToOrderSummaryDtoList(orders),
                totalCount,
                request.PageIndex,
                request.PageSize,
                request.SortColumn ?? "OrderDate",
                request.SortDirection ?? SortDirection.Descending
            );
        }

    }
}
