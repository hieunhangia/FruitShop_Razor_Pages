using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Orders;
using Service.DTOs;
using Service.DTOs.SalesStaff.Order;

namespace Service.SalesStaff;

public class OrderService(AppDbContext context)
{
    public async Task<PagedAndSortedDto<OrderListDto>> GetOrderListAsync(PagedAndSortedRequest<OrderFilter> request)
    {
        var query = context.Orders.AsNoTracking().AsQueryable();

        request.SortColumn ??= nameof(Order.OrderDate);
        request.SortDirection ??= SortDirection.Descending;

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return new PagedAndSortedDto<OrderListDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ProjectToOrderListDto()
            .ToListAsync();

        return new PagedAndSortedDto<OrderListDto>(items, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }
}