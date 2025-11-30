namespace Learnify.Web.Pages.Basket.ViewModel;

public record BasketViewModelItem(
    Guid Id,
    string PictureUrl,
    string Name,
    decimal Price,
    decimal PriceWithDiscountRate);