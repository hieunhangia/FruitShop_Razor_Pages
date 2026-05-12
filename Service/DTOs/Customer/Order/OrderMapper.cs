using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Order;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class OrderMapper
{
    public partial OrderSummaryDto ToOrderSummaryDtoBasic(Repository.Models.Orders.Order order);

    public partial List<OrderSummaryDto> ToOrderSummaryDtoList(List<Repository.Models.Orders.Order> orders);
}