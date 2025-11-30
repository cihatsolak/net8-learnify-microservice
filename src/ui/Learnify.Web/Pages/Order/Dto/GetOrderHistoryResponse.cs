namespace Learnify.Web.Pages.Order.Dto;

public record GetOrderHistoryResponse(DateTime Created, decimal TotalPrice, List<OrderItemViewModel> Items);
