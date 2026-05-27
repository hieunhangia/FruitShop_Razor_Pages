using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Orders;
using Service.DTOs;
using Service.DTOs.SalesStaff.Order;

namespace Service.SalesStaff;

public class OrderService(AppDbContext context, FileService fileService)
{
    public async Task<OrderDetailDto> GetOrderDetailByIdAsync(long orderId)
    {

        var orderDto = await context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .ProjectToOrderDetailDto()
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        
        if (orderDto == null)
        {
            throw new Exception("Đơn hàng không tồn tại.");
        }

        foreach (var item in orderDto.OrderItems)
        {
            Console.WriteLine(item.ProductImageFilePath);
            item.ProductImageFilePath = fileService.GetPublicFileUrl(item.ProductImageFilePath);
        }

        return orderDto;
    }

    
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